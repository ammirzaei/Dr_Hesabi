using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Interface;

namespace Dr_Hesabi.Controllers
{
    [AutoValidateAntiforgeryToken]
    [RequireHttps]
    public class ContentsController : Controller
    {
        private readonly IContents _IContents;
        private readonly IViewComponents _IViewComponents;

        public ContentsController(IContents iContents, IViewComponents iViewComponents)
        {
            _IContents = iContents;
            _IViewComponents = iViewComponents;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _IViewComponents.GetAllMajors());
        }

        [Route("/Contents/{majorTitle}")]
        public async Task<IActionResult> ShowListContentsMajor(string majorTitle)
        {
            var major = await _IContents.GetMajor(majorTitle);
            if (major == null)
                return NotFound();
            ViewData["MajorTitle"] = major.Title;
            ViewData["Description"] = major.ShortDescription;
            return View(await _IContents.GetAllContentsMajor(major.MajorID));
        }

        [Route("/Contents/{majorTitle}/{contentID}")]
        public async Task<IActionResult> ShowListContents(string majorTitle, string contentID)
        {
            string MajorTitle = majorTitle.Trim(new char[] { 'D', 'r', ' ' });
            var content = await _IContents.GetContentForMajor(MajorTitle, contentID);
            if (content == null)
                return NotFound();
            if (!await _IContents.IsContentinList(content.ContentID))
            {
                return RedirectToAction(nameof(ShowContent), new { majorTitle = majorTitle, contentID = content.ContentID });
            }
            ViewData["ContentTitle"] = content.Title;
            ViewData["MajorTitle"] = MajorTitle;
            return View(await _IContents.GetAllContents(content.MajorID, contentID));
        }

        [Route("/Content/{majorTitle}/{contentID}")]
        public async Task<IActionResult> ShowContent(string majorTitle, string contentID)
        {
            string MajorTitle = majorTitle.Trim(new char[] { 'D', 'r', ' ' });
            var content = await _IContents.GetContentForMajor(MajorTitle, contentID);
            if (content == null)
                return NotFound();
            if (await _IContents.IsContentinList(content.ContentID))
                return RedirectToAction(nameof(ShowListContents), new { majorTitle = majorTitle, contentID = content.ContentID });

            ViewData["MajorTitle"] = MajorTitle;
            return View(content);
        }

        [Route("/Contents/Search")]
        public async Task<IActionResult> Search(string q = "", string majorTitle = "")
        {
            if (ModelState.IsValid)
            {
                var major = await _IContents.GetMajor(majorTitle);
                if (major == null)
                    return NotFound();
                q = q == null ? "" : q.Trim();
                ViewBag.Search = q;
                ViewData["MajorTitle"] = major.Title;
                ViewData["Description"] = major.ShortDescription;
                return View(nameof(ShowListContentsMajor), await _IContents.GetSearchContents(major.MajorID, q));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
