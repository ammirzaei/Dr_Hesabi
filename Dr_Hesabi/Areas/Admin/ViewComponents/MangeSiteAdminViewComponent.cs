using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.ViewModel;
using Dr_Hesabi.DataLayers.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Areas.Admin.ViewComponents
{
    public class MangeSiteAdminViewComponent : ViewComponent
    {
        private readonly DataBaseContext db;

        public MangeSiteAdminViewComponent(DataBaseContext _db)
        {
            this.db = _db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            AdminViewModel.ManageSiteViewModel Model = new AdminViewModel.ManageSiteViewModel()
            {
                CountSlider = db.Sliders.Count(),
                CountUser = db.Users.Count(),
                CountNews = db.Newses.Count(),
                CountStaffs = await db.Staffs.Where(s => s.ParentID != null).CountAsync() + await db.ProfileStaffs.Where(s => s.StaffID != null).CountAsync(),
                CountMajors = db.Majors.Count(),
                CountBlogs = db.Blogs.Count(),
                CountBests = await db.Bests.Where(s => s.ParentID == null).CountAsync(),
                CountGallerys = await db.Gallerys.Where(s => s.ParentID == null).CountAsync(),
                CountSurveys = await db.Surveys.CountAsync(),
                CountAttachments = await db.Attachments.Where(s => s.PanelName == "Admin").CountAsync()
            };
            return View(Model);
        }
    }
}
