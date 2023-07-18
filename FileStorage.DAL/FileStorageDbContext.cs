using FileStorage.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.DAL
{
    public class FileStorageDbContext : DbContext
    {
        public DbSet<FileModel> Files { get; set; }
        public DbSet<OneTimeLink> OneTimeLinks { get; set; }

        public FileStorageDbContext(DbContextOptions<FileStorageDbContext> options)
            :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FileModelConfiguration());
            modelBuilder.ApplyConfiguration(new OneTimeLinkConfiguration());
        }
    }
}
