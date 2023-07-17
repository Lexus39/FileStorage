using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.Core
{
    public class FileService
    {
        private readonly IFileModelRepository _models;

        public FileService(IFileModelRepository models)
        {
            _models = models;
        }

        public async Task AddFile(CreateFileParameters parameters)
        {
            var file = parameters.File;
            string extension = Path.GetExtension(file.FileName);
            string untrustedName = Path.GetFileNameWithoutExtension(file.FileName);
            string trustedName = Path.GetRandomFileName();
            var model = new FileModel()
            {
                FileId = 0,
                UntrustedName= untrustedName,
                TrustedName= trustedName,
                ContentType= file.ContentType
            };
            var savePath = Path.Combine(parameters.Path, trustedName);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await parameters.File.CopyToAsync(stream);
            }
            await _models.AddFileModel(model); 
        }

        public async Task<FileStream> DownloadFile(string fileName, string pathToDirectory)
        {
            var model = await _models.GetFileModelByUntrustedName(fileName);
            if (model == null)
            {
                throw new ArgumentException();
            }
            var path = Path.Combine(pathToDirectory, model.TrustedName);
            return new FileStream(path, FileMode.Open);
        }

        public async Task<FileModel> GetFileModel(string untrustedName)
            => await _models.GetFileModelByUntrustedName(untrustedName);

        public async Task<List<FileModel>> ListFileModels() => await _models.ListFileModels();
    }
}
