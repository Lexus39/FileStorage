using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.Shared
{
    public class UpdateResult
    {
        public string Name { get; set; } = null!;
        public string StoredName { get; set; } = null!;
        public bool IsUpload { get; set; }
    }
}
