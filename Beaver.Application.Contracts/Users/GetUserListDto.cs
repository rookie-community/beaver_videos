using Volo.Abp.Application.Dtos;

namespace Beaver.Users
{
    public class GetUserListDto: PagedAndSortedResultRequestDto
    {
        public string UserName { get; set; }
        public string Filter { get; set; }
    }
}
