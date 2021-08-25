using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class TestRequests
    {
        [Key]
        [MaxLength(50)]
        public string TestRequestID { get; set; }

        [MaxLength(50)]
        public string TestID { get; set; }

        [MaxLength(50)]
        public string? UserID { get; set; }

        [Display(Name = "وضعیت")]
        public bool? IsActive { get; set; }

        [ForeignKey("TestID")]
        public virtual Tests Tests { get; set; }

        [ForeignKey("UserID")]
        public virtual Users Users { get; set; }
    }
}
