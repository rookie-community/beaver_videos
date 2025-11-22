using Volo.Abp.Application.Services;

namespace Beaver.Users
{
    public interface IUserService :
    ICrudAppService< //Defines CRUD methods
        UserDto, //Used to show books
        Guid, //Primary key of the book entity
        GetUserListDto, //Used for paging/sorting
        CreateUserDto, UpdateUserDto>
    {
    }
}
