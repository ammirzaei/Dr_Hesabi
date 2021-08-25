using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Class;
using Dr_Hesabi.Classes.Interface;
using Dr_Hesabi.Classes.ViewModel;
using Dr_Hesabi.DataLayers.Context;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Classes.Service
{
    public class TestsService : ITests,IDisposable
    {
        private readonly DataBaseContext db;

        public TestsService(DataBaseContext db)
        {
            this.db = db;
        }

        public async Task<Tests> GetTest2(string testID, string title)
        {
            return await db.Tests.FirstOrDefaultAsync(s =>
                s.TestID == testID && s.Title == title && s.IsDeleted == false && s.IsActive);
        }

        public async Task<Tests> GetTest(string testID)
        {
            return await db.Tests.FindAsync(testID);
        }


        public async Task<bool> ExistTest(string testID)
        {
            return await db.Tests.AnyAsync(s =>
                s.IsActive && s.StartDateTime <= DateTime.Now && s.EndDateTime >= DateTime.Now && s.TestID == testID);
        }

        public async Task<bool> ExistUser(string testID, string userID, string IP)
        {
            bool val = false;
            val = await db.LoginTests.AnyAsync(s => s.UserID == userID && s.IP == IP && s.TestID == testID);
            if (val != await db.LoginTests.AnyAsync(s => s.IP == IP && s.TestID == testID))
            {
                val = await db.LoginTests.AnyAsync(s => s.IP == IP && s.TestID == testID);
            }
            return val;
        }

        public async Task AddLoginTest(LoginTests loginTest)
        {
            db.Add(loginTest);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Questions>> GetAllQuestionsTest(string testID, string title)
        {
            var Test = await GetTest(testID);
            if (Test.IsRandom)
            {
                var ListQuestionID = await db.Questions.Where(s => s.TestID == testID && s.Tests.Title == title && s.Tests.IsActive && s.Tests.StartDateTime <= DateTime.Now && s.Tests.EndDateTime >= DateTime.Now).Select(s => s.QuestionID).ToListAsync();
                List<Questions> List = new List<Questions>();
                int count = ListQuestionID.Count();
                for (int i = count; i > 0;)
                {
                    Random Rn = new Random();
                    var Index = Rn.Next(0, count);
                    var question = await db.Questions.FindAsync(ListQuestionID[Index]);
                    if (!List.Any(s => s.QuestionID == question.QuestionID))
                    {
                        List.Add(question);
                        i--;
                    }
                }
                return await Task.FromResult(List.ToList());
            }
            return await db.Questions.Where(s => s.TestID == testID && s.Tests.Title == title && s.Tests.IsActive && s.Tests.StartDateTime <= DateTime.Now && s.Tests.EndDateTime >= DateTime.Now).ToListAsync();
        }

        public async Task<int> ExistAllTest(string? UserID)
        {
            if (await db.Tests.AnyAsync(s => s.IsActive && s.IsDeleted == false))
            {
                if (UserID != null && UserID != "")
                {
                    if (await db.Tests.AnyAsync(s =>
                        s.IsActive && s.StartDateTime <= DateTime.Now && s.EndDateTime >= DateTime.Now &&
                        !s.TestsUltimate.Any(a => a.UserID == UserID)))
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                if (await db.Tests.AnyAsync(s =>
                    s.IsActive && s.StartDateTime <= DateTime.Now && s.EndDateTime >= DateTime.Now))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            return 2;
        }

        public async Task<IEnumerable<Choices>> GetAllChoicesQuestion(string testID)
        {
            var Test = await GetTest(testID);
            if (Test.IsRandom)
            {
                List<Choices> List = new List<Choices>();
                var Choices = await db.Choices.Where(s => s.Questions.TestID == testID).Select(s => s.ChoiceID).ToListAsync();
                var count = Choices.Count();
                for (int i = count; i > 0;)
                {
                    Random Rn = new Random();
                    var Index = Rn.Next(0, count);
                    var choice = await db.Choices.FindAsync(Choices[Index]);
                    if (!List.Any(s => s.ChoiceID == choice.ChoiceID))
                    {
                        List.Add(choice);
                        i--;
                    }
                }
                return List.ToList();
            }
            return await db.Choices.Where(s => s.Questions.TestID == testID).ToListAsync();
        }

        public async Task<Questions> GetQuestion(string questionID)
        {
            return await db.Questions.Include(s => s.Tests).FirstOrDefaultAsync(s => s.QuestionID == questionID);
        }

        public async Task<Questions> GetQuestionforChoice(string choiceId)
        {
            var choice = await db.Choices.Include(s => s.Questions).FirstOrDefaultAsync(s => s.ChoiceID == choiceId);
            return await Task.FromResult(choice.Questions);
        }

        public async Task<bool> ExistReply(string UserID, string QuestionID)
        {
            return await db.QuestionReplys.AnyAsync(s => s.UserID == UserID && s.QuestionID == QuestionID);
        }

        public async Task AddReply(QuestionReplys questionReply)
        {
            db.Add(questionReply);
            await db.SaveChangesAsync();
        }

        public async Task UpdateReply(QuestionReplys questionReply)
        {
            db.Update(questionReply);
            await db.SaveChangesAsync();
        }

        public async Task<QuestionReplys> GetReply(string UserID, string QustionID)
        {
            return await db.QuestionReplys.Include(s => s.ReplyOptionals).Include(s => s.ReplyDescriptives).FirstOrDefaultAsync(s => s.UserID == UserID && s.QuestionID == QustionID);
        }

        public async Task AddReplyOptional(ReplyOptional replyOptional)
        {
            db.Add(replyOptional);
            await db.SaveChangesAsync();
        }

        public async Task UpdateReplyOptional(ReplyOptional replyOptional)
        {
            db.Update(replyOptional);
            await db.SaveChangesAsync();
        }

        public async Task<bool> GetChoiceIsSuccess(string QuestionID, string ChoiceID)
        {
            var choices = await db.Choices.FirstOrDefaultAsync(s => s.IsSuccess && s.QuestionID == QuestionID);
            if (choices.ChoiceID == ChoiceID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<ReplyOptional> GetReplyOptional(string ReplyID)
        {
            return await db.ReplyOptionals.FirstOrDefaultAsync(s => s.ReplyID == ReplyID);
        }

        public async Task<IEnumerable<ReplyOptional>> GetAllReplayOptional(string UserID, string TestID)
        {
            return await db.ReplyOptionals.Include(s => s.QuestionReply).Where(s => s.QuestionReply.UserID == UserID && s.QuestionReply.Questions.TestID == TestID).ToListAsync();
        }

        public async Task AddReplyDescriptive(ReplyDescriptives replyDescriptive)
        {
            db.Add(replyDescriptive);
            await db.SaveChangesAsync();
        }

        public async Task UpdateReplyDescriptive(ReplyDescriptives replyDescriptive)
        {
            db.Update(replyDescriptive);
            await db.SaveChangesAsync();
        }

        public async Task<ReplyDescriptives> GetReplyDescriptive(string ReplyID)
        {
            return await db.ReplyDescriptives.FirstOrDefaultAsync(s => s.ReplyID == ReplyID);
        }

        public async Task<IEnumerable<ReplyDescriptives>> GetAllReplyDescriptives(string UserID, string TestID)
        {
            return await db.ReplyDescriptives
                .Include(s => s.QuestionReplys).Where(s => s.QuestionReplys.UserID == UserID && s.QuestionReplys.Questions.TestID == TestID)
                .ToListAsync();
        }

        public async Task AddUltimate(TestsUltimate testsUltimate)
        {
            db.Add(testsUltimate);
            await db.SaveChangesAsync();
        }

        public async Task<TestsUltimate> GetUltimateTask(string UserID, string TestID)
        {
            return await db.TestsUltimate.FirstOrDefaultAsync(s => s.UserID == UserID && s.TestID == TestID);
        }

        public async Task<bool> ExistUltimate(string UserID, string TestID)
        {
            return await db.TestsUltimate.AnyAsync(s => s.UserID == UserID && s.TestID == TestID);
        }

        public async Task<IEnumerable<Questions>> GetAllQuestion(string TestID)
        {
            return await db.Questions.Where(s => s.TestID == TestID).ToListAsync();
        }

        public async Task UpdateReportTest(string TestID, string UserID, ReportTestUltimateViewModel report)
        {
            var Ultimate = await GetUltimateTask(UserID, TestID);
            if (Ultimate != null)
            {
                Ultimate.CountNull = report.CountNull;
                Ultimate.CountFalse = report.CountFalse;
                Ultimate.CountTrue = report.CountTrue;
                Ultimate.Score = float.Parse(report.Score.ToString());
                Ultimate.TestScore = report.TestScore;
                Ultimate.ReplyNull = report.ReplyNull;
                db.Update(Ultimate);
                await db.SaveChangesAsync();
            }
            else
            {
                await AddReportTest(TestID, UserID);
            }
        }

        public async Task AddReportTest(string TestID, string UserID)
        {
            var Result = await UltimateReport(TestID, UserID);
            TestsUltimate testsUltimate = new TestsUltimate()
            {
                UltimateID = CodeGeneratore.ActiveCode(),
                UserID = UserID,
                TestID = TestID,
                CountNull = Result.CountNull,
                CountFalse = Result.CountFalse,
                CountTrue = Result.CountTrue,
                ReplyNull = Result.ReplyNull,
                Score = float.Parse(Result.Score.ToString()),
                TestScore = Result.TestScore,
                DateTime = DateTime.Now
            };
            await AddUltimate(testsUltimate);
        }


        public async Task<ReportTestUltimateViewModel> UltimateReport(string TestID, string UserID)
        {
            var Test = await GetTest(TestID);
            ReportTestUltimateViewModel report = new ReportTestUltimateViewModel();
            foreach (var item in await GetAllQuestion(TestID))
            {
                var Reply = await GetReply(UserID, item.QuestionID);
                report.TestScore += float.Parse(item.Score.ToString());
                if (Reply != null)
                {
                    if (item.Method.Contains("تشریحی"))
                    {
                        var ReplyDescriptive = await GetReplyDescriptive(Reply.ReplyID);
                        if (ReplyDescriptive.IsCondition == true)
                        {
                            report.CountTrue += 1;
                            report.Score += item.Score;
                        }

                        if (ReplyDescriptive.IsCondition == false)
                        {
                            report.CountFalse += 1;
                            if (Test.IsNegative)
                            {
                                report.Score -= item.Score;
                            }
                        }

                        if (ReplyDescriptive.IsCondition == null)
                        {
                            report.CountNull += 1;
                        }
                    }
                    else
                    {
                        var ReplyOptional = await GetReplyOptional(Reply.ReplyID);
                        if (ReplyOptional.IsCondition)
                        {
                            report.CountTrue += 1;
                            report.Score += item.Score;
                        }
                        else
                        {
                            report.CountFalse += 1;
                            if (Test.IsNegative)
                            {
                                report.Score -= item.Score;
                            }
                        }
                    }
                }
                else
                {
                    report.ReplyNull += 1;
                }
            }
            return await Task.FromResult(report);
        }

        public async Task<bool> IsProfileUser(string UserID)
        {
            return await db.ProfileStudents.AnyAsync(s => s.UserID == UserID && s.IsCondition == true);
        }

        public async Task<bool> SetTestRequest(string testID, string userID)
        {
            if (!await db.TestRequests.AnyAsync(s => s.TestID == testID && s.UserID == userID))
            {
                db.Add(new TestRequests()
                {
                    TestRequestID = CodeGeneratore.ActiveCode(),
                    TestID = testID,
                    IsActive = null,
                    UserID = userID
                });
                await db.SaveChangesAsync();
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> ExistProfile(string userID)
        {
            return await db.ProfileStudents.AnyAsync(s => s.UserID == userID && s.IsCondition == true);
        }

        public async Task<bool> IsTestRequestClass(string userID, string testID)
        {
            int codeClass = db.ProfileStudents.SingleOrDefaultAsync(s => s.UserID == userID).Result.CodeClass;
            List<int> listClassTest =
                await db.TestClasses.Where(s => s.TestID == testID).Select(s => s.Class).ToListAsync();
            if (listClassTest.Any(s => s == codeClass))
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);
        }

        public async Task<bool> AuthStartTest(string userID, string testID)
        {
            if (await db.TestRequests.AnyAsync(s => s.TestID == testID && s.UserID == userID && s.IsActive == true))
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);
        }

        public async Task<string> GetFullNameTeacherTest(string testID)
        {
            var test = await GetTest(testID);
            string fullName = "";
            if (await db.ProfileStaffs.AnyAsync(s => s.UserID == test.UserID && s.StaffID != null))
            {
                fullName = db.ProfileStaffs.FirstOrDefaultAsync(s => s.UserID == test.UserID).Result.Title;
            }
            return await Task.FromResult(fullName);
        }

        public void Dispose()
        {
            db?.Dispose();
        }
    }
}
