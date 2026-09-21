using System.Text.Json.Serialization;

namespace Beaver.Services.Movies.Dtos
{
    public class MovieCarousel
    {
        public string Cat { get; set; } = null!;

        [JsonPropertyName("ent_id")]
        public string EntId { get; set; } = null!;
        public int Upinfo { get; set; }
        public string Publidate { get; set; } = null!;
        public string Url { get; set; } = null!;
        public List<string> Actor { get; set; } = new List<string>();
        public string Title { get; set; } = null!;
        public string Comment { get; set; } = null!;

        [JsonPropertyName("pic_lists")]
        public List<PicLists> PicLists { get; set; } = new List<PicLists>();
        public int Total { get; set; }
        public int Last { get; set; }
        public int Duration { get; set; }
        public bool Vip { get; set; }
        public string Status { get; set; } = null!;
    }

    public class PicLists
    {
        [JsonPropertyName("pic_type")]
        public string PicType { get; set; } = null!;
        public string Url { get; set; } = null!;
    }

}
