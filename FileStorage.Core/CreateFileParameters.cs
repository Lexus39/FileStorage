using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.Core
{
    public class CreateFileParameters
    {
        public string Path { get; set; } = null!;
        public IFormFile File { get; set; } = null!;
    }
}
