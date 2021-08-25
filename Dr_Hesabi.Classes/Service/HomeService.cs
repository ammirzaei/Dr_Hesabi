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

namespace Dr_Hesabi.Classes.Service
{
    public class HomeService : IHome,IDisposable
    {
        private readonly DataBaseContext db;

        public HomeService(DataBaseContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<Sliders>> GetAllSliders()
        {
            return await db.Sliders.Where(s => s.IsActive && s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now).ToListAsync();
        }


        public void Dispose()
        {
            db?.Dispose();
        }
    }
}
