using System;
using System.Text;
using System.Web.Mvc;
using CoronaStore.Models;
using CoronaStore.Services;

namespace CoronaStore.Controllers
{
    public class UsersController : Controller
    {
        UserServices userService = new UserServices();

        public ActionResult AttemptLogin(LoginDetails details)
        {
            LoginResult result = userService.AttemptLogin(details);
            if (result.LoginSucceeded)
            {
                User user = userService.GetUser(details.Username);
                Session["User"] = (user.FirstName);
                Session["IsAdmin"] = (user.IsAdmin == 1);
                Session["UserID"] = (user.UserID);


            }
            return Json(result);
        }

        public ActionResult Register(RegisterDetails details)
        {
            if (userService.Register(details))
                return Json(true);

            return Json(false);
        }

        public ActionResult CheckName(string username)
        {
            bool a = userService.DoesUserExists(username);
            return Json(a, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            HttpContext.Session.Remove("IsAdmin");

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProductsByUser()
        {
            return Json(userService.ProductsByUser(), JsonRequestBehavior.AllowGet);
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }
    }
}