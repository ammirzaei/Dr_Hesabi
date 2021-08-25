using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class RoleSelects
    {
        [Key]
        [MaxLength(50)]
        public string SelectID { get; set; }

        [MaxLength(50)]
        public string UserID { get; set; }

        [MaxLength(50)]
        public string RoleID { get; set; }

        [ForeignKey("UserID")]
        public virtual Users Users { get; set; }

        [ForeignKey("RoleID")]
        public virtual Roles Roles { get; set; }

    }
}
