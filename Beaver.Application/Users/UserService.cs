using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace Beaver.Users
{
    public class UserService : BeaverAppService, IUserService, ITransientDependency
    {
        private readonly IUserRepository<IdentityUser> _userRepository;
        private readonly IdentityUserManager _userManager;

        public UserService(IUserRepository<IdentityUser> userRepository, IdentityUserManager userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            var user = ObjectMapper.Map<CreateUserDto, IdentityUser>(input);
            await _userManager.CreateAsync(user, IdentityDataSeedContributor.AdminPasswordDefaultValue);
            return ObjectMapper.Map<IdentityUser, UserDto>(user);
        }

        public Task DeleteAsync(Guid id)
        {
            return _userRepository.DeleteAsync(id);
        }

        public async Task<UserDto> GetAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            return ObjectMapper.Map<IdentityUser, UserDto>(user);
        }

        public async Task<PagedResultDto<UserDto>> GetListAsync(GetUserListDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(input.UserName);
            }

            var users = await _userRepository.GetPagedListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting
            );

            var totalCount = input.Filter == null
                ? await _userRepository.GetCountAsync()
                : await _userRepository.GetCountAsync(input.Filter);

            return new PagedResultDto<UserDto>(
                totalCount,
                ObjectMapper.Map<List<IdentityUser>, List<UserDto>>(users)
            );
        }

        public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto input)
        {
            var user = await _userRepository.GetAsync(id);

            if (user.UserName != input.UserName)
            {
                //_userManager.ChangeEmailAsync(user, input.UserName);
            }

            //user.UserName = input.UserName;

            await _userRepository.UpdateAsync(user);
            return ObjectMapper.Map<IdentityUser, UserDto>(user);
        }
    }
}
