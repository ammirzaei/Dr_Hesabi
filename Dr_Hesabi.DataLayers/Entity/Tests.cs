using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class Tests
    {
        [Key]
        [MaxLength(50)]
        public string TestID { get; set; }

        [MaxLength(50)]
        public string UserID { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} آزمون را وارد نمایید")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} آزمون را وارد نمایید")]
        [MaxLength(500, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "تاریخ شروع")]
        [Required(ErrorMessage = "لطفا {0} آزمون را وارد نمایید")]
        public DateTime StartDateTime { get; set; }

        [Display(Name = "تاریخ پایان")]
        [Required(ErrorMessage = "لطفا {0} آزمون را وارد نمایید")]
        public DateTime EndDateTime { get; set; }

        [Display(Name = "وضعیت")]
        [Required]
        public bool IsActive { get; set; }

        [Display(Name = "نمره منفی")]
        [Required]
        public bool IsNegative { get; set; }

        [Display(Name = "رندوم")]
        [Required]
        public bool IsRandom { get; set; }

        [Display(Name = "حذف")]
        public bool IsDeleted { get; set; }

        [Display(Name = "گزارش پایانی")]
        public bool IsUltimate { get; set; }

        [ForeignKey("UserID")]
        public virtual Users Users { get; set; }

        public virtual ICollection<Questions> Questions { get; set; }
        public virtual ICollection<LoginTests> LoginTests { get; set; }
        public virtual ICollection<TestsUltimate> TestsUltimate { get; set; }
        public virtual ICollection<TestClasses> TestClasses { get; set; }
        public virtual ICollection<TestRequests> TestRequests { get; set; }
    }
}
