using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class Staffs
    {
        [Key]
        [MaxLength(50)]
        public string StaffID { get; set; }

        [MaxLength(50)]
        public string? ParentID { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]

        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [Display(Name = "تصویر")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; }

        [ForeignKey("ParentID")]
        public virtual Staffs Staffs2{ get; set; }
        public virtual ICollection<Staffs> Staffs1 { get; set; }
        public virtual ICollection<ProfileStaffs> ProfileStaffs { get; set; }
    }
}
