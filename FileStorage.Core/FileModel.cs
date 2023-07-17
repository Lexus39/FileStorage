using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.Core
{
    public class FileModel
    {
        public int FileId { get; set; }
        public string UntrustedName { get; set; } = null!;
        public string TrustedName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
    }
}
