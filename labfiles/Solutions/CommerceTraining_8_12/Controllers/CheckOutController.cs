using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using CommerceTraining.Models.Pages;
using Mediachase.Commerce.Orders.Dto;
using Mediachase.Commerce.Orders.Managers;
using Mediachase.Commerce;
using Mediachase.Commerce.Website.Helpers;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Engine;
using System;
using EPiServer.Security;
using Mediachase.Commerce.Customers;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Core;

namespace CommerceTraining.Controllers
{
    public class CheckOutController : PageController<CheckOutPage>
    {
        //ICurrentMarket _currentMarket; // not in fund... yet
        Injected<IContentLoader> _contentLoader;

        //public CheckOutController(ICurrentMarket currentMarket)
        //{
        //    _currentMarket = currentMarket;
        //}

        // ToDo: in the first exercise (E1) Ship & Pay
        public ActionResult Index(CheckOutPage currentPage)
        {
            var model = new CheckOutViewModel(currentPage)
            {
                // ToDo: get shipments & payments
                PaymentMethods = GetPaymentMethods(),
                ShipmentMethods = GetShipmentMethods(),
                ShippingRates = GetShippingRates(),
            };

            return View(model);
        }

        // ToDo: Get IEnumerables of Shipping and Payment methods along with ShippingRates
        // Exercise (E1) creation of GetPaymentMethods(), GetShipmentMethods() and GetShippingRates() goes below

        private IEnumerable<ShippingRate> GetShippingRates() 
        {
            List<ShippingRate> shippingRates = new List<ShippingRate>();
            IEnumerable<ShippingMethodDto.ShippingMethodRow> shippingMethods = GetShipmentMethods();

            foreach (var item in shippingMethods)
            {
                shippingRates.Add(new ShippingRate(item.ShippingMethodId, item.DisplayName, new Money(item.BasePrice, item.Currency)));
            }

            return shippingRates;
        }

        private IEnumerable<PaymentMethodDto.PaymentMethodRow> GetPaymentMethods()
        {
            return new List<PaymentMethodDto.PaymentMethodRow>(
                  PaymentManager.GetPaymentMethodsByMarket(MarketId.Default.Value).PaymentMethod);
        }

        private IEnumerable<ShippingMethodDto.ShippingMethodRow> GetShipmentMethods()
        {
            return new List<ShippingMethodDto.ShippingMethodRow>(
                ShippingManager.GetShippingMethodsByMarket(MarketId.Default.Value, false).ShippingMethod);
        }




