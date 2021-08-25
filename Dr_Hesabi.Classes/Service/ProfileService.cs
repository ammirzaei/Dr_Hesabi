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
    public class ProfileService : IProfile,IDisposable
    {
        private readonly DataBaseContext db;
        private readonly ITests _ITests;


        public ProfileService(DataBaseContext db, ITests iTests)
        {
            this.db = db;
            _ITests = iTests;
        }

        public async Task<ProfileStudents> GetProfile(string UserID)
        {
            return await db.ProfileStudents.Include(s => s.Users).FirstOrDefaultAsync(s => s.UserID == UserID);
        }

        public async Task AddProfile(ProfileStudents profiles)
        {
            db.Add(profiles);
            await db.SaveChangesAsync();
        }

        public async Task UpdateProfile(ProfileStudents profiles)
        {
            db.Update(profiles);
            await db.SaveChangesAsync();
        }

        public async Task<bool> ExistProfileStudent(string UserID)
        {
            return await db.ProfileStudents.AnyAsync(s => s.UserID == UserID);
        }

        public IEnumerable<TestsUltimate> GetAllUltimate(string UserID)
        {
            List<TestsUltimate> List = new List<TestsUltimate>();
            foreach (var item in db.TestsUltimate.Include(s => s.Tests).Where(s => s.UserID == UserID))
            {
                if (item.Tests.IsUltimate)
                {
                    List.Add(item);
                }
                else
                {
                    if (item.Tests.EndDateTime < DateTime.Now)
                    {
                        List.Add(item);
                    }
                }
            }

            return List.ToList();
            //return await db.TestsUltimate.Include(s => s.Tests).Where(s => s.UserID == UserID).ToListAsync();
        }

        public async Task CheckUltimate(string UserID)
        {
            foreach (var item in db.Tests.Where(s => s.EndDateTime < DateTime.Now && s.IsActive))
            {
                if (await db.QuestionReplys.Include(s => s.Questions).AnyAsync(s => s.Questions.TestID == item.TestID))
                {
                    if (!await db.TestsUltimate.AnyAsync(s => s.UserID == UserID && s.TestID == item.TestID))
                    {
                        await _ITests.AddReportTest(item.TestID, UserID);
                    }
                }
            }
        }

        public async Task AddProfileRequest(ProfileRequests profileRequest)
        {
            if (await db.ProfileRequests.AnyAsync(s => s.UserID == profileRequest.UserID))
            {
                var request = await db.ProfileRequests.FirstOrDefaultAsync(s => s.UserID == profileRequest.UserID);
                request.IsCondition = null;
                request.CreateDate = DateTime.Now;
                request.Description = profileRequest.Description;
                db.Update(request);
            }
            else
            {
                db.Add(profileRequest);
            }
            await db.SaveChangesAsync();
        }

        public async Task<bool> ExistProfileRequest(string userID)
        {
            return await Task.FromResult(await db.ProfileRequests.AnyAsync(s => s.UserID == userID));
        }

        public async Task<bool?> IsConditionProfileRequest(string userID)
        {
            var request = await db.ProfileRequests.FirstOrDefaultAsync(s => s.UserID == userID);
            if (request != null)
                return await Task.FromResult(request.IsCondition);
            else
            {
                bool? isC = null;
                return await Task.FromResult(isC);
            }
        }

        public async Task DeleteProfileRequest(string userID)
        {
            if (await ExistProfileRequest(userID))
            {
                var request = await db.ProfileRequests.FirstOrDefaultAsync(s => s.UserID == userID);
                db.Remove(request);
                await db.SaveChangesAsync();
            }
        }

        public async Task<string> GetRoleIDTeacher()
        {
            var role = await db.Roles.FirstOrDefaultAsync(s => s.Name == "Teacher");
            return await Task.FromResult(role.RoleID);
        }

        public async Task<string> GetRoleIDUser()
        {
            var role = await db.Roles.FirstOrDefaultAsync(s => s.Name == "User");
            return await Task.FromResult(role.RoleID);
        }
        public async Task<bool> IsUserStaff(string userID)
        {
            string roleID = await GetRoleIDUser();
            return await db.RoleSelects.AnyAsync(s => s.UserID == userID && s.RoleID != roleID);
        }

        public async Task<ProfileStaffs> GetProfileStaff(string userID)
        {
            return await db.ProfileStaffs.FirstOrDefaultAsync(s => s.UserID == userID);
        }

        public void Dispose()
        {
            db?.Dispose();
        }
    }
}
