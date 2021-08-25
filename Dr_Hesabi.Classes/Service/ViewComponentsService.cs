using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Interface;
using Dr_Hesabi.Classes.ViewModel;
using Dr_Hesabi.DataLayers.Context;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Classes.Service
{
    public class ViewComponentsService : IViewComponents,IDisposable
    {
        private readonly DataBaseContext db;

        public ViewComponentsService(DataBaseContext context)
        {
            db = context;
        }
        public async Task<IEnumerable<Staffs>> GetAllStaffs()
        {
            return await db.Staffs.ToListAsync();
        }

        public async Task<IEnumerable<GetAllMajorsViewModel>> GetAllMajors()
        {
            return await db.Majors.Select(s => new GetAllMajorsViewModel()
            {
                Title = s.Title,
                ImageName = s.ImageName
            }).ToListAsync();
        }

        public void Dispose()
        {
            db?.Dispose();
        }
    }
}
