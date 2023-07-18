using FileStorage.Core.Interfaces;
using FileStorage.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.Core.Services
{
    public class LinkService
    {
        private readonly IOneTimeLinkRepository _links;
        private readonly IFileModelRepository _files;

        public LinkService(IOneTimeLinkRepository links, IFileModelRepository files)
        {
            _links = links;
            _files = files;
        }

        public async Task<FileModelResponse> GetFileModelByUri(string uri)
        {
            var link = await _links.GetOneTimeLink(uri);
            if (link == null)
            {
                throw new ArgumentException();
            }
            var fileModel = await _files.GetFileModelById(link.FileModelId);
            if (fileModel == null)
            {
                throw new ArgumentException();
            }
            return new FileModelResponse()
            {
                UntrustedName = fileModel.UntrustedName,
                ContentType = fileModel.ContentType
            };
        }

        public async Task<string> CreateOneTimeLink(string fileName)
        {
            var model = await _files.GetFileModelByUntrustedName(fileName);
            var link = new OneTimeLink()
            {
                LinkId = 0,
                FileModelId = model.FileId,
                Uri = Path.GetRandomFileName(),
            };
            await _links.CreateOneTimeLink(link);
            return link.Uri;
        }

        public async Task DeleteOneTimeLink(string uri)
        {
            await _links.DeleteLink(uri);
        }
    }
}
