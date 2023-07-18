using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.Core.Interfaces
{
    public interface IFileModelRepository
    {
        public Task<int> AddFileModel(FileModel model);

        public Task<FileModel> GetFileModelByUntrustedName(string untrustedName);

        public Task<List<FileModel>> ListFileModels();

        public Task<FileModel> GetFileModelById(int id);

        public Task<bool> IsFileExists(string fileName);
    }
}
