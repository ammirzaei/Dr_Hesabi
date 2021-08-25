using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Class;
using Dr_Hesabi.Classes.Interface;
using Dr_Hesabi.DataLayers.Context;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Dr_Hesabi.Classes.Service
{
    public class AccountService : IAccount, IDisposable
    {
        private DataBaseContext db;

        public AccountService(DataBaseContext context)
        {
            db = context;
        }
        public async Task AddUser(Users users)
        {
            db.Add(users);
            await db.SaveChangesAsync();
            //db.Add(new RoleSelects()
            //{
            //    SelectID = CodeGeneratore.ActiveCode(),
            //    UserID = users.UserID,
            //    RoleID = await LastRoleID()
            //});
            //await db.SaveChangesAsync();
        }

        public async Task<bool> ExistUserEmail(string Email)
        {
            return await db.Users.AnyAsync(s => s.Email.ToLower() == Email.ToLower());
        }

        public async Task<bool> ExistUserUserName(string UserName)
        {
            return await db.Users.AnyAsync(s => s.UserName.ToLower() == UserName.ToLower());
        }

        public async Task<string> LastRoleID()
        {
            return await db.Roles.MaxAsync(s => s.RoleID);
        }

        public async Task<Users> GetUser(string UsernameEmail, string Password)
        {
            Users user = new Users();
            user = await db.Users.FirstOrDefaultAsync(s => s.UserName.ToLower() == UsernameEmail.ToLower() && s.Password.ToLower() == Password.ToLower());
            if (user == null)
            {
                user = await db.Users.FirstOrDefaultAsync(s => s.Email.ToLower() == UsernameEmail.ToLower() && s.Password.ToLower() == Password.ToLower());
            }
            return user;
        }

        public async Task UpdateUser(Users users)
        {
            db.Update(users);
            await db.SaveChangesAsync();
        }

        public async Task<Users> GetUser2(string UserName)
        {
            return await db.Users.FirstOrDefaultAsync(s => s.UserName.ToLower() == UserName.ToLower());
        }

        public async Task<bool> ActiveCode(string ActiveCode)
        {
            var User = await db.Users.FirstOrDefaultAsync(s => s.ActiveCode == ActiveCode);
            if (User != null)
            {
                User.IsActive = true;
                User.ActiveCode = CodeGeneratore.ActiveCode();
                db.Update(User);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Users> GetUserInForget(string Email)
        {
            return await db.Users.FirstOrDefaultAsync(s => s.Email.ToLower() == Email.ToLower());
        }

        public async Task<Users> GetUserInRecover(string ActiveCode)
        {
            return await db.Users.FirstOrDefaultAsync(s => s.ActiveCode == ActiveCode && s.IsActive);
        }

        public bool ExistRoleUser(string UserID, string RoleName)
        {
            var Role = RoleName.Split(',');
            foreach (var item in Role)
            {
                if (db.RoleSelects.Any(s => s.Roles.Name == item && s.UserID == UserID))
                {
                    return true;
                }
            }
            return false;
        }

        public List<string> GetRoleUser(string UserID)
        {
            return db.RoleSelects.Where(s => s.UserID == UserID).Select(s => s.Roles.Name).ToList();
        }

        public async Task<bool> ExistProfile(string userID)
        {
            if (await db.ProfileStaffs.AnyAsync(s => s.UserID == userID) ||
                await db.ProfileStudents.AnyAsync(s => s.UserID == userID))
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);
        }

        public void Dispose()
        {
            db?.Dispose();
        }
    }
}
