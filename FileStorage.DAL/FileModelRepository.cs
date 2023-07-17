using FileStorage.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.DAL
{
    public class FileModelRepository : IFileModelRepository
    {
        private readonly FileStorageDbContext _context;

        public FileModelRepository(FileStorageDbContext context) 
        {
            _context = context;
        }

        public async Task<int> AddFileModel(FileModel model)
        {
            var createdModel = await _context.Files.AddAsync(model);
            await _context.SaveChangesAsync();
            return createdModel.Entity.FileId;
        }

        public Task<FileModel> GetFileModelByUntrustedName(string untrustedName)
        {
            throw new NotImplementedException();
        }
    }
}
