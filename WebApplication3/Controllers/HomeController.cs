using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFeatureManager featureManager;

        public HomeController(IFeatureManager featureManager)
        {
            this.featureManager = featureManager;
        }
        public async Task<ActionResult> Index()
        {
            TargetingContext targetingContext = new TargetingContext
            {
                Groups = new List<string>() { "Mercy" }
            };

            if (await featureManager.IsEnabledAsync("Beta"))
            {
                Console.WriteLine("Welcome to the beta!");
            }
            if (await featureManager.IsEnabledAsync("Beta", targetingContext))
                ViewBag.CQRS = true;

            return View();
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