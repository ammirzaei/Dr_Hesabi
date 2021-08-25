using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.ViewModel;
using Dr_Hesabi.DataLayers.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Areas.Admin.ViewComponents.ListModel
{
    [ViewComponent(Name = "ListStaffsViewComponent")]
    public class ListStaffsViewComponent : ViewComponent
    {
        private readonly DataBaseContext db;

        public ListStaffsViewComponent(DataBaseContext context)
        {
            db = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<AdminViewModel.ListStaffsViewModel> model = new List<AdminViewModel.ListStaffsViewModel>();
            model.AddRange(db.Staffs.Include(s => s.Staffs2).Select(s => new AdminViewModel.ListStaffsViewModel()
            {
                Title = s.Title,
                ParentID = s.ParentID,
                StaffID = s.StaffID,
                IsNative = true
            }));
            model.AddRange(db.ProfileStaffs.Where(s => s.StaffID != null).Select(s => new AdminViewModel.ListStaffsViewModel()
            {
                Title = s.Title,
                ParentID = s.StaffID,
                StaffID = s.ProfileStaffID,
                IsNative = false
            }));
            ViewData["ListProfileStaffs"] = await db.ProfileStaffs.Where(s => s.StaffID == null).ToListAsync();
            return View(await Task.FromResult(model));
        }
    }
}
