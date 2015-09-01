using System;
using System.Web.Mvc;
using CommerceTraining.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using Mediachase.Commerce;

namespace CommerceTraining.Controllers
{
    public class CheckOutController : PageController<CheckOutPage>
    {
        ICurrentMarket _currentMarket; // not in fund... yet
        Injected<IContentLoader> _contentLoader;

        public CheckOutController(ICurrentMarket currentMarket)
        {
            _currentMarket = currentMarket;
        }

        // ToDo: in the first exercise (E1) Ship & Pay
        public ActionResult Index(CheckOutPage currentPage)
        {
            var model = new CheckOutViewModel(currentPage)
            {
                // ToDo: get shipments & payments
                
            };

            return View(model);
        }

        // ToDo: Get IEnumerables of Shipping and Payment methods along with ShippingRates
        // Exercise (E1) creation of GetPaymentMethods(), GetShipmentMethods() and GetShippingRates() goes below





        //Exercise (E2) Do CheckOut
        public ActionResult CheckOut(CheckOutViewModel model)
        {
            // ToDo: declare a variable for CartHelper


            // ToDo: Addresses (an If-Else)


            // ToDo: Define Shipping


            // Execute the "Shipping & Taxes - WF" (CartPrepare) ... and take care of the return object


            // ToDo: Define Shipping


            // Execute the "Payment activation - WF" (CartCheckout) ... and take care of the return object


            // Add a transaction scope and convert the cart to PO


            // Housekeeping below (Shipping release, OrderNotes and save the order)




            // Final steps, navigate to the order confirmation page
            StartPage home = _contentLoader.Service.Get<StartPage>(ContentReference.StartPage);
            ContentReference orderPageReference = home.Settings.orderPage;

            string passingValue = String.Empty;

            return RedirectToAction("Index", new { node = orderPageReference, passedAlong = passingValue });
        }
    }
}