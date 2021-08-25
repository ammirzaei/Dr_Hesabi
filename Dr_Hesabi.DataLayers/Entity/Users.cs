using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dr_Hesabi.DataLayers.Entity
{
    public class Users
    {
        [Key]
        [MaxLength(50)]
        public string UserID { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا نام کاربری خود را وارد نمایید")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [MinLength(8, ErrorMessage = "مقدار {0} نباید کمتر از {1} کاراکتر باشد")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا ایمیل خود را وارد نمایید")]
        [MaxLength(250, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "لطفا ایمیل معتبر وارد نمایید")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا رمز عبور خود را وارد نمایید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [MinLength(6, ErrorMessage = "مقدار {0} نباید کمتر از {1} کاراکتر باشد")]
        //[RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{7,100}$", ErrorMessage = "کلمه عبور باید شامل حرف و عدد باشد")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "کد فعالسازی")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string ActiveCode { get; set; }

        [Display(Name = "وضعیت")]
        [Required]
        public bool IsActive { get; set; }

        [Display(Name = "تاریخ عضویت")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public DateTime Date { get; set; }

        public virtual ICollection<RoleSelects> RoleSelects { get; set; }
        public virtual ICollection<Connections> Connections { get; set; }
        public virtual ICollection<SurveysVotes> SurveysVotes { get; set; }
        public virtual ICollection<Tests> Tests { get; set; }
        public virtual ICollection<Attachments> Attachments { get; set; }
        public virtual ProfileStudents ProfileStudents { get; set; }
        public virtual ICollection<LoginTests> LoginTests { get; set; }
        public virtual ICollection<QuestionReplys> QuestionReplys { get; set; }
        public virtual ICollection<TestsUltimate> TestsUltimate { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<TestRequests> TestRequests { get; set; }
        public virtual ICollection<ProfileRequests> ProfileRequests { get; set; }
        public virtual ProfileStaffs ProfileStaffs { get; set; }
        public virtual ICollection<MajorTeachers> MajorTeachers { get; set; }
    }
}
