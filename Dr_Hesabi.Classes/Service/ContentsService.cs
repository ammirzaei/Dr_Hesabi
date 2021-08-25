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
    public class ContentsService : IContents,IDisposable
    {
        private readonly DataBaseContext db;

        public ContentsService(DataBaseContext db)
        {
            this.db = db;
        }

        public async Task<Majors> GetMajor(string title)
        {
            return await db.Majors.FirstOrDefaultAsync(s => s.Title == title);
        }

        public async Task<IEnumerable<Contents>> GetAllContentsMajor(string majorID)
        {
            return await db.Contents.Include(s => s.Majors).Where(s => s.MajorID == majorID && s.ParentID == null).ToListAsync();
        }

        public async Task<IEnumerable<Contents>> GetAllContents(string majorID, string contentID)
        {
            return await db.Contents.Where(s => s.MajorID == majorID && s.ParentID == contentID).ToListAsync();
        }

        public async Task<Contents> GetContent(string contentID)
        {
            return await db.Contents.FirstOrDefaultAsync(s => s.ContentID == contentID);
        }

        public async Task<Contents> GetContentForMajor(string majorTitle, string contentID)
        {
            return await db.Contents.FirstOrDefaultAsync(s => s.ContentID == contentID && s.Majors.Title == majorTitle);
        }

        public async Task<bool> IsContentinList(string contentID)
        {
            return await db.Contents.AnyAsync(s => s.ContentID == contentID && s.Description == null);
        }

        public async Task<IEnumerable<Contents>> GetSearchContents(string majorID, string q)
        {
            IQueryable<Contents> list = db.Contents.Where(s => s.MajorID == majorID && s.ParentID != null && s.Description != null);
            return await list.Include(s=>s.Majors).Where(s => EF.Functions.Like(s.Title, $"%{q}%")).ToListAsync();
        }

        public void Dispose()
        {
            db?.Dispose();
        }
    }
}
