using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Beaver.Services.Account.Dtos
{
    /// <summary>
    /// 自助注册表单。注册出来的账号固定为普通用户，没有后台权限。
    /// </summary>
    public class RegisterDto : EntityDto
    {
        [Required(ErrorMessage = "请填写用户名")]
        [StringLength(64, MinimumLength = 3, ErrorMessage = "用户名长度需为 3~64 个字符")]
        [RegularExpression("^[a-zA-Z0-9_]+$", ErrorMessage = "用户名只能包含字母、数字和下划线")]
        [Display(Name = "用户名")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "请填写昵称")]
        [StringLength(64, ErrorMessage = "昵称最长 64 个字符")]
        [Display(Name = "昵称")]
        public string DisplayName { get; set; } = null!;

        [Required(ErrorMessage = "请填写密码")]
        [StringLength(64, MinimumLength = 6, ErrorMessage = "密码长度需为 6~64 个字符")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "请再次输入密码")]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare(nameof(Password), ErrorMessage = "两次输入的密码不一致")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
