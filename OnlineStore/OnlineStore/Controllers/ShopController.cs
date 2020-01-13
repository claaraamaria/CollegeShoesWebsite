using OnlineStore.Models.Data;
using OnlineStore.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Pages");
        }

        public ActionResult CategoryMenuPartial()
        {
            //Declare list of CategoryVM
            List<CategoryVM> categoryVMList;

            //Init the list
            using (Db db = new Db())
            {
                categoryVMList = db.Categories.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoryVM(x)).ToList();
            }

            //Return partial with list
            return PartialView(categoryVMList);
        }

        // GET: shop/category/name
        public ActionResult Category(string name)
        {
            //Declare a list of ProductVM
            List<ProductVM> productVMList;

            using (Db db = new Db())
            {
                //Get category id
                CategoryDTO categoryDTO = db.Categories.Where(x => x.Slug == name).FirstOrDefault();
                int catId = categoryDTO.Id;

                //Init the list
                productVMList = db.Products.ToArray().Where(x => x.CategoryId == catId).Select(x => new ProductVM(x)).ToList();

                //Get category name
                var productCat = db.Products.Where(x => x.CategoryId == catId).FirstOrDefault();
                ViewBag.CategoryName = productCat.CategoryName;
            }

            //Return view with the list
            return View(productVMList);
        }

        // GET: shop/product-details/name
        [ActionName("product-details")]
        public ActionResult ProductDetails(string name)
        {
            //Declare the VM and DTO
            ProductVM model;
            ProductDTO dto;

            //init product ID
            int id = 0;

            using (Db db = new Db())
            {
                //check if product exists
                if(!db.Products.Any(x => x.Slug.Equals(name)))
                {
                    return RedirectToAction("Index", "Shop");
                }

                //init productDTO
                dto = db.Products.Where(x => x.Slug == name).FirstOrDefault();

                //get id
                id = dto.Id;

                //init model
                model = new ProductVM(dto);
            }
            //get gallery images
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                                               .Select(fn => Path.GetFileName(fn));

            //return view with model
            return View("ProductDetails", model);
        }

       /* public ActionResult Action()
        {
            return View();
        }*/
    }
}