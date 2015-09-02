using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Commerce.Catalog.ContentTypes;

namespace CommerceTraining.Models.Catalog
{
    [CatalogContentType(MetaClassName = "Fashion_Product", DisplayName = "FashionProduct", GUID = "93faff15-3d07-4d95-a64e-34df366675cb", Description = "")]
    public class FashionProduct : ProductContent
    {
        [IncludeInDefaultSearch]
        [CultureSpecific]
        [Display(
            Name = "Main body",
            Description = "The main body will be shown in the main content area of the page, using the XHTML-editor you can insert for example text, images and tables.",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual XhtmlString MainBody { get; set; }

    }
}