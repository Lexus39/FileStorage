using FileStorage.Core;
using FileStorage.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly FileService _fileStorage;

        public FileController(FileService fileStorage, IWebHostEnvironment env)
        {
            _fileStorage = fileStorage;
            _env = env;
        }

        [HttpPost("upload-multiple-file")]
        public async Task<IActionResult> CreateFile(IList<IFormFile> files)
        {
            //var path = Path.Combine(_env.ContentRootPath, "Development", "Files");
            foreach (var file in files)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Development", "Files");
                var createParameters = new CreateFileParameters()
                {
                    File = file,
                    Path = path
                };
                await _fileStorage.AddFile(createParameters);
            }
            return Ok();
        }

        [HttpGet("list-files")]
        public async Task<IActionResult> ListFiles()
        {
            var fileModels = await _fileStorage.ListFileModels();
            return Ok(fileModels.Select(fileModel => new FileModelResponse()
            {
                UntrustedName = fileModel.UntrustedName,
                ContentType = fileModel.ContentType
            }).ToList());
        }

        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var model = await _fileStorage.GetFileModel(fileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Development", "Files");
            var memory = new MemoryStream();
            using (var stream = await _fileStorage.DownloadFile(fileName, path))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, model.ContentType, fileName);
        }

        [HttpGet("generate-link/{fileName}")]
        public async Task<IActionResult> CreateOneTimeLink(string fileName)
        {
            var model = await _fileStorage.GetFileModel(fileName);
            return Ok(await _fileStorage.CreateOneTimeLink(model));
        }

        [HttpGet("one-time-link/{uri}")]
        public async Task<IActionResult> DownloadFileByOneTimeLink(string uri)
        {
            var link = await _fileStorage.GetOneTimeLink(uri);
            var model = await _fileStorage.GetFileModelById(link.FileModelId);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Development", "Files");
            var memory = new MemoryStream();
            using (var stream = await _fileStorage.DownloadFileByLink(uri, path))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, model.ContentType, model.UntrustedName);
        }
    }
}
