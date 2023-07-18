﻿using FileStorage.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.DAL
{
    public class OneTimeLinkRepository : IOneTimeLinkRepository
    {
        private readonly FileStorageDbContext _context;

        public OneTimeLinkRepository(FileStorageDbContext context)
        {
            _context = context;
        }

        public async Task CreateOneTimeLink(OneTimeLink oneTimeLink)
        {
            await _context.OneTimeLinks.AddAsync(oneTimeLink);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLinkById(int id)
        {
            await _context.OneTimeLinks.Where(link => link.LinkId == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }

        public async Task<OneTimeLink> GetOneTimeLink(string uri)
        {
            var link = await _context.OneTimeLinks.FirstOrDefaultAsync(link => link.Uri == uri);
            if (link == null)
            {
                throw new ArgumentException();
            }
            return link;
        }
    }
}