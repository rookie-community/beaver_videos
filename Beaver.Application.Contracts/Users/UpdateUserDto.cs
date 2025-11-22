using Volo.Abp.Application.Dtos;

namespace Beaver.Users
{
    public class UpdateUserDto : EntityDto<Guid>
    {
        public string UserName { get; set; }
    }
}
