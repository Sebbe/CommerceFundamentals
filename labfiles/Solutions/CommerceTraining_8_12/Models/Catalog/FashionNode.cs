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
    [CatalogContentType(MetaClassName = "Fachion_Node", DisplayName = "FashionNode", GUID = "c2cc9a2c-f297-40e0-8b66-1503a79ae68e", Description = "")]
    public class FashionNode : NodeContent
    {
        [IncludeInDefaultSearch]
        [CultureSpecific]
        [Display(
            Name = "Main body",
            Description = "The main body will be shown in the main content area of the page, using the XHTML-editor you can insert for example text, images and tables.",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual XhtmlString MainBody { get; set; }

        [IncludeInDefaultSearch]
        [Searchable]
        [Tokenize]
        public virtual string themeTag { get; set; }

    }
}