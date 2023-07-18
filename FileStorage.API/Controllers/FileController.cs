using FileStorage.Core;
using FileStorage.Core.Services;
using FileStorage.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace FileStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly FileService _fileStorage;
        private readonly LinkService _linkService;

        public FileController(FileService fileStorage, IWebHostEnvironment env, LinkService linkService)
        {
            _fileStorage = fileStorage;
            _env = env;
            _linkService = linkService;
        }

        /// <summary>
        /// Загружает несколько файлов. Максимальный размер файла - 30000000
        /// </summary>
        /// <param name="files"></param>
        /// <returns>Возвращает результаты загрузки</returns>
        /// <response code="200">Если все файлы загрузились</response>
        /// <response code="500">Если хотя бы один из файлов не загрузился(слишком большой размер, файл с таким именем уже существует)</response>
        [HttpPost("upload-multiple-file")]
        public async Task<ActionResult<List<UploadResult>>> CreateFile(IList<IFormFile> files)
        {
            var uploadResults = new List<UploadResult>();
            foreach (var file in files)
            {
                var uploadResult = new UploadResult();
                uploadResult.Name = file.FileName;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Development", "Files");
                var createParameters = new CreateFileParameters()
                {
                    File = file,
                    Path = path
                };
                await _fileStorage.UploadFile(createParameters);             
                uploadResult.IsUpload = true;
                uploadResults.Add(uploadResult);
            }
            return Ok(uploadResults);
        }

        /// <summary>
        /// Возвращает список с названиями и типом загруженных файлов
        /// </summary>
        /// <returns>Возвращает список FileModelResponse</returns>
        /// <response code="200"></response>
        [HttpGet("list-files")]
        public async Task<IActionResult> ListFiles()
        {
            return Ok(await _fileStorage.ListFileModel());
        }

        /// <summary>
        /// Скачивает файл с указанным именем
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>File</returns>
        /// <response code="200">Если удалось найти файл</response>
        /// <response code="404">Если не удалось найти файл с таким именем</response>
        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var model = await _fileStorage.GetFileModel(fileName);
            if (model == null)
                return NotFound();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Development", "Files");
            var memory = new MemoryStream();
            using (var stream = await _fileStorage.DownloadFile(fileName, path))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, model.ContentType, fileName);
        }

        /// <summary>
        /// Генерирует одноразовую ссылку на файл
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Возвращает строку</returns>
        /// <response code="200">Если файл с указанным именем существует</response>
        /// <response code="404">Если файл с таким именем не найден</response>
        [HttpGet("generate-link/{fileName}")]
        public async Task<IActionResult> CreateOneTimeLink(string fileName)
        {
            if (await _fileStorage.GetFileModel(fileName) == null)
                return NotFound();
            return Ok(await _linkService.CreateOneTimeLink(fileName));
        }

        /// <summary>
        /// Скачивает файл по однорозавой ссылке
        /// </summary>
        /// <param name="uri">Одноразовая ссылка</param>
        /// <returns>File</returns>
        /// <response code="200">Если одноразовая ссылка активна</response>
        /// <response code="404">Если ссылка не действует</response>
        [HttpGet("one-time-link/{uri}")]
        public async Task<IActionResult> DownloadFileByOneTimeLink(string uri)
        {
            var model = await _linkService.GetFileModelByUri(uri);
            if (model == null) 
                return NotFound();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Development", "Files");
            var memory = new MemoryStream();
            using (var stream = await _fileStorage.DownloadFile(model.UntrustedName, path))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            await _linkService.DeleteOneTimeLink(uri);
            return File(memory, model.ContentType, model.UntrustedName);
        }
    }
}
