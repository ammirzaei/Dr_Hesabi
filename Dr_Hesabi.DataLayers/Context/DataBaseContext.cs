using System;
using System.Collections.Generic;
using System.Text;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.DataLayers.Context
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior =
                QueryTrackingBehavior.NoTracking;
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<RoleSelects> RoleSelects { get; set; }
        public DbSet<Sliders> Sliders { get; set; }
        public DbSet<News> Newses { get; set; }
        public DbSet<VisitsDocument> VisitsDocuments { get; set; }
        public DbSet<Staffs> Staffs { get; set; }
        public DbSet<Majors> Majors { get; set; }
        public DbSet<Blogs> Blogs { get; set; }
        public DbSet<Setting> Setting { get; set; }
        public DbSet<Bests> Bests { get; set; }
        public DbSet<Gallerys> Gallerys { get; set; }
        public DbSet<Connections> Connections { get; set; }
        public DbSet<Surveys> Surveys { get; set; }
        public DbSet<SurveysVotes> SurveysVotes { get; set; }
        public DbSet<SurveysQuestions> SurveysQuestions { get; set; }
        public DbSet<Tests> Tests { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<Choices> Choices { get; set; }
        public DbSet<ProfileStudents> ProfileStudents { get; set; }
        public DbSet<LoginTests> LoginTests { get; set; }
        public DbSet<QuestionReplys> QuestionReplys { get; set; }
        public DbSet<ReplyOptional> ReplyOptionals { get; set; }
        public DbSet<ReplyDescriptives> ReplyDescriptives { get; set; }
        public DbSet<TestsUltimate> TestsUltimate { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Attachments> Attachments { get; set; }
        public DbSet<TestClasses> TestClasses { get; set; }
        public DbSet<TestRequests> TestRequests { get; set; }
        public DbSet<ProfileRequests> ProfileRequests { get; set; }
        public DbSet<MajorTeachers> MajorTeachers { get; set; }
        public DbSet<ProfileStaffs> ProfileStaffs { get; set; }
        public DbSet<Contents> Contents { get; set; }
    }
}
