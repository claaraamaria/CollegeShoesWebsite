﻿using OnlineStore.Models.Data;
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
                model.Quantity = qty;
                model.Price = price;
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

        public ActionResult AddToCartPartial(int id)
        {
            //init CartVM list
            List<CartVM> cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();

            //init CartVM
            CartVM model = new CartVM();

            using (Db db = new Db()) 
            {
                //get the product
                ProductDTO product = db.Products.Find(id);

                //check if the product is already in the cart
                var productInCart = cart.FirstOrDefault(x => x.ProductId == id);

                // if not, add new
                if (productInCart == null)
                {
                    cart.Add(new CartVM()
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Quantity = 1,
                        Price = product.Price,
                        Image = product.ImageName
                    });
                }
                else 
                {
                    //if it is, increment
                    productInCart.Quantity++;
                }
            }
            //get total quantity and price and add to model
            int qty = 0;
            decimal price = 0m;

            foreach(var item in cart)
            {
                qty += item.Quantity;
                price += item.Quantity * item.Price;
            }
            model.Quantity = qty;
            model.Price = price;

            //save cart back to session
            Session["cart"] = cart;

            //return partial view with model
            return PartialView(model);
        }

        // GET: /Cart/IncrementProduct
        public JsonResult IncrementProduct(int productId)
        {
            //Init cart list
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            using (Db db = new Db())
            {
                //Get cartVM from list
                CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);

                //increment qty
                model.Quantity++;

                //store needed data
                var result = new { qty = model.Quantity, price = model.Price };

                //return Json with data
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        // GET: /Cart/DecrementProduct
        public ActionResult DecrementProduct(int productId)
        {
            //Init cart list
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            using (Db db = new Db())
            {
                //Get model from list
                CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);

                //decrement qty
                if (model.Quantity > 1)
                {
                    model.Quantity--;
                }
                else
                {
                    model.Quantity = 0;
                    cart.Remove(model);
                }

                //store needed data
                var result = new { qty = model.Quantity, price = model.Price };

                //return Json with data
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //GET: /Cart/RemoveProduct
        public void RemoveProduct(int productId)
        {
            //Init cart list
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            using (Db db = new Db())
            {
                //Get model from list
                CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);

                //Remove model from list
                cart.Remove(model);
            }

        }

        /*public ActionResult Index()
        {
            return View();
        }*/
    }
}