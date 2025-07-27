namespace BeaverVideos.Dtos
{
    public class MenuDataItem
    {
        public string Path { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Icon { get; set; } = null!;

        public List<MenuDataItem> Children { get; set; } = new List<MenuDataItem>();
    }
}
