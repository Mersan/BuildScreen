using System.Web.Mvc;
using BuildScreen.Configuration;
using BuildScreen.ViewModels;

namespace BuildScreen.Controllers
{
    public class HomeController : Controller
    {
        [Route]
        public ActionResult Index()
        {
            var model = new BuildStatusViewModel(Config.TCProject);
            return View(model);
        }
    }
}