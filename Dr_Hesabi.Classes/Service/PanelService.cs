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
    public class PanelService : IPanel,IDisposable
    {
        private readonly DataBaseContext db;

        public PanelService(DataBaseContext context)
        {
            this.db = context;
        }

        public async Task<IEnumerable<Blogs>> GetAllBlogs()
        {
            return await db.Blogs.Where(s => s.IsActive).OrderByDescending(s => s.DateTime).ToListAsync();
        }

        public async Task<Blogs> GetBlog(string blogID, string title)
        {
            return await db.Blogs.FirstOrDefaultAsync(s => s.BlogID == blogID && s.Title == title);
        }

        public async Task UpdateBlog(Blogs blog)
        {
            db.Update(blog);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Bests>> GetAllBests()
        {
            return await db.Bests.Where(s => s.ParentID == null && s.IsActive && s.Bests1.Any(a => a.IsActive)).ToListAsync();
        }

        public async Task<IEnumerable<Bests>> GetAllBestItems(string bestID, string title)
        {
            return await db.Bests.Where(s => s.Bests2.BestID == bestID && s.Bests2.Title == title && s.IsActive).ToListAsync();
        }

        public async Task<Bests> GetBest(string bestID, string title)
        {
            return await db.Bests.FirstOrDefaultAsync(s => s.BestID == bestID && s.Title == title);
        }

        public async Task<IEnumerable<News>> GetAllNews()
        {
            return await db.Newses.Where(s => s.IsActive).OrderByDescending(s => s.DateTime).ToListAsync();
        }
        public async Task<News> GetNews(string newsID, string title)
        {
            return await db.Newses.FirstOrDefaultAsync(s => s.NewsID == newsID && s.Title == title && s.IsActive);
        }

        public async Task UpdateNews(News news)
        {
            db.Update(news);
            await db.SaveChangesAsync();
        }

        public async Task<Majors> GetMajor(string title)
        {
            return await db.Majors.FirstOrDefaultAsync(s => s.Title == title);
        }

        public async Task<GetStaffViewModel> GetStaff(string staffID, string title)
        {
            if (await db.Staffs.AnyAsync(s => s.StaffID == staffID && s.Title == title))
            {
                var staff = await db.Staffs.Include(s => s.Staffs2).FirstOrDefaultAsync(s => s.Title == title && s.StaffID == staffID);
                return await Task.FromResult(new GetStaffViewModel()
                {
                    StaffID = staff.StaffID,
                    ParentTitle = staff.Staffs2.Title,
                    Title = staff.Title,
                    ImageName = staff.ImageName,
                    Description = staff.Text
                });
            }
            else
            {
                var profile = await db.ProfileStaffs.Include(s => s.Staffs).FirstOrDefaultAsync(s => s.Title == title && s.ProfileStaffID == staffID);
                return await Task.FromResult(new GetStaffViewModel()
                {
                    StaffID = profile.ProfileStaffID,
                    ParentTitle = profile.Staffs.Title,
                    Title = profile.Title,
                    ImageName = profile.ImageName,
                    Description = profile.Description
                });
            }
        }

        public async Task<IEnumerable<GetAllStaffsViewModel>> GetAllStaffs()
        {
            List<GetAllStaffsViewModel> model = new List<GetAllStaffsViewModel>();

            model.AddRange(db.Staffs.Select(s => new GetAllStaffsViewModel()
            {
                StaffID = s.StaffID,
                ParentID = s.ParentID,
                Title = s.Title,
                ImageName = s.ImageName
            }));
            model.AddRange(db.ProfileStaffs.Where(s => s.StaffID != null).Select(s => new GetAllStaffsViewModel()
            {
                StaffID = s.ProfileStaffID,
                ParentID = s.StaffID,
                Title = s.Title,
                ImageName = s.ImageName
            }));

            return await Task.FromResult(model.ToList());
        }

        public async Task<IEnumerable<Gallerys>> GetAllGallerys()
        {
            return await db.Gallerys.Include(s => s.Gallery1).Where(s => s.ParentID == null).ToListAsync();
        }

        public async Task<IEnumerable<Gallerys>> GetImageGallery(string galleryID, string title)
        {
            return await db.Gallerys.Include(s => s.Gallery2).Where(s => s.ParentID == galleryID && s.Gallery2.Title == title).ToListAsync();
        }

        public async Task<Gallerys> GetGallery(string galleryID, string title)
        {
            return await db.Gallerys.FirstOrDefaultAsync(s => s.Title == title && s.GalleryID == galleryID);
        }

        public async Task<IEnumerable<Comments>> GetAllComments(string panelID, string title)
        {
            return await db.Comments.Include(s => s.Users).Where(s => s.PanelID == panelID && s.Method == title).ToListAsync();
        }

        public async Task AddComment(Comments comments)
        {
            db.Add(comments);
            await db.SaveChangesAsync();
        }

        public async Task<Comments> GetComment(string commentID)
        {
            return await db.Comments.FirstOrDefaultAsync(s => s.CommentID == commentID);
        }

        public async Task UpdateComment(Comments comments)
        {
            db.Update(comments);
            await db.SaveChangesAsync();
        }

        public async Task DeleteComment(Comments comments)
        {
            db.Remove(comments);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            db?.Dispose();
        }
    }
}
