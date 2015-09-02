using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using CommerceTraining.Models.Catalog;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using EPiServer.Commerce.Catalog.ContentTypes;
using CommerceTraining.Models.Pages;
using EPiServer.Commerce.Catalog;
using CommerceTraining.SupportingClasses;

namespace CommerceTraining.Controllers
{
    public class NodeController : MyControllerBase<FashionNode>
    {
        // ...needs to be there ... ToDo: "into the course"
        public NodeController(
            IContentLoader contentLoader
            , UrlResolver urlResolver
            , AssetUrlResolver assetUrlResolver
            , ThumbnailUrlResolver thumbnailUrlResolver
            , AssetUrlConventions assetUrlConvensions // Adv.
            )
            : base(contentLoader, urlResolver, assetUrlResolver, thumbnailUrlResolver)
        {

        }

        public ActionResult Index(NodeContent currentContent)
        {
            var model = new NodeEntryCombo(); // could change the name ...it´s a viewModel
            model.nodes = GetNodes(currentContent.ContentLink);
            model.entries = GetEntries(currentContent.ContentLink);

            return View(model); 

        }

    }
}