using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class ProfileRequests
    {
        [Key]
        [MaxLength(50)]
        public string ProfileRequestID { get; set; }

        [MaxLength(50)]
        public string? UserID { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} خود را وارد نمایید")]
        [MaxLength(350, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "وضعیت درخواست")]
        public bool? IsCondition { get; set; }

        [Display(Name = "تاریخ درخواست")]
        public DateTime CreateDate { get; set; }

        [ForeignKey("UserID")]
        public virtual Users Users { get; set; }
    }
}
