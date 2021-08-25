using System;
using System.Collections.Generic;
using System.Text;

namespace Dr_Hesabi.Classes.ViewModel
{
   public class AdminViewModel
    {
        public class ManageSiteViewModel
        {
            public int CountUser { get; set; }
            public int CountSlider { get; set; }
            public int CountNews { get; set; }
            public int CountStaffs { get; set; }
            public int CountMajors { get; set; }
            public int CountBlogs { get; set; }
            public int CountBests { get; set; }
            public int CountGallerys { get; set; }
            public int CountSurveys { get; set; }
            public int CountAttachments { get; set; }
        }

        public class MajorsTeachersViewModel
        {
            public string MajorID { get; set; }
            public string Title { get; set; }
        }

        public class SetStaffToParentViewModel
        {
            public string StaffID { get; set; }
            public string Title { get; set; }
        }

        public class ListStaffsViewModel
        {
            public string StaffID { get; set; }
            public string Title { get; set; }
            public string? ParentID { get; set; }
            public bool IsNative { get; set; }
        }

        public class SetUserToStaffViewModel
        {
            public string UserID { get; set; }
            public string UserName { get; set; }
        }
    }
}
