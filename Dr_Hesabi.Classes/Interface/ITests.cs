using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.ViewModel;
using Dr_Hesabi.DataLayers.Entity;

namespace Dr_Hesabi.Classes.Interface
{
    public interface ITests
    {
        Task<Tests> GetTest2(string testID, string title);
        Task<Tests> GetTest(string testID);
        Task<bool> ExistTest(string testID);
        Task<bool> ExistUser(string testID, string userID, string IP);
        Task AddLoginTest(LoginTests loginTest);
        Task<IEnumerable<Questions>> GetAllQuestionsTest(string testID, string title);
        Task<int> ExistAllTest(string? UserID);
        Task<IEnumerable<Choices>> GetAllChoicesQuestion(string testID);
        Task<Questions> GetQuestion(string questionID);
        Task<Questions> GetQuestionforChoice(string choiceId);
        Task<bool> ExistReply(string UserID, string QuestionID);
        Task AddReply(QuestionReplys questionReply);
        Task UpdateReply(QuestionReplys questionReply);
        Task<QuestionReplys> GetReply(string UserID, string QustionID);
        Task AddReplyOptional(ReplyOptional replyOptional);
        Task UpdateReplyOptional(ReplyOptional replyOptional);
        Task<bool> GetChoiceIsSuccess(string QuestionID, string ChoiceID);
        Task<ReplyOptional> GetReplyOptional(string ReplyID);
        Task<IEnumerable<ReplyOptional>> GetAllReplayOptional(string UserID, string TestID);
        Task AddReplyDescriptive(ReplyDescriptives replyDescriptive);
        Task UpdateReplyDescriptive(ReplyDescriptives replyDescriptive);
        Task<ReplyDescriptives> GetReplyDescriptive(string ReplyID);
        Task<IEnumerable<ReplyDescriptives>> GetAllReplyDescriptives(string UserID, string TestID);
        Task AddUltimate(TestsUltimate testsUltimate);
        Task<TestsUltimate> GetUltimateTask(string UserID, string TestID);
        Task<bool> ExistUltimate(string UserID, string TestID);
        Task<IEnumerable<Questions>> GetAllQuestion(string TestID);
        Task UpdateReportTest(string TestID, string UserID, ReportTestUltimateViewModel report);
        Task AddReportTest(string TestID, string UserID);
        Task<ReportTestUltimateViewModel> UltimateReport(string TestID, string UserID);
        Task<bool> IsProfileUser(string UserID);
        Task<bool> SetTestRequest(string testID, string userID);
        Task<bool> ExistProfile(string userID);
        Task<bool> IsTestRequestClass(string userID, string testID);
        Task<bool> AuthStartTest(string userID, string testID);
        Task<string> GetFullNameTeacherTest(string testID);
    }
}
