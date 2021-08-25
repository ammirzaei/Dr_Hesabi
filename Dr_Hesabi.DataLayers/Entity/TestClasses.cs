using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class TestClasses
    {
        [Key]
        [MaxLength(50)]
        public string TestClassID { get; set; }

        [MaxLength(50)]
        public string TestID { get; set; }

        [Required]
        public int Class { get; set; }

        [ForeignKey("TestID")]
        public virtual Tests Tests { get; set; }
    }
}
