using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Beaver.Account
{
    public class SettingPasswordDto : EntityDto
    {
        /// <summary>
        /// 当前密码/旧密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "当前密码")]
        public string OldPassword { get; set; } = null!;

        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; } = null!;

        /// <summary>
        /// 确认新密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare(nameof(NewPassword), ErrorMessage = "新密码和确认密码不匹配")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
