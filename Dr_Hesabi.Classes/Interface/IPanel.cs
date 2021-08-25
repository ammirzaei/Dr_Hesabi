using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.ViewModel;
using Dr_Hesabi.DataLayers.Entity;

namespace Dr_Hesabi.Classes.Interface
{
    public interface IPanel
    {
        Task<IEnumerable<Blogs>> GetAllBlogs();
        Task<Blogs> GetBlog(string blogID, string title); 
        Task UpdateBlog(Blogs blog);
        Task<IEnumerable<Bests>> GetAllBests();
        Task<IEnumerable<Bests>> GetAllBestItems(string bestID, string title);
        Task<Bests> GetBest(string bestID, string title);
        Task<IEnumerable<News>> GetAllNews();
        Task<News> GetNews(string newsID, string title);
        Task UpdateNews(News news);
        Task<Majors> GetMajor(string title);
        Task<GetStaffViewModel> GetStaff(string staffID, string title);
        Task<IEnumerable<GetAllStaffsViewModel>> GetAllStaffs();
        Task<IEnumerable<Gallerys>> GetAllGallerys();
        Task<IEnumerable<Gallerys>> GetImageGallery(string galleryID, string title);
        Task<Gallerys> GetGallery(string galleryID, string title);
        Task<IEnumerable<Comments>> GetAllComments(string panelID,string title);
        Task AddComment(Comments comments);
        Task<Comments> GetComment(string commentID);
        Task UpdateComment(Comments comments);
        Task DeleteComment(Comments comments);
    }
}
