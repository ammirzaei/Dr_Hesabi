using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class Majors
    {
        [Key]
        [MaxLength(50)]
        public string MajorID { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} رشته را وارد نمایید")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "توضیحات مختصر")]
        [Required(ErrorMessage = "لطفا {0} رشته را وارد نمایید")]
        [MaxLength(200, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [DataType(DataType.MultilineText)]
        public string ShortDescription { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} رشته را وارد نمایید")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [Display(Name = "تصویر")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; }

        [Display(Name = "تعداد هنرجویان")]
        [Required(ErrorMessage = "لطفا {0} رشته را وارد نمایید")]
        public int Count { get; set; }

        public virtual ICollection<MajorTeachers> MajorTeachers { get; set; }
        public virtual ICollection<Contents> Contents { get; set; }
    }
}
