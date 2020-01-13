using OnlineStore.Models.Data;
using OnlineStore.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Controllers
{
    public class PagesController : Controller
    {
        // GET: Index/{page}
        public ActionResult Index(string page= "")
        {
            //Get/set slug
            if (page == "")
                page = "home";

            //declare model and dto
            PageVM model;
            PageDTO dto;

            //check if page exists
            using (Db db = new Db())
            {
                if(!db.Pages.Any(x => x.Slug.Equals(page)))
                {
                    return RedirectToAction("Index", new { page = "" });
                }
            }

            //get page DTO
            using (Db db = new Db())
            {
                dto = db.Pages.Where(x => x.Slug == page).FirstOrDefault();
            }

            //set page title
            ViewBag.PageTitle = dto.Title;
            
            //chack for sidebar
            if(dto.HasSideBar == true)
            {
                ViewBag.SideBar = "Yes";
            }
            else
            {
                ViewBag.SideBar = "No";
            }

            //init model
            model = new PageVM(dto);

            //return view with model
            return View(model);
        }

        public ActionResult PagesMenuPartial()
        {
            //declare a list
            List<PageVM> pageVMList;

            //Get all pages except the home page
            using(Db db = new Db())
            {
                pageVMList = db.Pages.ToArray().OrderBy(x => x.Sorting).Where(x => x.Slug != "home").Select(x => new PageVM(x)).ToList();
            }
            //return the partial view with list
            return PartialView(pageVMList);
        }

        public ActionResult SidebarPartial()
        {
            //declare the model
            SidebarVM model;

            //init moodel
            using(Db db = new Db())
            {
                SidebarDTO dto = db.Sidebar.Find(1);
                model = new SidebarVM(dto);
            }

            //return PartialView view with model
            return PartialView(model);
        } 
    }
}