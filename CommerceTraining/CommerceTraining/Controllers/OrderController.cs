using System.Web.Mvc;
using CommerceTraining.Models.Pages;
using EPiServer.Web.Mvc;

namespace CommerceTraining.Controllers
{
    public class OrderController : PageController<OrderPage>
    {
        public ActionResult Index(OrderPage currentPage, string passedAlong)
        {
            var model = new OrderViewModel()
            {
                TrackingNumber = passedAlong
            };

            return View(model);
        }
    }
}