using System.Collections.Generic;
using Mediachase.Commerce.Orders;

namespace CommerceTraining.Models.ViewModels
{
    public class CartViewModel
    {
        public IEnumerable<LineItem> lineItems { get; set; }
        public string cartTotal { get; set; }
        public List<string> messages { get; set; }
    }
}