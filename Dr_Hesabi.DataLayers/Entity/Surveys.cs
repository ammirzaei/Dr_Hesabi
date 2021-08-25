using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class Surveys
    {
        [Key]
        [MaxLength(50)]
        public string SurveyID { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} نظرسنجی را وارد نمایید")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} نظرسنجی را وارد نمایید")]
        [MaxLength(400, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "تصویر")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; }

        [Display(Name = "تاریخ شروع")]
        [Required(ErrorMessage = "لطفا {0} نظرسنجی را وارد نمایید")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "تاریخ پایان")]
        [Required(ErrorMessage = "لطفا {0} نظرسنجی را وارد نمایید")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "تعداد ستاره")]
        [Required(ErrorMessage = "لطفا {0} نظرسنجی را وارد نمایید")]
        public int CountStar { get; set; }

        [Display(Name = "وضعیت")]
        [Required]
        public bool IsActive { get; set; }

        [Display(Name = "اجازه")]
        [Required]
        public bool IsPermission { get; set; }

        public virtual ICollection<SurveysQuestions> SurveysQuestions { get; set; }

    }
}
