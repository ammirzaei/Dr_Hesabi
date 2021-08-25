using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class Questions
    {
        [Key]
        [MaxLength(50)]
        public string QuestionID { get; set; }

        [MaxLength(50)]
        public string TestID { get; set; }

        [Display(Name = "نوع سوال")]
        [Required(ErrorMessage = "لطفا {0} را انتخاب نمایید")]
        [MaxLength(20, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Method { get; set; }

        [Display(Name = "عنوان سوال")]
        [Required(ErrorMessage = "لطفا {0} سوال را وارد نمایید")]
        [MaxLength(300, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [DataType(DataType.MultilineText)]
        public string Title { get; set; }

        [Display(Name = "نمره")]
        [Required(ErrorMessage = "لطفا {0} سوال را وارد نمایید")]
        public double Score { get; set; }

        [Display(Name = "تصویر")]
        public string ImageName { get; set; }

        [Display(Name = "نوع دریافت پاسخ")]
        [MaxLength(30, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string MethodInput { get; set; }

        [ForeignKey("TestID")]
        public virtual Tests Tests { get; set; }

        public virtual ICollection<Choices> Choices { get; set; }
        public virtual ICollection<QuestionReplys> QuestionReplys { get; set; }
    }
}
