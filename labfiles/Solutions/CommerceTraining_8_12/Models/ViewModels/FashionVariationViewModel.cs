using CommerceTraining.SupportingClasses;
using EPiServer.Core;
using Mediachase.Commerce.Catalog.Objects;
using Mediachase.Commerce.Inventory;
using Mediachase.Commerce.InventoryService;
//using EPiServer.Commerce.SpecializedProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommerceTraining.Models.ViewModels
{
    public class FashionVariationViewModel
    {
        public Price discountPrice { get; set; }
        public string priceString { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public bool CanBeMonogrammed { get; set; }
        public string MainBody { get; set; }
    }
}