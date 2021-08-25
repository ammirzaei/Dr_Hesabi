using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Class;
using Dr_Hesabi.Classes.Interface;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Dr_Hesabi.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    [RequireHttps]
    public class TestsController : Controller
    {
        private readonly ITests _ITests;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public TestsController(ITests iTests, IWebHostEnvironment hostingEnvironment)
        {
            _ITests = iTests;
            _hostingEnvironment = hostingEnvironment;
        }

        [Route("Tests/{id}/{title}")]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string id, string title)
        {
            var Error = TempData["ErrorExistUser"];
            if (Error != null)
            {
                ViewBag.Error = Error.ToString();
            }

            var model = await _ITests.GetTest2(id, title);
            if (model == null || model.StartDateTime.ToDate() != DateTime.Now.ToDate())
            {
                return NotFound();
            }

            if (User.Identity.IsAuthenticated)
            {
                string UserID = HttpContext.User.Claims
                    .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
                if (!await _ITests.AuthStartTest(UserID, model.TestID))
                {
                    ViewBag.ErrorAuthTest = true;
                    ViewBag.DisabledBtn = true;
                }
            }
            else
                ViewBag.DisabledBtn = true;

            ViewData["TeacherFullName"] = await _ITests.GetFullNameTeacherTest(id);
            return View(model);
        }

        [Route("Tests/Participating/{id}/{title}")]
        public async Task<IActionResult> ShowQuestions(string id, string title)
        {
            var Test = await _ITests.GetTest(id);
            if (Test == null || Test.Title != title)
            {
                return NotFound();
            }
            if (!await _ITests.ExistTest(id))
            {
                return RedirectToAction(nameof(Index));
            }
            string UserID = HttpContext.User.Claims
                .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
            if (!await _ITests.AuthStartTest(UserID, id))
            {
                TempData["ErrorExistUser"] = "Auth";
                return RedirectToAction(nameof(Index));
            }
            if (await _ITests.ExistUltimate(UserID, Test.TestID))
            {
                TempData["ErrorExistUser"] = "Ultimate";
                return RedirectToAction(nameof(Index));
            }

            string IP = HttpContext.Connection.RemoteIpAddress.ToString();
            if (!await _ITests.ExistUser(id, UserID, IP))
            {
                LoginTests loginTest = new LoginTests()
                {
                    LoginID = CodeGeneratore.ActiveCode(),
                    UserID = UserID,
                    DateTime = DateTime.Now,
                    IP = IP,
                    TestID = id
                };
                await _ITests.AddLoginTest(loginTest);
            }

            ViewData["TestTitle"] = Test.Title;
            ViewData["TestDes"] = Test.Description;
            ViewData["TestID"] = Test.TestID;
            ViewData["TestTime"] = Test.EndDateTime.GetSocond();
            ViewData["Choices"] = await _ITests.GetAllChoicesQuestion(id);
            ViewData["ReplyOptional"] = await _ITests.GetAllReplayOptional(UserID, Test.TestID);
            ViewData["ReplyDescriptives"] = await _ITests.GetAllReplyDescriptives(UserID, Test.TestID);
            return View(await _ITests.GetAllQuestionsTest(id, title));
        }

        public async Task<int> SaveReplyOptional(string ChoiceID)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return 3;
                }
                var Question = await _ITests.GetQuestionforChoice(ChoiceID);
                if (Question == null)
                {
                    return 1;
                }
                if (!await _ITests.ExistTest(Question.TestID))
                {
                    return 2;
                }

                string UserID = HttpContext.User.Claims
                    .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
                if (!await _ITests.AuthStartTest(UserID, Question.TestID))
                {
                    return 5;
                }
                if (await _ITests.ExistReply(UserID, Question.QuestionID))
                {
                    var Reply = await _ITests.GetReply(UserID, Question.QuestionID);
                    Reply.DateTime = DateTime.Now;
                    await _ITests.UpdateReply(Reply);

                    if (Reply.ReplyOptionals.Any())
                    {
                        var ReplyOptional = await _ITests.GetReplyOptional(Reply.ReplyID);
                        ReplyOptional.ChoiceID = ChoiceID;
                        ReplyOptional.IsCondition = await _ITests.GetChoiceIsSuccess(Question.QuestionID, ChoiceID);
                        await _ITests.UpdateReplyOptional(ReplyOptional);
                    }
                    else
                    {
                        ReplyOptional replyOptional = new ReplyOptional()
                        {
                            OptionalID = CodeGeneratore.ActiveCode(),
                            ChoiceID = ChoiceID,
                            ReplyID = Reply.ReplyID,
                            IsCondition = await _ITests.GetChoiceIsSuccess(Question.QuestionID, ChoiceID)
                        };
                        await _ITests.AddReplyOptional(replyOptional);
                    }
                }
                else
                {
                    QuestionReplys questionReply = new QuestionReplys()
                    {
                        ReplyID = CodeGeneratore.ActiveCode(),
                        DateTime = DateTime.Now,
                        QuestionID = Question.QuestionID,
                        UserID = UserID
                    };
                    await _ITests.AddReply(questionReply);

                    ReplyOptional replyOptional = new ReplyOptional()
                    {
                        OptionalID = CodeGeneratore.ActiveCode(),
                        ChoiceID = ChoiceID,
                        ReplyID = questionReply.ReplyID,
                        IsCondition = await _ITests.GetChoiceIsSuccess(Question.QuestionID, ChoiceID)
                    };
                    await _ITests.AddReplyOptional(replyOptional);
                }
                return 0;
            }
            catch
            {
                return 1;
            }
        }
        public async Task<int> SaveReplyDescriptive(string QuestionID, string? Text, IFormFile? file)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return 3;
                }
                var Question = await _ITests.GetQuestion(QuestionID);
                if (Question == null)
                {
                    return 1;
                }
                if (!await _ITests.ExistTest(Question.TestID))
                {
                    return 2;
                }

                string UserID = HttpContext.User.Claims
                    .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
                if (!await _ITests.AuthStartTest(UserID, Question.TestID))
                {
                    return 5;
                }
                string FileName = null;
                if (file != null)
                {
                    if (!CheckContentImage.IsImage(file))
                    {
                        return 4;
                    }
                    FileName = FileGeneratore.NameFile(file.FileName);
                }

                if (await _ITests.ExistReply(UserID, QuestionID))
                {
                    var Reply = await _ITests.GetReply(UserID, QuestionID);
                    Reply.DateTime = DateTime.Now;
                    await _ITests.UpdateReply(Reply);

                    if (Reply.ReplyDescriptives.Any())
                    {
                        if (file != null)
                        {
                            var OldFileName = Reply.ReplyDescriptives.FirstOrDefault().ImageName;
                            FileGeneratore.DeleteFile("Replys/Thumb", OldFileName, _hostingEnvironment.WebRootPath);

                            await FileGeneratore.SaveFileResizer("Replys", "Replys/Thumb", FileName, file, 800, _hostingEnvironment.WebRootPath);
                        }
                        var ReplyDescriptive = await _ITests.GetReplyDescriptive(Reply.ReplyID);
                        ReplyDescriptive.Text = Text;
                        ReplyDescriptive.ImageName = FileName;
                        await _ITests.UpdateReplyDescriptive(ReplyDescriptive);
                    }
                    else
                    {
                        if (file != null)
                        {
                            var OldFileName = Reply.ReplyDescriptives.FirstOrDefault().ImageName;
                            FileGeneratore.DeleteFile("Replys/Thumb", OldFileName, _hostingEnvironment.WebRootPath);

                            await FileGeneratore.SaveFileResizer("Replys", "Replys/Thumb", FileName, file, 800, _hostingEnvironment.WebRootPath);
                        }
                        ReplyDescriptives replyDescriptive = new ReplyDescriptives()
                        {
                            DescriptiveID = CodeGeneratore.ActiveCode(),
                            ReplyID = Reply.ReplyID,
                            Text = Text,
                            IsCondition = null,
                            ImageName = FileName
                        };
                        await _ITests.AddReplyDescriptive(replyDescriptive);
                    }
                }
                else
                {
                    if (file != null)
                    {
                        await FileGeneratore.SaveFileResizer("Replys", "Replys/Thumb", FileName, file, 800, _hostingEnvironment.WebRootPath);
                    }
                    QuestionReplys questionReply = new QuestionReplys()
                    {
                        ReplyID = CodeGeneratore.ActiveCode(),
                        DateTime = DateTime.Now,
                        QuestionID = QuestionID,
                        UserID = UserID
                    };
                    await _ITests.AddReply(questionReply);
                    ReplyDescriptives replyDescriptive = new ReplyDescriptives()
                    {
                        DescriptiveID = CodeGeneratore.ActiveCode(),
                        ReplyID = questionReply.ReplyID,
                        Text = Text,
                        IsCondition = null,
                        ImageName = FileName
                    };
                    await _ITests.AddReplyDescriptive(replyDescriptive);
                }
                return 0;
            }
            catch
            {
                return 1;
            }
        }

        [Route("Tests/Ultimate/{TestID}")]
        public async Task<IActionResult> Ultimate(string TestID)
        {
            var Test = await _ITests.GetTest(TestID);
            if (Test == null)
            {
                return NotFound();
            }

            string UserID = HttpContext.User.Claims
                .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
            if (!await _ITests.ExistUltimate(UserID, Test.TestID))
            {
                await _ITests.AddReportTest(TestID, UserID);
            }

            ViewData["TestTitle"] = Test.Title;
            ViewData["IsUltimate"] = Test.IsUltimate;
            return View(await _ITests.GetUltimateTask(UserID, TestID));
        }

        [AllowAnonymous]
        public async Task<IActionResult> HaveTest()
        {
            string UserID = String.Empty;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                UserID = HttpContext.User.Claims
                   .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
            }
            int Result = await _ITests.ExistAllTest(UserID);
            return Ok(Result);
        }

        [AllowAnonymous]
        [Route("Tests/Request/{id}")]
        public async Task<IActionResult> Request(string id)
        {
            var test = await _ITests.GetTest(id);
            if (test == null)
                return NotFound();
            ViewData["TeacherFullName"] = await _ITests.GetFullNameTeacherTest(id);
            return View(test);
        }

        [HttpPost]
        [Route("Tests/Request/{id}")]
        [ActionName("Request")]
        public async Task<IActionResult> SetRequest(string id)
        {
            var test = await _ITests.GetTest(id);
            if (ModelState.IsValid)
            {
                if (test == null)
                    return NotFound();
                string UserID = HttpContext.User.Claims
                    .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();

                if (await _ITests.ExistProfile(UserID))
                {
                    if (await _ITests.IsTestRequestClass(UserID, id))
                    {
                        if (await _ITests.SetTestRequest(id, UserID))
                            ViewBag.SetRequestCondition = true;
                        else
                            ViewBag.SetRequestCondition = false;
                    }
                    else
                    {
                        ViewBag.SetRequestClass = true;
                    }
                }
                else
                    ViewBag.SetRequestProfile = true;

                ViewData["TeacherFullName"] = await _ITests.GetFullNameTeacherTest(id);
                return View(test);
            }
            ViewData["TeacherFullName"] = await _ITests.GetFullNameTeacherTest(id);
            return View(test);
        }

        [Route("/Tests/ExistTest/{id}")]
        public async Task<bool> ExistTest(string id)
        {
            return await _ITests.ExistTest(id);
        }
    }
}
