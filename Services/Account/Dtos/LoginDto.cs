using Volo.Abp.Application.Dtos;

namespace Beaver.Services.Account.Dtos
{
    public class LoginDto : EntityDto
    {
        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
}
