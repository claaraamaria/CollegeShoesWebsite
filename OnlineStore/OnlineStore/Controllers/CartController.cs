﻿using OnlineStore.Models.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Areas.Admin.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            //Init the cart list
            var cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();

            //check if the cart is empty
            if(cart.Count == 0 || Session["cart"] == null)
            {
                ViewBag.Message = "Your cart is empty.";
                return View();
            }

            //calculate total and sa to viewBag
            decimal total = 0m;

            foreach(var item in cart)
            {
                total += item.Total;
            }
            ViewBag.GrandTotal = total;

            //return view with list
            return View(cart);
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