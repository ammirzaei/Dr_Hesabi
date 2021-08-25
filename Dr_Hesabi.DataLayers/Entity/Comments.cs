using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class Comments
    {
        [Key]
        [MaxLength(50)]
        public string CommentID { get; set; }

        [MaxLength(50)]
        public string PanelID { get; set; }

        [MaxLength(50)]
        public string UserID { get; set; }

        public string Method { get; set; }

        [Display(Name = "نظر")]
        [Required(ErrorMessage = "لطفا {0} خود را وارد نمایید")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime DateTime { get; set; }

        [ForeignKey("UserID")]
        public virtual Users Users { get; set; }
    }
}
