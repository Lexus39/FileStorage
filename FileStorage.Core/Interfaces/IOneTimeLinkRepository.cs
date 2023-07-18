using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.Core.Interfaces
{
    public interface IOneTimeLinkRepository
    {
        public Task<OneTimeLink> GetOneTimeLink(string uri);

        public Task CreateOneTimeLink(OneTimeLink oneTimeLink);

        public Task DeleteLinkById(int id);

        public Task DeleteLink(string uri);
    }
}
