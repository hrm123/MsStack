using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContactMgmtApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult list()
        {
            return PartialView();
        }

        public ActionResult editcontact(int id)
        {
            return PartialView();
        }

        public ActionResult addcontact()
        {
            return PartialView();
        }

        public ActionResult Index()
        {
            return View("ContactMgmt");
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