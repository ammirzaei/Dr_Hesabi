using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class Contents
    {
        [Key]
        [MaxLength(50)]
        public string ContentID { get; set; }

        [MaxLength(50)]
        public string? ParentID { get; set; }

        [MaxLength(50)]
        public string MajorID { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(250, ErrorMessage = "مقدار {0} نباید بیشتر {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "تصویر")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نباید بیشتر {1} کاراکتر باشد")]
        public string ImageName { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }


        [ForeignKey("ParentID")]
        public virtual Contents Contents1 { get; set; }

        public virtual ICollection<Contents> Contents2 { get; set; }

        [ForeignKey("MajorID")]
        public virtual Majors Majors { get; set; }
    }
}
