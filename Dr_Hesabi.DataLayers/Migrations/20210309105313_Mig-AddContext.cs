using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dr_Hesabi.DataLayers.Migrations
{
    public partial class MigAddContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bests",
                columns: table => new
                {
                    BestID = table.Column<string>(maxLength: 50, nullable: false),
                    ParentID = table.Column<string>(maxLength: 50, nullable: true),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    Description = table.Column<string>(maxLength: 800, nullable: true),
                    ImageName = table.Column<string>(maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bests", x => x.BestID);
                    table.ForeignKey(
                        name: "FK_Bests_Bests_ParentID",
                        column: x => x.ParentID,
                        principalTable: "Bests",
                        principalColumn: "BestID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    BlogID = table.Column<string>(maxLength: 50, nullable: false),
                    NameWriter = table.Column<string>(maxLength: 150, nullable: false),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    ShortDescription = table.Column<string>(maxLength: 200, nullable: false),
                    Description = table.Column<string>(nullable: false),
                    ImageName = table.Column<string>(maxLength: 50, nullable: true),
                    Visit = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.BlogID);
                });

            migrationBuilder.CreateTable(
                name: "Gallerys",
                columns: table => new
                {
                    GalleryID = table.Column<string>(maxLength: 50, nullable: false),
                    ParentID = table.Column<string>(maxLength: 50, nullable: true),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    ImageName = table.Column<string>(maxLength: 50, nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gallerys", x => x.GalleryID);
                    table.ForeignKey(
                        name: "FK_Gallerys_Gallerys_ParentID",
                        column: x => x.ParentID,
                        principalTable: "Gallerys",
                        principalColumn: "GalleryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Majors",
                columns: table => new
                {
                    MajorID = table.Column<string>(maxLength: 50, nullable: false),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    ShortDescription = table.Column<string>(maxLength: 200, nullable: false),
                    Text = table.Column<string>(nullable: false),
                    ImageName = table.Column<string>(maxLength: 50, nullable: true),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Majors", x => x.MajorID);
                });

            migrationBuilder.CreateTable(
                name: "Newses",
                columns: table => new
                {
                    NewsID = table.Column<string>(maxLength: 50, nullable: false),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    ShortDescription = table.Column<string>(maxLength: 200, nullable: false),
                    Text = table.Column<string>(nullable: false),
                    ImageName = table.Column<string>(maxLength: 50, nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Visit = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Newses", x => x.NewsID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleID = table.Column<string>(maxLength: 50, nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    Name = table.Column<string>(maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "Setting",
                columns: table => new
                {
                    SettingID = table.Column<string>(maxLength: 50, nullable: false),
                    NameSite = table.Column<string>(maxLength: 150, nullable: true),
                    NameSite2 = table.Column<string>(maxLength: 150, nullable: true),
                    ShortDescription = table.Column<string>(maxLength: 200, nullable: true),
                    Targets = table.Column<string>(nullable: true),
                    History = table.Column<string>(nullable: true),
                    GuideTeacher = table.Column<string>(nullable: true),
                    GuideStudent = table.Column<string>(nullable: true),
                    Telephone = table.Column<string>(maxLength: 100, nullable: true),
                    Address = table.Column<string>(maxLength: 200, nullable: true),
                    Telegram = table.Column<string>(maxLength: 150, nullable: true),
                    ImgCodeQR = table.Column<string>(maxLength: 50, nullable: true),
                    ImgLogo = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 250, nullable: true),
                    PasswordEmail = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.SettingID);
                });

            migrationBuilder.CreateTable(
                name: "Sliders",
                columns: table => new
                {
                    SlideID = table.Column<string>(maxLength: 50, nullable: false),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    Caption = table.Column<string>(maxLength: 400, nullable: true),
                    ImageName = table.Column<string>(maxLength: 50, nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliders", x => x.SlideID);
                });

            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    StaffID = table.Column<string>(maxLength: 50, nullable: false),
                    ParentID = table.Column<string>(maxLength: 50, nullable: true),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    Text = table.Column<string>(nullable: true),
                    ImageName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.StaffID);
                    table.ForeignKey(
                        name: "FK_Staffs_Staffs_ParentID",
                        column: x => x.ParentID,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    SurveyID = table.Column<string>(maxLength: 50, nullable: false),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    Description = table.Column<string>(maxLength: 400, nullable: false),
                    ImageName = table.Column<string>(maxLength: 50, nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    CountStar = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsPermission = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.SurveyID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<string>(maxLength: 50, nullable: false),
                    UserName = table.Column<string>(maxLength: 150, nullable: false),
                    Email = table.Column<string>(maxLength: 250, nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: false),
                    ActiveCode = table.Column<string>(maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "VisitsDocuments",
                columns: table => new
                {
                    VisitID = table.Column<string>(maxLength: 50, nullable: false),
                    TableID = table.Column<string>(maxLength: 50, nullable: true),
                    IP = table.Column<string>(maxLength: 150, nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitsDocuments", x => x.VisitID);
                });

            migrationBuilder.CreateTable(
                name: "Contents",
                columns: table => new
                {
                    ContentID = table.Column<string>(maxLength: 50, nullable: false),
                    ParentID = table.Column<string>(maxLength: 50, nullable: true),
                    MajorID = table.Column<string>(maxLength: 50, nullable: true),
                    Title = table.Column<string>(maxLength: 250, nullable: false),
                    ImageName = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.ContentID);
                    table.ForeignKey(
                        name: "FK_Contents_Majors_MajorID",
                        column: x => x.MajorID,
                        principalTable: "Majors",
                        principalColumn: "MajorID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contents_Contents_ParentID",
                        column: x => x.ParentID,
                        principalTable: "Contents",
                        principalColumn: "ContentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SurveysQuestions",
                columns: table => new
                {
                    QuestionID = table.Column<string>(maxLength: 50, nullable: false),
                    SurveyID = table.Column<string>(maxLength: 50, nullable: true),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    ImageName = table.Column<string>(maxLength: 50, nullable: true),
                    SumVote = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveysQuestions", x => x.QuestionID);
                    table.ForeignKey(
                        name: "FK_SurveysQuestions_Surveys_SurveyID",
                        column: x => x.SurveyID,
                        principalTable: "Surveys",
                        principalColumn: "SurveyID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    AttachmentID = table.Column<string>(maxLength: 50, nullable: false),
                    UserID = table.Column<string>(maxLength: 50, nullable: true),
                    FileName = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.AttachmentID);
                    table.ForeignKey(
                        name: "FK_Attachments_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentID = table.Column<string>(maxLength: 50, nullable: false),
                    PanelID = table.Column<string>(maxLength: 50, nullable: true),
                    UserID = table.Column<string>(maxLength: 50, nullable: true),
                    Method = table.Column<string>(nullable: true),
                    Text = table.Column<string>(maxLength: 150, nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentID);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    ConnectionID = table.Column<string>(maxLength: 50, nullable: false),
                    ParentID = table.Column<string>(maxLength: 50, nullable: true),
                    Comment = table.Column<string>(maxLength: 500, nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    IsReplayAdmin = table.Column<bool>(nullable: false),
                    UserID = table.Column<string>(maxLength: 50, nullable: true),
                    UserID2 = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.ConnectionID);
                    table.ForeignKey(
                        name: "FK_Connections_Connections_ParentID",
                        column: x => x.ParentID,
                        principalTable: "Connections",
                        principalColumn: "ConnectionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Connections_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Connections_Users_UserID2",
                        column: x => x.UserID2,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MajorTeachers",
                columns: table => new
                {
                    MajorTeacherID = table.Column<string>(maxLength: 50, nullable: false),
                    UserID = table.Column<string>(maxLength: 50, nullable: true),
                    MajorID = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MajorTeachers", x => x.MajorTeacherID);
                    table.ForeignKey(
                        name: "FK_MajorTeachers_Majors_MajorID",
                        column: x => x.MajorID,
                        principalTable: "Majors",
                        principalColumn: "MajorID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MajorTeachers_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfileRequests",
                columns: table => new
                {
                    ProfileRequestID = table.Column<string>(maxLength: 50, nullable: false),
                    UserID = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 350, nullable: false),
                    IsCondition = table.Column<bool>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileRequests", x => x.ProfileRequestID);
                    table.ForeignKey(
                        name: "FK_ProfileRequests_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfileStaffs",
                columns: table => new
                {
                    ProfileStaffID = table.Column<string>(maxLength: 50, nullable: false),
                    StaffID = table.Column<string>(maxLength: 50, nullable: true),
                    UserID = table.Column<string>(maxLength: 50, nullable: true),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ImageName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileStaffs", x => x.ProfileStaffID);
                    table.ForeignKey(
                        name: "FK_ProfileStaffs_Staffs_StaffID",
                        column: x => x.StaffID,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileStaffs_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfileStudents",
                columns: table => new
                {
                    ProfileID = table.Column<string>(maxLength: 50, nullable: false),
                    UserID = table.Column<string>(maxLength: 50, nullable: true),
                    FullName = table.Column<string>(maxLength: 150, nullable: false),
                    CodeMeli = table.Column<string>(maxLength: 10, nullable: false),
                    CodeClass = table.Column<int>(nullable: false),
                    IsCondition = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileStudents", x => x.ProfileID);
                    table.ForeignKey(
                        name: "FK_ProfileStudents_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleSelects",
                columns: table => new
                {
                    SelectID = table.Column<string>(maxLength: 50, nullable: false),
                    UserID = table.Column<string>(maxLength: 50, nullable: true),
                    RoleID = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleSelects", x => x.SelectID);
                    table.ForeignKey(
                        name: "FK_RoleSelects_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleSelects_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    TestID = table.Column<string>(maxLength: 50, nullable: false),
                    UserID = table.Column<string>(maxLength: 50, nullable: true),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: false),
                    StartDateTime = table.Column<DateTime>(nullable: false),
                    EndDateTime = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsNegative = table.Column<bool>(nullable: false),
                    IsRandom = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsUltimate = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.TestID);
                    table.ForeignKey(
                        name: "FK_Tests_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SurveysVotes",
                columns: table => new
                {
                    VoteID = table.Column<string>(maxLength: 50, nullable: false),
                    UserID = table.Column<string>(maxLength: 50, nullable: true),
                    QuestionID = table.Column<string>(maxLength: 50, nullable: true),
                    Vote = table.Column<int>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveysVotes", x => x.VoteID);
                    table.ForeignKey(
                        name: "FK_SurveysVotes_SurveysQuestions_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "SurveysQuestions",
                        principalColumn: "QuestionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SurveysVotes_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoginTests",
                columns: table => new
                {
                    LoginID = table.Column<string>(maxLength: 50, nullable: false),
                    UserID = table.Column<string>(maxLength: 50, nullable: true),
                    TestID = table.Column<string>(maxLength: 50, nullable: true),
                    IP = table.Column<string>(maxLength: 150, nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginTests", x => x.LoginID);
                    table.ForeignKey(
                        name: "FK_LoginTests_Tests_TestID",
                        column: x => x.TestID,
                        principalTable: "Tests",
                        principalColumn: "TestID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoginTests_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionID = table.Column<string>(maxLength: 50, nullable: false),
                    TestID = table.Column<string>(maxLength: 50, nullable: true),
                    Method = table.Column<string>(maxLength: 20, nullable: false),
                    Title = table.Column<string>(maxLength: 300, nullable: false),
                    Score = table.Column<double>(nullable: false),
                    ImageName = table.Column<string>(nullable: true),
                    MethodInput = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionID);
                    table.ForeignKey(
                        name: "FK_Questions_Tests_TestID",
                        column: x => x.TestID,
                        principalTable: "Tests",
                        principalColumn: "TestID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestClasses",
                columns: table => new
                {
                    TestClassID = table.Column<string>(maxLength: 50, nullable: false),
                    TestID = table.Column<string>(maxLength: 50, nullable: true),
                    Class = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestClasses", x => x.TestClassID);
                    table.ForeignKey(
                        name: "FK_TestClasses_Tests_TestID",
                        column: x => x.TestID,
                        principalTable: "Tests",
                        principalColumn: "TestID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestRequests",
                columns: table => new
                {
                    TestRequestID = table.Column<string>(maxLength: 50, nullable: false),
                    TestID = table.Column<string>(maxLength: 50, nullable: true),
                    UserID = table.Column<string>(maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestRequests", x => x.TestRequestID);
                    table.ForeignKey(
                        name: "FK_TestRequests_Tests_TestID",
                        column: x => x.TestID,
                        principalTable: "Tests",
                        principalColumn: "TestID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestRequests_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestsUltimate",
                columns: table => new
                {
                    UltimateID = table.Column<string>(maxLength: 50, nullable: false),
                    TestID = table.Column<string>(maxLength: 50, nullable: true),
                    UserID = table.Column<string>(maxLength: 50, nullable: true),
                    Score = table.Column<float>(nullable: false),
                    CountTrue = table.Column<int>(nullable: false),
                    CountFalse = table.Column<int>(nullable: false),
                    ReplyNull = table.Column<int>(nullable: false),
                    CountNull = table.Column<int>(nullable: false),
                    TestScore = table.Column<float>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestsUltimate", x => x.UltimateID);
                    table.ForeignKey(
                        name: "FK_TestsUltimate_Tests_TestID",
                        column: x => x.TestID,
                        principalTable: "Tests",
                        principalColumn: "TestID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestsUltimate_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Choices",
                columns: table => new
                {
                    ChoiceID = table.Column<string>(maxLength: 50, nullable: false),
                    QuestionID = table.Column<string>(maxLength: 50, nullable: true),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    Order = table.Column<int>(nullable: false),
                    IsSuccess = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choices", x => x.ChoiceID);
                    table.ForeignKey(
                        name: "FK_Choices_Questions_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "Questions",
                        principalColumn: "QuestionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionReplys",
                columns: table => new
                {
                    ReplyID = table.Column<string>(maxLength: 50, nullable: false),
                    QuestionID = table.Column<string>(maxLength: 50, nullable: true),
                    UserID = table.Column<string>(maxLength: 50, nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionReplys", x => x.ReplyID);
                    table.ForeignKey(
                        name: "FK_QuestionReplys_Questions_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "Questions",
                        principalColumn: "QuestionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionReplys_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReplyDescriptives",
                columns: table => new
                {
                    DescriptiveID = table.Column<string>(maxLength: 50, nullable: false),
                    ReplyID = table.Column<string>(maxLength: 50, nullable: true),
                    Text = table.Column<string>(maxLength: 400, nullable: true),
                    ImageName = table.Column<string>(maxLength: 60, nullable: true),
                    IsCondition = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplyDescriptives", x => x.DescriptiveID);
                    table.ForeignKey(
                        name: "FK_ReplyDescriptives_QuestionReplys_ReplyID",
                        column: x => x.ReplyID,
                        principalTable: "QuestionReplys",
                        principalColumn: "ReplyID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReplyOptionals",
                columns: table => new
                {
                    OptionalID = table.Column<string>(maxLength: 50, nullable: false),
                    ReplyID = table.Column<string>(maxLength: 50, nullable: true),
                    ChoiceID = table.Column<string>(maxLength: 50, nullable: true),
                    IsCondition = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplyOptionals", x => x.OptionalID);
                    table.ForeignKey(
                        name: "FK_ReplyOptionals_Choices_ChoiceID",
                        column: x => x.ChoiceID,
                        principalTable: "Choices",
                        principalColumn: "ChoiceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReplyOptionals_QuestionReplys_ReplyID",
                        column: x => x.ReplyID,
                        principalTable: "QuestionReplys",
                        principalColumn: "ReplyID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_UserID",
                table: "Attachments",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Bests_ParentID",
                table: "Bests",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_Choices_QuestionID",
                table: "Choices",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserID",
                table: "Comments",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_ParentID",
                table: "Connections",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_UserID",
                table: "Connections",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_UserID2",
                table: "Connections",
                column: "UserID2");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_MajorID",
                table: "Contents",
                column: "MajorID");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_ParentID",
                table: "Contents",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_Gallerys_ParentID",
                table: "Gallerys",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_LoginTests_TestID",
                table: "LoginTests",
                column: "TestID");

            migrationBuilder.CreateIndex(
                name: "IX_LoginTests_UserID",
                table: "LoginTests",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_MajorTeachers_MajorID",
                table: "MajorTeachers",
                column: "MajorID");

            migrationBuilder.CreateIndex(
                name: "IX_MajorTeachers_UserID",
                table: "MajorTeachers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileRequests_UserID",
                table: "ProfileRequests",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileStaffs_StaffID",
                table: "ProfileStaffs",
                column: "StaffID");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileStaffs_UserID",
                table: "ProfileStaffs",
                column: "UserID",
                unique: true,
                filter: "[UserID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileStudents_UserID",
                table: "ProfileStudents",
                column: "UserID",
                unique: true,
                filter: "[UserID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionReplys_QuestionID",
                table: "QuestionReplys",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionReplys_UserID",
                table: "QuestionReplys",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TestID",
                table: "Questions",
                column: "TestID");

            migrationBuilder.CreateIndex(
                name: "IX_ReplyDescriptives_ReplyID",
                table: "ReplyDescriptives",
                column: "ReplyID");

            migrationBuilder.CreateIndex(
                name: "IX_ReplyOptionals_ChoiceID",
                table: "ReplyOptionals",
                column: "ChoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_ReplyOptionals_ReplyID",
                table: "ReplyOptionals",
                column: "ReplyID");

            migrationBuilder.CreateIndex(
                name: "IX_RoleSelects_RoleID",
                table: "RoleSelects",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_RoleSelects_UserID",
                table: "RoleSelects",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_ParentID",
                table: "Staffs",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_SurveysQuestions_SurveyID",
                table: "SurveysQuestions",
                column: "SurveyID");

            migrationBuilder.CreateIndex(
                name: "IX_SurveysVotes_QuestionID",
                table: "SurveysVotes",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_SurveysVotes_UserID",
                table: "SurveysVotes",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_TestClasses_TestID",
                table: "TestClasses",
                column: "TestID");

            migrationBuilder.CreateIndex(
                name: "IX_TestRequests_TestID",
                table: "TestRequests",
                column: "TestID");

            migrationBuilder.CreateIndex(
                name: "IX_TestRequests_UserID",
                table: "TestRequests",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_UserID",
                table: "Tests",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_TestsUltimate_TestID",
                table: "TestsUltimate",
                column: "TestID");

            migrationBuilder.CreateIndex(
                name: "IX_TestsUltimate_UserID",
                table: "TestsUltimate",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "Bests");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.DropTable(
                name: "Contents");

            migrationBuilder.DropTable(
                name: "Gallerys");

            migrationBuilder.DropTable(
                name: "LoginTests");

            migrationBuilder.DropTable(
                name: "MajorTeachers");

            migrationBuilder.DropTable(
                name: "Newses");

            migrationBuilder.DropTable(
                name: "ProfileRequests");

            migrationBuilder.DropTable(
                name: "ProfileStaffs");

            migrationBuilder.DropTable(
                name: "ProfileStudents");

            migrationBuilder.DropTable(
                name: "ReplyDescriptives");

            migrationBuilder.DropTable(
                name: "ReplyOptionals");

            migrationBuilder.DropTable(
                name: "RoleSelects");

            migrationBuilder.DropTable(
                name: "Setting");

            migrationBuilder.DropTable(
                name: "Sliders");

            migrationBuilder.DropTable(
                name: "SurveysVotes");

            migrationBuilder.DropTable(
                name: "TestClasses");

            migrationBuilder.DropTable(
                name: "TestRequests");

            migrationBuilder.DropTable(
                name: "TestsUltimate");

            migrationBuilder.DropTable(
                name: "VisitsDocuments");

            migrationBuilder.DropTable(
                name: "Majors");

            migrationBuilder.DropTable(
                name: "Staffs");

            migrationBuilder.DropTable(
                name: "Choices");

            migrationBuilder.DropTable(
                name: "QuestionReplys");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "SurveysQuestions");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Surveys");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
