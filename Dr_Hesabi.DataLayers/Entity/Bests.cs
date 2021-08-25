using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class Bests
    {
        [Key]
        [MaxLength(50)]
        public string BestID { get; set; }

        [MaxLength(50)]
        public string? ParentID { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} برترین را وارد نمایید")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(800, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "تصویر")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; }

        [Display(Name = "وضعیت")]
        [Required]
        public bool IsActive { get; set; }

        [ForeignKey("ParentID")]
        public virtual ICollection<Bests> Bests1{ get; set; }
        public virtual Bests Bests2 { get; set; }

    }
}
