using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dr_Hesabi.Classes.ViewModel
{
    public class ContactUsViewModel
    {
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
    }

    public class TopSiteViewModel
    {
        [Display(Name = "نام سایت")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string NameSite { get; set; }

        [Display(Name = " (شهر یا استان) نام دوم سایت")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string NameSite2 { get; set; }

        [Display(Name = "لگوی سایت")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string ImgLogo { get; set; }
    }
}
