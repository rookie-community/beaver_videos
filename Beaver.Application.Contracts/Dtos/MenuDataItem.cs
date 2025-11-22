using Volo.Abp.Application.Dtos;

namespace Beaver.Dtos
{
    public class MenuItemDto : EntityDto
    {
        public string Path { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Icon { get; set; } = null!;

        public List<MenuItemDto> Children { get; set; } = new List<MenuItemDto>();
    }
}
