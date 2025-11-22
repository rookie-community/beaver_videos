using Volo.Abp.Application.Dtos;

namespace Beaver.Models
{
    public class LoginDto : EntityDto
    {
        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
}
