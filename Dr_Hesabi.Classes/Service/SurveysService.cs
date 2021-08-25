using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Interface;
using Dr_Hesabi.DataLayers.Context;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Classes.Service
{
    public class SurveysService : ISurveys,IDisposable
    {
        private readonly DataBaseContext db;

        public SurveysService(DataBaseContext db)
        {
            this.db = db;
        }
        public async Task<IEnumerable<Surveys>> GetAllSurveys()
        {
            var Model = db.Surveys.Include(s => s.SurveysQuestions).Where(s => s.IsActive && s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now);
            return await Model.ToListAsync();
        }

        public async Task<IEnumerable<Surveys>> GetAllSurveysClose()
        {
            var Model = db.Surveys.Include(s => s.SurveysQuestions).Where(s => s.EndDate < DateTime.Now);
            return await Model.ToListAsync();
        }

        public async Task<IEnumerable<SurveysQuestions>> GetAllSurveysQuestion(string surveyID, string title)
        {
            var Model = db.SurveysQuestions.Include(s => s.Surveys).Include(s => s.SurveysVotes).Where(s => s.Surveys.SurveyID == surveyID && s.Surveys.Title == title);
            return await Model.ToListAsync();
        }

        public async Task<Surveys> GetSurveys(string surveyID, string title)
        {
            return await db.Surveys.FirstOrDefaultAsync(s => s.SurveyID == surveyID && s.Title == title);
        }

        public async Task<SurveysQuestions> GetQuestion(string questionID)
        {
            return await db.SurveysQuestions.Include(s => s.Surveys).FirstOrDefaultAsync(s => s.QuestionID == questionID);
        }

        public async Task<bool> ExistUserQuestionPermission(string userid, string questionid)
        {
            return await db.SurveysVotes.AnyAsync(s => s.UserID == userid && s.QuestionID == questionid);
        }

        public async Task<bool> ExistUserQuestion(string userid, string surveyid)
        {
            return await db.SurveysVotes.AnyAsync(s =>
                s.UserID == userid && s.SurveysQuestions.Surveys.SurveyID == surveyid);
        }

        public async Task AddVote(SurveysVotes surveysVotes)
        {
            db.Add(surveysVotes);
            await db.SaveChangesAsync();
        }

        public async Task EditQuestion(SurveysQuestions surveysQuestions)
        {
            try
            {
                db.Update(surveysQuestions);
                await db.SaveChangesAsync();
            }
            catch
            {
                surveysQuestions.SumVote = 0;
                db.Update(surveysQuestions);
                await db.SaveChangesAsync();
            }

        }

        public async Task<float> SumVoteQuestion(string questionID)
        {
            return await db.SurveysVotes.Where(s => s.QuestionID == questionID).SumAsync(s => s.Vote);
        }

        public async Task<int> CountVoteQuestion(string questionID)
        {
            return await db.SurveysVotes.Where(s => s.QuestionID == questionID).CountAsync();
        }

        public async Task EditVote(SurveysVotes surveysVotes)
        {
            db.Update(surveysVotes);
            await db.SaveChangesAsync();
        }

        public async Task<SurveysVotes> GetVote(string UserID, string QuestionID)
        {
            return await db.SurveysVotes.FirstOrDefaultAsync(s => s.UserID == UserID && s.QuestionID == QuestionID);
        }

        public async Task<List<string>> GetLabels(string Surveyid)
        {
            return await db.SurveysQuestions.Where(s => s.SurveyID == Surveyid).Select(s => s.Title).ToListAsync();
        }

        public async Task<List<float>> GetValues(string Surveyid)
        {
            return await db.SurveysQuestions.Where(s => s.SurveyID == Surveyid).Select(s => s.SumVote).ToListAsync();
        }

        public async Task<SurveysVotes> GetVoteinSurveynotPermission(string userID, string surveyID)
        {
            return await db.SurveysVotes.FirstOrDefaultAsync(s =>
                s.UserID == userID && s.SurveysQuestions.SurveyID == surveyID);
        }

        public async Task<float> SumVotes(string QuestionId)
        {
            return await SumVoteQuestion(QuestionId) /
                   await CountVoteQuestion(QuestionId);
        }

        public void Dispose()
        {
            db?.Dispose();
        }
    }
}
