using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class LoginTests
    {
        [Key]
        [MaxLength(50)]
        public string LoginID { get; set; }

        [MaxLength(50)]
        public string? UserID { get; set; }

        [MaxLength(50)]
        public string TestID { get; set; }

        [Display(Name = "آی پی")]
        [Required(ErrorMessage = "لطفا {0} خبر را وارد نمایید")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string IP { get; set; }

        [Display(Name = "تاریخ ورود")]
        public DateTime DateTime { get; set; }

        [ForeignKey("UserID")]
        public virtual Users Users { get; set; }

        [ForeignKey("TestID")]
        public virtual Tests Tests { get; set; }
    }
}
