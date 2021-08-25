using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dr_Hesabi.DataLayers.Entity;

namespace Dr_Hesabi.Classes.Interface
{
    public interface ISurveys
    {
        Task<IEnumerable<Surveys>> GetAllSurveys();
        Task<IEnumerable<Surveys>> GetAllSurveysClose();
        Task<IEnumerable<SurveysQuestions>> GetAllSurveysQuestion(string surveyID, string title);
        Task<Surveys> GetSurveys(string surveyID, string title);
        Task<SurveysQuestions> GetQuestion(string questionID);
        Task<bool> ExistUserQuestionPermission(string userid, string questionid);
        Task<bool> ExistUserQuestion(string userid, string surveyid);
        Task AddVote(SurveysVotes surveysVotes);
        Task EditQuestion(SurveysQuestions surveysQuestions);
        Task<float> SumVoteQuestion(string questionID);
        Task<int> CountVoteQuestion(string questionID);
        Task EditVote(SurveysVotes surveysVotes);
        Task<SurveysVotes> GetVote(string UserID, string QuestionID);
        Task<List<string>> GetLabels(string Surveyid);
        Task<List<float>> GetValues(string Surveyid);
        Task<SurveysVotes> GetVoteinSurveynotPermission(string userID, string surveyID);
        Task<float> SumVotes(string QuestionId);
    }
}
