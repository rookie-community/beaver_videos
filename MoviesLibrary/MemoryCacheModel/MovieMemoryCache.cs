using MoviesLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesLibrary.MemoryCacheModel
{
    public class MovieMemoryCache : BaseMemoryCacheModel
    {
        public IEnumerable<Movie> Movies { get; set; } = new List<Movie>();
    }
}
