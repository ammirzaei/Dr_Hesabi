using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dr_Hesabi.DataLayers.Entity;

namespace Dr_Hesabi.Classes.Interface
{
    public interface IProfile
    {
        Task<ProfileStudents> GetProfile(string UserID);
        Task AddProfile(ProfileStudents profiles);
        Task UpdateProfile(ProfileStudents profiles);
        Task<bool> ExistProfileStudent(string UserID);
        IEnumerable<TestsUltimate> GetAllUltimate(string UserID);
        Task CheckUltimate(string UserID);
        Task AddProfileRequest(ProfileRequests profileRequest);
        Task<bool> ExistProfileRequest(string userID);
        Task<bool?> IsConditionProfileRequest(string userID);
        Task DeleteProfileRequest(string userID);
        Task<bool> IsUserStaff(string userID);
        Task<ProfileStaffs> GetProfileStaff(string userID);
    }
}
