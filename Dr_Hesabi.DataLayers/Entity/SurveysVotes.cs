using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class SurveysVotes
    {
        [Key]
        [MaxLength(50)]
        public string VoteID { get; set; }

        [MaxLength(50)]
        public string UserID { get; set; }

        [MaxLength(50)]
        public string QuestionID { get; set; }

        [Display(Name = "رأی")]
        [Required(ErrorMessage = "لطفا {0} خود را وارد نمایید")]
        public int Vote { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime DateTime { get; set; }

        [ForeignKey("UserID")]
        public virtual Users Users { get; set; }

        [ForeignKey("QuestionID")]
        public virtual SurveysQuestions SurveysQuestions { get; set; }


    }
}