        //Exercise (E2) Do CheckOut
        public ActionResult CheckOut(CheckOutViewModel model)
        {
            // ToDo: declare a variable for CartHelper
            CartHelper ch = new CartHelper(Cart.DefaultName);

            int orderAddressId = 0;

            // ToDo: Addresses (an If-Else)
            if (CustomerContext.Current.CurrentContact == null)
            {
                // Anonymous... one way of "doing it"... for example, if no other address exist
                orderAddressId = ch.Cart.OrderAddresses.Add(
                   new OrderAddress
                   {
                       CountryCode = "SWE",
                       CountryName = "Sweden",
                       Name = "SomeCustomerAddress",
                       DaytimePhoneNumber = "123456",
                       FirstName = "John",
                       LastName = "Smith",
                       Email = "John@company.com",
                   });
            }
            else
            {
                // Logged in
                if (CustomerContext.Current.CurrentContact.PreferredShippingAddress == null)
                {
                    // no pref. address set... so we set one for the contact
                    CustomerAddress newCustAddress =
                        CustomerAddress.CreateForApplication(AppContext.Current.ApplicationId);
                    newCustAddress.AddressType = CustomerAddressTypeEnum.Shipping; // mandatory
                    newCustAddress.ContactId = CustomerContext.Current.CurrentContact.PrimaryKeyId;
                    newCustAddress.CountryCode = "SWE";
                    newCustAddress.CountryName = "Sweden";
                    newCustAddress.Name = "new customer address"; // mandatory
                    newCustAddress.DaytimePhoneNumber = "123456";
                    newCustAddress.FirstName = CustomerContext.Current.CurrentContact.FirstName;
                    newCustAddress.LastName = CustomerContext.Current.CurrentContact.LastName;
                    newCustAddress.Email = "GuitarWorld@Thule.com";

                    // note: Line1 & City is what is shown in CM at a few places... not the Name
                    CustomerContext.Current.CurrentContact.AddContactAddress(newCustAddress);
                    CustomerContext.Current.CurrentContact.SaveChanges();

                    // ... needs to be in this order
                    CustomerContext.Current.CurrentContact.PreferredShippingAddress = newCustAddress;
                    CustomerContext.Current.CurrentContact.SaveChanges(); // need this ...again 

                    // then, for the cart
                    orderAddressId = ch.Cart.OrderAddresses.Add(new OrderAddress(newCustAddress));
                }
                else
                {
                    // there is a preferred address set (and, a fourth alternative exists... do later )
                    OrderAddress orderAddress =
                        new OrderAddress(CustomerContext.Current.CurrentContact.PreferredShippingAddress);

                    // then, for the cart
                    orderAddressId = ch.Cart.OrderAddresses.Add(orderAddress);
                }
            }

            // Depending how it was created...
            OrderAddress address = ch.FindAddressById(orderAddressId.ToString());

            // ToDo: Define Shipping
            ShippingMethodDto.ShippingMethodRow theShip =
                ShippingManager.GetShippingMethod(model.SelectedShipId).ShippingMethod.First();

            int shippingId = ch.Cart.OrderForms[0].Shipments.Add(
                new Shipment
                { // ...removing anything?
                    ShippingAddressId = address.Name, // note: use no custom prefixes
                    ShippingMethodId = theShip.ShippingMethodId,
                    ShippingMethodName = theShip.Name,
                    ShipmentTotal = theShip.BasePrice,
                    ShipmentTrackingNumber = "My tracking number",
                });

            // get the Shipping ... check to see if the Shipping knows about the LineItem
            Shipment firstOrderShipment = ch.Cart.OrderForms[0].Shipments.FirstOrDefault();

            // First (and only) OrderForm 
            LineItemCollection lineItems = ch.Cart.OrderForms[0].LineItems;

            // ...basic now... one OrderForm - one Shipping
            foreach (LineItem lineItem in lineItems)
            {
                int index = lineItems.IndexOf(lineItem);
                if ((firstOrderShipment != null) && (index != -1))
                {
                    firstOrderShipment.AddLineItemIndex(index, lineItem.Quantity);
                }
            }


            // Execute the "Shipping & Taxes - WF" (CartPrepare) ... and take care of the return object
            WorkflowResults resultPrepare = ch.Cart.RunWorkflow(OrderGroupWorkflowManager.CartPrepareWorkflowName);
            List<string> wfMessagesPrepare = new List<string>(OrderGroupWorkflowManager.GetWarningsFromWorkflowResult(resultPrepare));


            // ToDo: Define Shipping
            PaymentMethodDto.PaymentMethodRow thePay = PaymentManager.GetPaymentMethod(model.SelectedPayId).PaymentMethod.First();
            Payment firstOrderPayment = ch.Cart.OrderForms[0].Payments.AddNew(typeof(OtherPayment));

            // ... need both below
            firstOrderPayment.Amount = firstOrderShipment.SubTotal + firstOrderShipment.ShipmentTotal; // will change...
            firstOrderPayment.BillingAddressId = address.Name;
            firstOrderPayment.PaymentMethodId = thePay.PaymentMethodId;
            firstOrderPayment.PaymentMethodName = thePay.Name;
            // ch.Cart.CustomerName = "John Smith"; // ... this line overwrites what´s in there, if logged in


            // Execute the "Payment activation - WF" (CartCheckout) ... and take care of the return object
            // ...activates the gateway (same for shipping)
            WorkflowResults resultCheckout = ch.Cart.RunWorkflow(OrderGroupWorkflowManager.CartCheckOutWorkflowName, false);
            List<string> wfMessagesCheckout = new List<string>(OrderGroupWorkflowManager.GetWarningsFromWorkflowResult(resultCheckout));
            //ch.RunWorkflow("CartValidate") ... can see this (or variations)

            string trackingNumber = String.Empty;
            PurchaseOrder purchaseOrder = null;

            // Add a transaction scope and convert the cart to PO
            using (var scope = new Mediachase.Data.Provider.TransactionScope())
            {
                purchaseOrder = ch.Cart.SaveAsPurchaseOrder(); 
                ch.Cart.Delete();
                ch.Cart.AcceptChanges();
                trackingNumber = purchaseOrder.TrackingNumber;
                scope.Complete();
            }

            // Housekeeping below (Shipping release, OrderNotes and save the order)
            OrderStatusManager.ReleaseOrderShipment(purchaseOrder.OrderForms[0].Shipments[0]); 

            OrderNotesManager.AddNoteToPurchaseOrder(purchaseOrder, DateTime.UtcNow.ToShortDateString() + " released for shipping", OrderNoteTypes.System, CustomerContext.Current.CurrentContactId);

            purchaseOrder.ExpirationDate = DateTime.UtcNow.AddDays(30);
            purchaseOrder.Status = OrderStatus.InProgress.ToString();

            purchaseOrder.AcceptChanges(); // need this here, else no "order-note" persisted


            // Final steps, navigate to the order confirmation page
            StartPage home = _contentLoader.Service.Get<StartPage>(ContentReference.StartPage);
            ContentReference orderPageReference = home.Settings.orderPage;

            string passingValue = trackingNumber;

            return RedirectToAction("Index", new { node = orderPageReference, passedAlong = passingValue });
        }
    }
}