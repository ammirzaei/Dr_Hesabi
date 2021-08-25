using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class MajorTeachers
    {
        [Key]
        [MaxLength(50)]
        public string MajorTeacherID { get; set; }

        [MaxLength(50)]
        public string? UserID { get; set; }

        [MaxLength(50)]
        public string MajorID { get; set; }

        [ForeignKey("UserID")]
        public virtual Users Users { get; set; }

        [ForeignKey("MajorID")]
        public virtual Majors Majors { get; set; }
    }
}
