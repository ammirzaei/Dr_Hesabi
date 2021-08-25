using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Dr_Hesabi.Classes.ViewModel
{
    public class RegisterViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا نام کاربری خود را وارد نمایید")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [MinLength(8, ErrorMessage = "مقدار {0} نباید کمتر از {1} کاراکتر باشد")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا ایمیل خود را وارد نمایید")]
        [MaxLength(250, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "لطفا ایمیل معتبر وارد نمایید")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا رمز عبور خود را وارد نمایید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [MinLength(6, ErrorMessage = "مقدار {0} نباید کمتر از {1} کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "تکرار رمز عبور")]
        [Required(ErrorMessage = "لطفا تکرار رمز عبور خود را وارد نمایید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [MinLength(6, ErrorMessage = "مقدار {0} نباید کمتر از {1} کاراکتر باشد")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "رمز های عبور با یکدیگر مغایرت دارند")]
        public string RePassword { get; set; }
    }

    public class LoginViewModel
    {
        [Display(Name = "ایمیل یا نام کاربری")]
        [Required(ErrorMessage = "لطفا ایمیل یا نام کاربری خود را وارد نمایید")]
        [MaxLength(250, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string EmailandUserName { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا رمز عبور خود را وارد نمایید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool ReMemeberMe { get; set; }

        public string ReturnUrl { get; set; }

    }

    public class ChangePasswordViewModel
    {
        [Display(Name = "رمز عبور فعلی")]
        [Required(ErrorMessage = "لطفا رمز عبور فعلی خود را وارد نمایید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [MinLength(6, ErrorMessage = "مقدار {0} نباید کمتر از {1} کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "رمز عبور جدید")]
        [Required(ErrorMessage = "لطفا رمز عبور جدید خود را وارد نمایید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [MinLength(6, ErrorMessage = "مقدار {0} نباید کمتر از {1} کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "تکرار رمز عبور جدید")]
        [Required(ErrorMessage = "لطفا تکرار رمز عبور جدید  خود را وارد نمایید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [MinLength(6, ErrorMessage = "مقدار {0} نباید کمتر از {1} کاراکتر باشد")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "رمز های عبور جدید با یکدیگر مغایرت دارند")]
        public string ReNewPassword { get; set; }
    }

    public class ChangeUserNameViewModel
    {
        [Display(Name = "نام کاربری جدید")]
        [Required(ErrorMessage = "لطفا نام کاربری جدید خود را وارد نمایید")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [MinLength(8, ErrorMessage = "مقدار {0} نباید کمتر از {1} کاراکتر باشد")]
        [Remote("VerifyUserName","Account")]
        public string UserName { get; set; }
    }

    public class ForgetPasswordViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا ایمیل خود را وارد نمایید")]
        [MaxLength(250, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "لطفا ایمیل معتبر وارد نمایید")]
        public string Email { get; set; }
    }

    public class RecoveryPasswordViewModel
    {
        [Display(Name = "رمز عبور جدید")]
        [Required(ErrorMessage = "لطفا رمز عبور جدید خود را وارد نمایید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [MinLength(6, ErrorMessage = "مقدار {0} نباید کمتر از {1} کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "تکرار رمز عبور جدید")]
        [Required(ErrorMessage = "لطفا تکرار رمز عبور جدید  خود را وارد نمایید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [MinLength(6, ErrorMessage = "مقدار {0} نباید کمتر از {1} کاراکتر باشد")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "رمز های عبور جدید با یکدیگر مغایرت دارند")]
        public string ReNewPassword { get; set; }
    }
}
