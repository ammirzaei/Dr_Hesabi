using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dr_Hesabi.DataLayers.Entity;

namespace Dr_Hesabi.Classes.Interface
{
    public interface IAccount
    {
        Task AddUser(Users users);
        Task<bool> ExistUserEmail(string Email);
        Task<bool> ExistUserUserName(string UserName);
        Task<string> LastRoleID();
        Task<Users> GetUser(string UsernameEmail, string Password);
        Task UpdateUser(Users users);
        Task<Users> GetUser2(string UserName);
        Task<bool> ActiveCode(string ActiveCode);
        Task<Users> GetUserInForget(string Email);
        Task<Users> GetUserInRecover(string ActiveCode);
        bool ExistRoleUser(string UserID, string RoleName);
        List<string> GetRoleUser(string UserID);
        Task<bool> ExistProfile(string userID);
    }
}
