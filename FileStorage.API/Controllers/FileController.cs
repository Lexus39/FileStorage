using FileStorage.Core;
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

        [HttpPost]
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
    }
}
