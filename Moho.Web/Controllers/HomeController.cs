using Moho.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Moho.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View(IocHelper.mongoHelper.FindScreens());
        }

        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View(new Screen());
        }

        // POST: Home/Create
        [HttpPost]
        public ActionResult Create(Screen screen)
        {
            try
            {
                screen.CreatedAt = DateTime.Now;
                screen.ScreenFields.AddRange(new ScreenField[]
                {
                    new ScreenField() { Name ="field1", MaxLength=5, Required=false, Type= ScreenFieldTypeEnum.Text },
                    new ScreenField() { Name ="field2", MaxLength=-1, Required=false, Type= ScreenFieldTypeEnum.Date },
                    new ScreenField() { Name ="field3", MaxLength=-1, Required=false, Type= ScreenFieldTypeEnum.Number },
                    new ScreenField() { Name ="field4", MaxLength=-1, Required=false, Type= ScreenFieldTypeEnum.Checkbox },
                    new ScreenField() { Name ="field5", MaxLength=255, Required=false, Type= ScreenFieldTypeEnum.TextArea },
                });

                IocHelper.mongoHelper.InsertScreen(screen);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(screen);
            }
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Home/Edit/5
        [HttpPost]
        public ActionResult Edit(Screen model)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Home/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
