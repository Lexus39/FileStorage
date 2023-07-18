using FileStorage.Core;
using FileStorage.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.DAL.Repositories
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

        public async Task<FileModel> GetFileModelByUntrustedName(string untrustedName)
        {
            var model = await _context.Files.Where(file => file.UntrustedName == untrustedName).FirstOrDefaultAsync();
            if (model == null)
            {
                throw new ArgumentException();
            }
            return model;
        }

        public async Task<List<FileModel>> ListFileModels() => await _context.Files.ToListAsync();

        public async Task<FileModel> GetFileModelById(int id)
        {
            var model = await _context.Files.FirstOrDefaultAsync(file => file.FileId == id);
            if (model == null)
            {
                throw new ArgumentException();
            }
            return model;
        }
    }
}
