using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class Gallerys
    {
        [Key]
        [MaxLength(50)]
        public string GalleryID { get; set; }

        [MaxLength(50)]
        public string? ParentID { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "تصویر")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime DateTime { get; set; }

        [ForeignKey("ParentID")]
        public virtual ICollection<Gallerys> Gallery1 { get; set; }
        public virtual Gallerys Gallery2 { get; set; }

        public Gallerys()
        {

        }
    }
}
