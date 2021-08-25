using System;
using System.Collections.Generic;
using System.Text;

namespace Dr_Hesabi.Classes.ViewModel
{
    public class GetAllStaffsViewModel
    {
        public string StaffID { get; set; }
        public string? ParentID { get; set; }
        public string? Title { get; set; }
        public string? ImageName { get; set; }
    }

    public class GetStaffViewModel
    {
        public string StaffID { get; set; }
        public string ParentTitle { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public string Description { get; set; }
    }

    public class GetAllMajorsViewModel
    {
        public string Title { get; set; }
        public string ImageName { get; set; }
    }
}
