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
    [CatalogContentType(MetaClassName = "Fashion_Variant"
        , DisplayName = "FashionVariant"
        , GUID = "c0058f2d-9893-41d9-8c19-19c94d34ded1"
        , Description = "Use with mens shirts")]
    public class FashionVariation : VariationContent
    {
        
        [IncludeInDefaultSearch]
        [CultureSpecific]
        [Display(
            Name = "Main body",
            Description = "The main body, for text, images and tables.",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual XhtmlString MainBody { get; set; }

        [Searchable]
        [IncludeInDefaultSearch]
        public virtual string Size { get; set; }

        [Searchable]
        [IncludeValuesInSearchResults]
        public virtual string Color { get; set; }

        public virtual bool CanBeMonogrammed { get; set; }

        [Searchable]
        [IncludeValuesInSearchResults]
        [Tokenize]
        public virtual string ThematicTag { get; set; }


        public virtual ContentArea ProductArea { get; set; }
    }
}