using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class ReplyOptional
    {
        [Key]
        [MaxLength(50)]
        public string OptionalID { get; set; }

        [MaxLength(50)]
        public string? ReplyID { get; set; }

        [MaxLength(50)]
        public string ChoiceID { get; set; }
        public bool IsCondition { get; set; }


        [ForeignKey("ReplyID")]
        public virtual QuestionReplys QuestionReply { get; set; }

        [ForeignKey("ChoiceID")]
        public virtual Choices Choices { get; set; }
    }
}
