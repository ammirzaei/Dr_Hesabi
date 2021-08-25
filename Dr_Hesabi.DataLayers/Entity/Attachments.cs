using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class Attachments
    {
        [Key]
        [MaxLength(50)]
        public string AttachmentID { get; set; }

        [MaxLength(50)]
        public string? UserID { get; set; }

        [MaxLength(20)]
        public string PanelName { get; set; }

        [Display(Name = "فایل")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string FileName { get; set; }

        [ForeignKey("UserID")]
        public Users Users { get; set; }
    }
}
