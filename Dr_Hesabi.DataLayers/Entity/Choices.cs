using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class Choices
    {
        [Key]
        [MaxLength(50)]
        public string ChoiceID { get; set; }

        [MaxLength(50)]
        public string QuestionID { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} گزینه را وارد نمایید")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "ترتیب نمایش")]
        [Required(ErrorMessage = "لطفا {0} گزینه را وارد نمایید")]
        public int Order { get; set; }

        [Display(Name = "گزینه")]
        [Required]
        public bool IsSuccess { get; set; }

        [ForeignKey("QuestionID")]
        public virtual Questions Questions { get; set; }

        public virtual ICollection<ReplyOptional> ReplyOptionals { get; set; }
    }
}
