using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using CommerceTraining.Models.Pages;
using Mediachase.Commerce.Website.Helpers;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce;
using Mediachase.Commerce.Orders.Managers;
using Mediachase.Commerce.Engine;
using CommerceTraining.Models.ViewModels;
using Mediachase.Commerce.Catalog.Objects;
using CommerceTraining.Models.Catalog;
using Mediachase.Commerce.Catalog;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.ServiceLocation;

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