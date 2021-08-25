using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class SurveysQuestions
    {
        [MaxLength(50)]
        [Key]
        public string QuestionID { get; set; }

        [MaxLength(50)]
        public string SurveyID { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} سوال نظرسنجی را وارد نمایید")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "تصویر")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; }

        [Display(Name = "میانگین آراء")]
        public float SumVote { get; set; }

        [ForeignKey("SurveyID")]
        public virtual Surveys Surveys { get; set; }


        public virtual ICollection<SurveysVotes> SurveysVotes { get; set; }
    }
}
