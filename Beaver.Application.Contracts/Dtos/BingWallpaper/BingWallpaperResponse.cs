namespace Beaver.Dtos.BingWallpaper
{
    public class BingWallpaperResponse
    {
        public List<BingImage> Images { get; set; } = new List<BingImage>();
    }

    public class BingImage
    {
        public string Startdate { get; set; } = null!;
        public string Fullstartdate { get; set; } = null!;
        public string Enddate { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string Urlbase { get; set; } = null!;
        public string Copyright { get; set; } = null!;
        public string Copyrightlink { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Quiz { get; set; } = null!;
        public bool Wp { get; set; }
        public string Hsh { get; set; } = null!;
        public int Drk { get; set; }
        public int Top { get; set; }
        public int Bot { get; set; }
    }
}
