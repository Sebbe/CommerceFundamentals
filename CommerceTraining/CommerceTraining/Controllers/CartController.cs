using System.Collections.Generic;
using System.Web.Mvc;
using CommerceTraining.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;

namespace CommerceTraining.Controllers
{
    public class CartController : PageController<CartPage>
    {
        Injected<IContentLoader> _contentLoader;

        // ToDo: declare a variable for CartHelper (lab D)


        // ...variable to take care of the workflow result
        List<string> wfMessages = new List<string>();

        public ActionResult Index(CartPage currentPage)
        {
            // ToDo: (lab D2)



            // The below is a dummy, remove when lab D2 is done
            return null;
        }

        public ActionResult Checkout()
        {
            // Final steps and go to checkout
            StartPage home = _contentLoader.Service.Get<StartPage>(ContentReference.StartPage);
            ContentReference theRef = home.Settings.checkoutPage;
            string passingValue = "This is fun"; // could pass the cart instead

            return RedirectToAction("Index", new { node = theRef, passedAlong = passingValue }); 
        }

    }
}