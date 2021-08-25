using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class ProfileStudents
    {
        [Key]
        [MaxLength(50)]
        public string ProfileID { get; set; }

        [MaxLength(50)]
        public string UserID { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} خود را وارد نمایید")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string FullName { get; set; }

        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا {0} خود را وارد نمایید")]
        [MaxLength(10, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [MinLength(10, ErrorMessage = "مقدار {0} نباید کمتر از {1} کاراکتر باشد")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "لطفا {0} را صحیح وارد نمایید")]
        [Phone(ErrorMessage = "لطفا {0} را صحیح وارد نمایید")]
        public string CodeMeli { get; set; }

        [Display(Name = "شماره کلاس")]
        [Required(ErrorMessage = "لطفا {0} خود را انتخاب نمایید")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "لطفا {0} را صحیح وارد نمایید")]
        public int CodeClass { get; set; }

        [Display(Name = "وضعیت")]
        public bool? IsCondition { get; set; }

        [ForeignKey("UserID")]
        public virtual Users Users { get; set; }
    }
}
