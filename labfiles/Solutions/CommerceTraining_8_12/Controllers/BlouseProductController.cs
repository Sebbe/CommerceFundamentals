using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using CommerceTraining.Models.Catalog;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.ServiceLocation;
using CommerceTraining.Models.Pages;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Web.Routing;
using EPiServer.Commerce.Catalog;
using Mediachase.Commerce.Catalog;
using System.Globalization;
using EPiServer.DataAccess;
using CommerceTraining.Models.ViewModels;

namespace CommerceTraining.Controllers
{
    public class BlouseProductController : MyControllerBase<BlouseProduct> 
    {
        public BlouseProductController(
        IContentLoader contentLoader
        , UrlResolver urlResolver
        , AssetUrlResolver assetUrlResolver
        , ThumbnailUrlResolver thumbnailUrlResolver 
        , AssetUrlConventions assetUrlConvensions
        )
            : base(contentLoader, urlResolver, assetUrlResolver, thumbnailUrlResolver)
        { }

        public ActionResult Index(BlouseProduct currentContent, StartPage currentPage)
        {
            IEnumerable<ContentReference> variationRefs = currentContent.GetVariants();
            IEnumerable<EntryContentBase> variations =
                _contentLoader.GetItems(variationRefs, new LoaderOptions()).OfType<EntryContentBase>();

            // ...will of course be a specific campaign page
            ContentReference campLink = ContentReference.StartPage;

            var model = new BlouseProductViewModel(currentContent, currentPage)
            {
                productVariations = variations, // ECF 
                campaignLink = campLink // CMS
            };

            return View(model);
        }


        public void CreateWithCode()
        {
            string nodeName = "myNode";
            string productName = "myProduct";
            string skuName = "mySku";

            // Get ReferenceConverter and LinksRepository
            ReferenceConverter refConv = ServiceLocator.Current.GetInstance<ReferenceConverter>();
            ILinksRepository linksRep = ServiceLocator.Current.GetInstance<ILinksRepository>();

            // Create Node
            ContentReference linkToParentNode = refConv.GetContentLink("Women_1");
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();

            var newNode = contentRepository.GetDefault<FashionNode>(linkToParentNode, new CultureInfo("en"));
            newNode.Code = nodeName;
            newNode.SeoUri = nodeName;
            newNode.Name = nodeName;
            newNode.DisplayName = nodeName;

            ContentReference newNodeRef = contentRepository.Save
                (newNode, SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);

            // Create Product
            var newProduct = contentRepository.GetDefault<BlouseProduct>(newNodeRef, new CultureInfo("en"));

            //Set some properties.
            newProduct.Code = productName;
            newProduct.SeoUri = productName;
            newProduct.Name = productName; 
            newProduct.DisplayName = productName; 
            newProduct.SeoInformation.Title = "SEO Title";
            newProduct.SeoInformation.Keywords = "Some keywords";
            newProduct.SeoInformation.Description = "A nice one";
            newProduct.MainBody = new XhtmlString("This new product is great");

            ContentReference newProductRef = contentRepository.Save
                (newProduct, SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);

            // Create SKU
            var newSku = contentRepository.GetDefault<FashionVariation>(newNodeRef, new CultureInfo("en"));

            newSku.Code = skuName;
            newSku.SeoUri = skuName;
            newSku.Name = skuName;
            newSku.DisplayName = skuName;

            ContentReference newSkuRef = contentRepository.Save
                (newSku, SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);

            // what differs from CMS
            ProductVariation prodVarRel = new ProductVariation();

            prodVarRel.Target = newSkuRef;
            prodVarRel.Source = newProductRef;
            prodVarRel.SortOrder = 100; 

            linksRep.UpdateRelation(prodVarRel);

            // done, but... 
            /* ...still missing Market, Inventory, Pricing and Media + a few other things */

            // ToDo: ...some redirect... somewhere

        
        }
    }
}