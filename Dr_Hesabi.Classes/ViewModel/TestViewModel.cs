using System;
using System.Collections.Generic;
using System.Text;

namespace Dr_Hesabi.Classes.ViewModel
{
    public class PrintReportTestViewModel
    {
        public int Code { get; set; }
        public string FullName { get; set; }
        public string CodeClass { get; set; }
        public float Score { get; set; }
        public int CountTrue { get; set; }
        public int CountFalse { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class PrintReportTestRequestViewModel
    {
        public int Code { get; set; }
        public string FullName { get; set; }
        public string CodeClass { get; set; }
    }

    public class GetAllParticipantTest
    {
        public string UserID { get; set; }
        public string FullName { get; set; }
        public string CodeClass { get; set; }
        public float Score { get; set; }
    }

    public class ReportTestUltimateViewModel
    {
        public ReportTestUltimateViewModel()
        {
            this.Score = 0;
            this.CountFalse = 0;
            this.CountNull = 0;
            this.CountTrue = 0;
            this.ReplyNull = 0;
            this.TestScore = 0;
        }
        public int CountFalse { get; set; }
        public int CountTrue { get; set; }
        public int CountNull { get; set; }
        public double Score { get; set; }
        public int ReplyNull { get; set; }
        public float TestScore { get; set; }
    }
}
