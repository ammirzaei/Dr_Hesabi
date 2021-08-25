using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
   public class VisitsDocument
    {
        [Key]
        [MaxLength(50)]
        public string VisitID { get; set; }

        [MaxLength(50)]
        public string TableID { get; set; }

        [MaxLength(150)]
        public string IP { get; set; }
        public DateTime DateTime { get; set; }
    }
}
