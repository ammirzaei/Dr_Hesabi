using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class ReplyDescriptives
    {
        [Key]
        [MaxLength(50)]
        public string DescriptiveID { get; set; }

        [MaxLength(50)]
        public string? ReplyID { get; set; }

        [Display(Name = "پاسخ")]
        [MaxLength(400, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Text { get; set; }

        [Display(Name = "تصویر")]
        [MaxLength(60, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; }

        [Display(Name = "وضعیت پاسخ")]
        public bool? IsCondition { get; set; }

        [ForeignKey("ReplyID")]
        public virtual QuestionReplys QuestionReplys { get; set; }

    }
}
