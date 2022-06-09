using MoviesLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesLibrary.MemoryCacheModel
{
    public class TopMomoryCache : BaseMemoryCacheModel
    {
        public List<TopModel> Tops { get; set; } = new List<TopModel>();
    }
}
