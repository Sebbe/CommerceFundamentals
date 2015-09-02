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
using CommerceTraining.Models.Pages;
using EPiServer.Web.Routing;
using EPiServer.Commerce.Catalog;

namespace CommerceTraining.Controllers
{

    // have a try and start from the "ASimple" and use the pattern here
    // ...doing it basic --> no ViewModel and other luxuries
    public class ProductController : ContentController<FashionProduct>
    {
        
        public ActionResult Index(FashionProduct currentContent)
        {
            /* It´s a "Product",  can do fine with the data-model 
             ...but it gets a bit more flexible with the ViewModel
             ... in lab - use as is with some CMS-functionality added */

            return View(currentContent);
        }
    }
}