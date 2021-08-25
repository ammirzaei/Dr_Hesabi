using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Interface;
using Dr_Hesabi.DataLayers.Context;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Classes.Service
{
   public class SettingService:ISetting,IDisposable
    {
        private DataBaseContext db;

        public SettingService(DataBaseContext context)
        {
            db = context;
        }

        public async Task<Setting> GetSetting()
        {
            return await db.Setting.FirstOrDefaultAsync();
        }

        public void Dispose()
        {
            db?.Dispose();
        }
    }
}
