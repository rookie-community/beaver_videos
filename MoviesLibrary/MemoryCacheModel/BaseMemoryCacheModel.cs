using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesLibrary.MemoryCacheModel
{
    public class BaseMemoryCacheModel
    {
        public Guid Guid => Guid.NewGuid();
        public string Identification { get; set; } = string.Empty;
        public DateTime Time => DateTime.Now;
    }
}
