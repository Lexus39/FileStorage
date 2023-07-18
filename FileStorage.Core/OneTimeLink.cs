using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.Core
{
    public class OneTimeLink
    {
        public int LinkId { get; set; }
        public string Uri { get; set; } = null!;
        public int FileModelId { get; set; }
    }
}
