namespace BeaverVideos.Areas.Identity.Models
{
    public class LoginModel
    {
        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
}
