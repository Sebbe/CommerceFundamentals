using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using CommerceTraining.Models.Catalog;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using CommerceTraining.Models.Pages;
using Mediachase.Commerce.Website.Helpers;
using Mediachase.Commerce.Orders;
using EPiServer.Commerce.Catalog;
using CommerceTraining.Models.ViewModels;
using CommerceTraining.SupportingClasses;
using Mediachase.Commerce.Inventory;
using Mediachase.Commerce.Catalog;
using System;
using Mediachase.Commerce.InventoryService;

namespace CommerceTraining.Controllers
{
    public class VariationController : MyControllerBase<FashionVariation>
    {
        public VariationController(
            IContentLoader contentLoader
            , UrlResolver urlResolver
            , AssetUrlResolver assetUrlResolver
            , ThumbnailUrlResolver thumbnailUrlResolver // use this in node listing instead
            )
            : base(contentLoader, urlResolver, assetUrlResolver, thumbnailUrlResolver)
        {
        }

        public ActionResult Index(FashionVariation currentContent)
        {
            var model = new FashionVariationViewModel
            {
                MainBody = currentContent.MainBody.ToString(),
                priceString = currentContent.GetDefaultPrice().UnitPrice.Amount.ToString("C"),
                discountPrice = StoreHelper.GetDiscountPrice(currentContent.LoadEntry()),
                image = GetDefaultAsset(currentContent),
                CanBeMonogrammed = currentContent.CanBeMonogrammed,
            };

            return View(model);
        }

        public ActionResult AddToCart(FashionVariation currentContent, decimal Quantity, string Monogram)
        {
            // ToDo: (lab D1)
            CartHelper ch = new CartHelper(Cart.DefaultName);
            LineItem lineitem = ch.AddEntry(currentContent.LoadEntry(), Quantity, false);

            // could have a bool-check for "monogrammable" 
            lineitem["Monogram"] = Monogram;

            // Maybe would like to set expiration-date on the cart ... that is custom, but exists on PO
            ch.Cart.ProviderId = "frontend"; // needs to be set for WF
            ch.Cart.AcceptChanges(); // Persist

            StartPage home = _contentLoader.Get<StartPage>(ContentReference.StartPage);
            ContentReference theRef = home.Settings.checkoutPage;

            string passingValue = String.Empty; // if needed, the cart for example
            return RedirectToAction("Index", new { node = theRef, passedAlong = passingValue });
        }

        // Optional lab in Mod. D - create a WishList and inspection in CM + config -alteration
        public void AddToWishList(FashionVariation currentContent)
        {
            CartHelper wishList = new CartHelper(CartHelper.WishListName); // note the arg.
            wishList.AddEntry(currentContent.LoadEntry());
        }
    }
}