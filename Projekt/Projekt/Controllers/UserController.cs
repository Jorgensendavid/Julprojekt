using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekt.Controllers
{
    public class UserController : BaseController
    {
        public ActionResult Index()
        {
            var users = db.Users.ToList();
            return View(users);
        }
    }
}