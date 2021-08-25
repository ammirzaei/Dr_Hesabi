using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
   public class Roles
    {
        [Key]
        [MaxLength(50)]
        public string RoleID { get; set; }

        [Display(Name ="عنوان")]
        [Required(ErrorMessage ="")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "")]
        [MaxLength(60, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Name { get; set; }

        public virtual ICollection<RoleSelects> RoleSelects { get; set; }
    }
}
