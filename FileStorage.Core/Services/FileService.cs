using FileStorage.Core.Interfaces;
using FileStorage.Shared;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.Core.Services
{
    public class FileService
    {
        private readonly IFileModelRepository _files;

        public FileService(IFileModelRepository files)
        {
            _files = files;
        }

        public async Task UploadFile(CreateFileParameters parameters)
        {
            var file = parameters.File;
            string untrustedName = Path.GetFileNameWithoutExtension(file.FileName);
            if (await _files.IsFileExists(untrustedName))
            {
                throw new ArgumentException("File already exists!");
            }
            string trustedName = Path.GetRandomFileName();
            var model = new FileModel()
            {
                FileId = 0,
                UntrustedName = untrustedName,
                TrustedName = trustedName,
                ContentType = file.ContentType
            };
            var savePath = Path.Combine(parameters.Path, trustedName);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await parameters.File.CopyToAsync(stream);
            }
            await _files.AddFileModel(model);
        }

        public async Task<FileStream> DownloadFile(string fileName, string pathToDirectory)
        {
            var model = await _files.GetFileModelByUntrustedName(fileName);
            if (model == null)
            {
                throw new ArgumentException("File not found");
            }
            var path = Path.Combine(pathToDirectory, model.TrustedName);
            return new FileStream(path, FileMode.Open);
        }

        public async Task<FileModelResponse> GetFileModel(string untrustedName)
        {
            var fileModel = await _files.GetFileModelByUntrustedName(untrustedName);
            return new FileModelResponse()
            {
                UntrustedName = fileModel.UntrustedName,
                ContentType = fileModel.ContentType,
            };
        }

        public async Task<List<FileModelResponse>> ListFileModel()
        {
            var fileModels = await _files.ListFileModels();
            return fileModels.Select(model => new FileModelResponse()
            {
                UntrustedName = model.UntrustedName,
                ContentType = model.ContentType,
            }).ToList();
        }
    }
}
