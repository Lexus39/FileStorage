using FileStorage.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.DAL
{
    public class OneTimeLinkConfiguration : IEntityTypeConfiguration<OneTimeLink>
    {
        public void Configure(EntityTypeBuilder<OneTimeLink> builder)
        {
            builder.HasKey(link => link.LinkId);

            builder.HasIndex(link => link.LinkId);

            builder.HasAlternateKey(link => link.Uri);

            builder.HasOne<FileModel>().WithMany().HasForeignKey(link => link.FileModelId);
        }
    }
}
