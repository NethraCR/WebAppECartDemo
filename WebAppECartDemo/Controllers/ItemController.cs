using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppECartDemo.Models;
using WebAppECartDemo.ViewModel;

namespace WebAppECartDemo.Controllers
{
    //[Authorize]
    public class ItemController : Controller
    {
        private ECartDBEntities objECartDbEntities;

        public ItemController()
        {
            objECartDbEntities = new ECartDBEntities();
        }
        // GET: Item
        [Authorize(Roles = "Admin,User,Customer")]
        public ActionResult Index()
        {
            ItemViewModel objItemViewModel = new ItemViewModel();
            objItemViewModel.CategorySelectListItem = (from objCat in objECartDbEntities.Categoriesses
                                                       select new SelectListItem()
                                                       {
                                                           Text = objCat.CategoryName,
                                                           Value = objCat.CategoryId.ToString(),
                                                           Selected = true

                                                       });

            return View(objItemViewModel);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        

        public JsonResult Index(ItemViewModel objItemViewModel)
        {
            string NewImage = Guid.NewGuid() + Path.GetExtension(objItemViewModel.ImagePath.FileName);
            //here we will get extention image name and extention in images folder
            objItemViewModel.ImagePath.SaveAs(Server.MapPath("~/Images/" + NewImage));

            Item objItem = new Item();
            objItem.ImagePath = "~/Images/" + NewImage;
            objItem.CategoryId = objItemViewModel.CategoryId;
            objItem.Description = objItemViewModel.Description;
            objItem.ItemCode = objItemViewModel.ItemCode;
            objItem.ItemId = Guid.NewGuid();
            objItem.ItemName = objItemViewModel.ItemName;
            objItem.ItemPrice = objItemViewModel.ItemPrice;
            objECartDbEntities.Items.Add(objItem);
            objECartDbEntities.SaveChanges();
            return Json(new { Success = true, Message = "Item is added Successfully." }, JsonRequestBehavior.AllowGet);
        }
    }
}