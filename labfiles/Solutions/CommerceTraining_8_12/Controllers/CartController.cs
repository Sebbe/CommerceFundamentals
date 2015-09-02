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
        CartHelper ch = new CartHelper(Cart.DefaultName);

        // ...variable to take care of the workflow result
        List<string> wfMessages = new List<string>();

        public ActionResult Index(CartPage currentPage)
        {
            // (added for D2)
            if (ch.LineItems.Count() == 0) // cart could exist but not containing LineItems? ... gets the "Index Out Of Range" then
            {
                wfMessages.Add("No LineItems");

                var model = new CartViewModel // using a bit of it
                {
                    //lineItems = new List<LineItem>(),
                    //cartTotal = "0",
                    messages = wfMessages
                };

                //return View(model);
                return View("NoCart", model);
            }
            else
            {
                // ToDo: (lab D2)
                //WorkflowResults result = ch.Cart.RunWorkflow(OrderGroupWorkflowManager.CartValidateWorkflowName);
                WorkflowResults result = OrderGroupWorkflowManager.RunWorkflow
                    (ch.Cart, OrderGroupWorkflowManager.CartValidateWorkflowName);

                //List<string> wfMessages = new List<string>(OrderGroupWorkflowManager.GetWarningsFromWorkflowResult(result));
                wfMessages = OrderGroupWorkflowManager.GetWarningsFromWorkflowResult(result).ToList();

                ch.Cart.AcceptChanges();

                var model = new CartViewModel
                {
                    lineItems = ch.LineItems,
                    cartTotal = ch.Cart.Total.ToString("C"),
                    messages = wfMessages
                };

                return View(model);
            }

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