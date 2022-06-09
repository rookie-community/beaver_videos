using MoviesLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesLibrary.MemoryCacheModel
{
    public class MovieDetailMemoryCache : BaseMemoryCacheModel
    {
        public MovieDetail MovieDetails { get; set; } = new MovieDetail();
    }
}
