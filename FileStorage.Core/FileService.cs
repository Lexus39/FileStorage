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
        private readonly IFileModelRepository _files;
        private readonly IOneTimeLinkRepository _links;

        public FileService(IFileModelRepository files, IOneTimeLinkRepository links)
        {
            _files = files;
            _links = links;
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
            await _files.AddFileModel(model); 
        }

        public async Task<FileStream> DownloadFile(string fileName, string pathToDirectory)
        {
            var model = await _files.GetFileModelByUntrustedName(fileName);
            if (model == null)
            {
                throw new ArgumentException();
            }
            var path = Path.Combine(pathToDirectory, model.TrustedName);
            return new FileStream(path, FileMode.Open);
        }

        public async Task<string> CreateOneTimeLink(FileModel model)
        {
            var randomStr = Path.GetFileName(Path.GetRandomFileName());
            var link = new OneTimeLink()
            {
                LinkId = 0,
                Uri = randomStr,
                FileModelId = model.FileId,
            };
            await _links.CreateOneTimeLink(link);
            return randomStr;
        }

        public async Task<FileStream> DownloadFileByLink(string uri, string pathToDirectory)
        {
            var link = await _links.GetOneTimeLink(uri);
            if (link == null)
            {
                throw new ArgumentException();
            }
            var model = await _files.GetFileModelById(link.FileModelId);
            if (model == null)
            {
                throw new ArgumentException();
            }
            var path = Path.Combine(pathToDirectory, model.TrustedName);
            await _links.DeleteLinkById(link.LinkId);
            return new FileStream(path, FileMode.Open);
        }

        public async Task<OneTimeLink> GetOneTimeLink(string uri) 
            => await _links.GetOneTimeLink(uri);

        public async Task<FileModel> GetFileModel(string untrustedName)
            => await _files.GetFileModelByUntrustedName(untrustedName);

        public async Task<List<FileModel>> ListFileModels() => await _files.ListFileModels();

        public async Task<FileModel> GetFileModelById(int id)
            => await _files.GetFileModelById(id);
    }
}
