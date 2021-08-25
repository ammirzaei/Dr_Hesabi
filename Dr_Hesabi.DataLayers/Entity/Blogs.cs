using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class Blogs
    {
        [Key]
        [MaxLength(50)]
        public string BlogID { get; set; }

        [Display(Name = "نام نویسنده")]
        [Required(ErrorMessage = "لطفا {0} مقاله را وارد نمایید")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]

        public string NameWriter { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} مقاله را وارد نمایید")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "توضیحات مختصر")]
        [Required(ErrorMessage = "لطفا {0} مقاله را وارد نمایید")]
        [MaxLength(200, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [DataType(DataType.MultilineText)]
        public string ShortDescription { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} مقاله را وارد نمایید")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "تصویر")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; }

        [Display(Name = "تعداد بازدید")]
        public int Visit { get; set; }

        [Display(Name = "وضعیت")]
        [Required]
        public bool IsActive { get; set; }

        [Display(Name = "تاریخ ثبت")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public DateTime DateTime { get; set; }
    }
}
