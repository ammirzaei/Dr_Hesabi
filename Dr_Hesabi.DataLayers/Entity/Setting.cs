using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class Setting
    {
        [Key]
        [MaxLength(50)]
        public string SettingID { get; set; }

        [Display(Name = "نام سایت")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string NameSite { get; set; }

        [Display(Name = " (شهر یا استان) نام دوم سایت")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string NameSite2 { get; set; }

        [Display(Name = "توضیحات مختصر")]
        [MaxLength(200, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [DataType(DataType.MultilineText)]
        public string ShortDescription { get; set; }

        [Display(Name = "اهداف آموزشی و پرورشی")]
        [DataType(DataType.MultilineText)]
        public string Targets { get; set; }

        [Display(Name = "تاریخچه هنرستان")]
        [DataType(DataType.MultilineText)]
        public string History { get; set; }

        [Display(Name = "راهنما و نکات معلم")]
        [DataType(DataType.MultilineText)]
        public string GuideTeacher { get; set; }

        [Display(Name = "راهنما و نکات دانش آموز")]
        [DataType(DataType.MultilineText)]
        public string GuideStudent { get; set; }

        [Display(Name = "تلفن تماس")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Telephone { get; set; }

        [Display(Name = "آدرس")]
        [MaxLength(200, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Address { get; set; }

        [Display(Name = "لینک تلگرام")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Telegram { get; set; }

        [Display(Name = "عکس کد کیوآر")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string ImgCodeQR { get; set; }

        [Display(Name = "لوگو سایت")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string ImgLogo { get; set; }

        [Display(Name = "عکس درباره")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string ImgHistory { get; set; }

        [Display(Name = "ایمیل")]
        [MaxLength(250, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "لطفا ایمیل معتبر وارد نمایید")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور ایمیل")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string PasswordEmail { get; set; }

        [Display(Name = "ایمیل پشتیبانی")]
        [MaxLength(250, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "لطفا ایمیل معتبر وارد نمایید")]
        public string EmailSupport { get; set; }
    }
}
