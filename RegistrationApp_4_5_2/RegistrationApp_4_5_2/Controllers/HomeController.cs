using Microsoft.AspNetCore.Mvc;
using Ninject;
using RegistrationApp_4_5_2.Data.Entities;
using RegistrationApp_4_5_2.Services;
using RegistrationApp_4_5_2.Services.Abstraction;
using System.Linq;
using System.Threading.Tasks;
using ActionResult = System.Web.Mvc.ActionResult;
using Controller = System.Web.Mvc.Controller;
using JsonResult = System.Web.Mvc.JsonResult;
using RedirectToRouteResult = System.Web.Mvc.RedirectToRouteResult;

namespace RegistrationApp_4_5_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _service;

        public HomeController()
        {
            IKernel ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<IUserService>().To<UserService>();
            _service = ninjectKernel.Get<IUserService>();
        }

        public async Task<ActionResult> Index(string sortingProp)
        {
            var userList = await _service.GetUsersListAsync(sortingProp);

            ViewBag.NameSortParm = string.IsNullOrEmpty(sortingProp) ? "FullName" : "";
            ViewBag.IdSortParm = string.IsNullOrEmpty(sortingProp) ? "Id" : "";
            ViewBag.CitySortParm = string.IsNullOrEmpty(sortingProp) ? "City" : "";
            ViewBag.AgeSortParm = string.IsNullOrEmpty(sortingProp) ? "Age" : "";
            ViewBag.EmailSortParm = string.IsNullOrEmpty(sortingProp) ? "Email" : "";
            ViewBag.PhoneNumberSortParm = string.IsNullOrEmpty(sortingProp) ? "PhoneNumber" : "";

            return View("Index", userList.ToArray());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToRouteResult> CreateUserAsync(User newUser)
        {
            await _service.SaveUserAsync(newUser);

            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<JsonResult> CheckFullNameAsync([FromBody] string fullName)
        {
            var isValid = await _service.IsUserExistAsync(fullName);
            return Json(isValid);
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}