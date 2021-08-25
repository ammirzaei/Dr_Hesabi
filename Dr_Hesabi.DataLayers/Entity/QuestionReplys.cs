using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class QuestionReplys
    {
        [Key]
        [MaxLength(50)]
        public string ReplyID { get; set; }

        [MaxLength(50)]
        public string QuestionID { get; set; }

        [MaxLength(50)]
        public string? UserID { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime DateTime { get; set; }


        [ForeignKey("UserID")]
        public virtual Users Users { get; set; }

        [ForeignKey("QuestionID")]
        public virtual Questions Questions { get; set; }

        public virtual ICollection<ReplyOptional> ReplyOptionals { get; set; }
        public virtual ICollection<ReplyDescriptives> ReplyDescriptives { get; set; }
    }
}
