using Moho.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Moho.Web.Controllers
{
    public class ScreenController : Controller
    {
        // GET: Screen
        public ActionResult Show(string uriName)
        {
            Screen screen = IocHelper.mongoHelper.FindScreenByUriName(uriName);
            List<Dictionary<string, object>> items = IocHelper.mongoHelper.FindUnknownCollection(screen);

            ScreenShowViewModel screenShowViewModel = new ScreenShowViewModel();
            screenShowViewModel.Screen = screen;
            screenShowViewModel.Items = items;

            return View(screenShowViewModel);
        }

        public ActionResult Create(string uriName)
        {
            Screen screen = IocHelper.mongoHelper.FindScreenByUriName(uriName);
            return View(screen);
        }

        [HttpPost]
        public ActionResult Create(string uriName, FormCollection collection)
        {
            Screen screen = IocHelper.mongoHelper.FindScreenByUriName(uriName);
            Dictionary<string, object> values = new Dictionary<string, object>();

            foreach (string key in screen.ScreenFields.Select(x => x.Name))
            {
                values.Add(key, collection.GetValues(key)[0]);
            }

            IocHelper.mongoHelper.InsertToUnknownCollection(screen, values);

            return RedirectToAction("Show", new { uriName = uriName });
        }

        public ActionResult Edit(string id, string uriName)
        {
            Screen screen = IocHelper.mongoHelper.FindScreenByUriName(uriName);
            var item = IocHelper.mongoHelper.FindItemFromUnknownCollection(screen, id);

            ScreenEditViewModel screenEditViewModel = new ScreenEditViewModel();
            screenEditViewModel.Screen = screen;
            screenEditViewModel.Item = item;

            return View(screenEditViewModel);
        }

        [HttpPost]
        public ActionResult Edit(string id, string uriName, FormCollection collection)
        {
            Screen screen = IocHelper.mongoHelper.FindScreenByUriName(uriName);
            Dictionary<string, object> values = new Dictionary<string, object>();

            foreach (string key in screen.ScreenFields.Select(x => x.Name))
            {
                values.Add(key, collection[key]);
            }

            values.Add("_id", collection["_id"]);

            IocHelper.mongoHelper.UpdateToUnknownCollection(screen, values);

            return RedirectToAction("Show", new { uriName = uriName });
        }

        public ActionResult Delete(string id, string uriName)
        {
            Screen screen = IocHelper.mongoHelper.FindScreenByUriName(uriName);
            IocHelper.mongoHelper.DeleteFromUnknownCollection(screen, id);

            return RedirectToAction("Show", new { uriName = uriName });
        }
    }
}