using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CoronaStore.Models;
using Accord.MachineLearning.Rules;
using CoronaStore.Services;

namespace CoronaStore.Controllers
{
    public class ShopController : Controller
    {
        ShopService shopService = new ShopService();

        public ActionResult GetAllProducts()
        {
            return Json(shopService.GetAllProductsFromInventory());
        }

        public ActionResult SalesPerCategory()
        {
            return Json(shopService.SalesPerCategory(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAllCategories()
        {
            var Categories = shopService.GetAllCategories();
            return Json(Categories, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAllAndNullCategories()
        {
            var Categories = shopService.GetAllAndNullCategories();
            return Json(Categories, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchProducts(string term, int? price)
        {
            return View(shopService.SearchProducts(term,price));
        }

        [HttpDelete]
        public ActionResult DeleteProduct(int id)
        {
            if (shopService.DeleteProduct(id))
                return Json(id);
            else
                return Json(false);
        }

        [HttpGet]
        public ActionResult ProductsStock()
        {
            return Json(shopService.GetProductsStock());
        }
        
        [HttpGet]
        public ActionResult GetUserProducts(int userId)
        {
            return Json(shopService.getUserProducts(userId), JsonRequestBehavior.AllowGet);
        }
    }
}
