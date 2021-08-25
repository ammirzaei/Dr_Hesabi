using System;
using System.Collections.Generic;
using System.Text;

namespace Dr_Hesabi.Classes.ViewModel
{
    public class ManageSiteTeacherViewModel
    {
        public int CountTests { get; set; }
        public int CountGuides { get; set; }
        public int CountAttachments { get; set; }
        public bool IsMajor { get; set; }
    }

    public class ListsContentsViewModel
    {
        public string ContentTitle { get; set; }
        public string ContentID { get; set; }
        public string? ContentParentID { get; set; }
        public string MajorID { get; set; }
    }
}
