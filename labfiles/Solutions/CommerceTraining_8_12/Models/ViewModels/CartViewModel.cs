using Mediachase.Commerce.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommerceTraining.Models.ViewModels
{
    public class CartViewModel
    {
        public IEnumerable<LineItem> lineItems { get; set; }
        public string cartTotal { get; set; }
        public List<string> messages { get; set; }
    }
}