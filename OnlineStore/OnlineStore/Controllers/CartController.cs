using OnlineStore.Models.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Areas.Admin.Controllers
{
    public class CartController : Controller
    {
        // GET: Admin/Cart
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CartPartial()
        {
            //Init cartVM
            CartVM model = new CartVM();

            //init quantity
            int qty = 0;

            //init price
            decimal price = 0m;

            //check for cart session
            if(Session["cart"] != null)
            {
                //get total qty and price
                var list = (List<CartVM>)Session["cart"];

                foreach(var item in list)
                {
                    qty += item.Quantity;
                    price += item.Quantity * item.Price;
                }
            }
            else
            {
                //or set qty and price to 0
                model.Quantity = 0;
                model.Price = 0m;
            }

            //return partial view with model
            return PartialView(model);
        }

        /*public ActionResult Index()
        {
            return View();
        }*/
    }
}