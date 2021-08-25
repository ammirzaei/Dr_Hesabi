using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Interface;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Dr_Hesabi.ViewComponents
{
    [ViewComponent(Name = "ListCommentViewComponent")]
    public class ListCommentViewComponent : ViewComponent
    {
        private readonly IPanel _iPanel;

        public ListCommentViewComponent(IPanel iPanel)
        {
            _iPanel = iPanel;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id, string Method = "")
        {
            return View(await _iPanel.GetAllComments(id, Method));
        }
    }
}
