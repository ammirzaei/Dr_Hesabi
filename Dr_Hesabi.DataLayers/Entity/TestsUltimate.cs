using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class TestsUltimate
    {
        [Key]
        [MaxLength(50)]
        public string UltimateID { get; set; }

        [MaxLength(50)]
        public string? TestID { get; set; }

        [MaxLength(50)]
        public string? UserID { get; set; }

        [Display(Name = "نمره نهایی")]
        [Required(ErrorMessage = "لطفا {0} آزمون را وارد نمایید")]
        public float Score { get; set; }

        [Display(Name = "تعداد صحیح")]
        [Required(ErrorMessage = "لطفا {0} آزمون را وارد نمایید")]
        public int CountTrue { get; set; }

        [Display(Name = "تعداد غلط")]
        [Required(ErrorMessage = "لطفا {0} آزمون را وارد نمایید")]
        public int CountFalse { get; set; }

        [Display(Name = "تعداد بی پاسخ")]
        [Required(ErrorMessage = "لطفا {0} آزمون را وارد نمایید")]
        public int ReplyNull { get; set; }

        [Display(Name = "تعداد در انتظار تصحیح")]
        [Required(ErrorMessage = "لطفا {0} آزمون را وارد نمایید")]
        public int CountNull { get; set; }

        [Display(Name = "نمره آزمون")]
        [Required(ErrorMessage = "لطفا {0} آزمون را وارد نمایید")]
        public float TestScore { get; set; }

        [Display(Name = "تاریخ پایان")]
        [Required(ErrorMessage = "لطفا {0} آزمون را وارد نمایید")]
        public DateTime DateTime { get; set; }
        [ForeignKey("TestID")]
        public virtual Tests Tests { get; set; }

        [ForeignKey("UserID")]
        public virtual Users Users { get; set; }
    }
}
