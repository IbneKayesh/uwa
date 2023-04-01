using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UniversalWebApi.Models;
using UniversalWebApi.Services.Db;

namespace UniversalWebApi.Areas.Manager.Controllers
{
    public class BranchController : Controller
    {
        // GET: Manager/Branch
        public ActionResult Index()
        {
            List<UWA_BRANCH> objList = new List<UWA_BRANCH>();
            EQResultTable_v1 dbObj = Services.Db.UWA.GetAllBranch();
            if (dbObj.Result.SUCCESS && dbObj.Result.ROWS > 0)
            {
                objList = SevEF_v1.ConvertToList<UWA_BRANCH>(dbObj.Table);
            }
            return View(objList);
        }
        public ActionResult Create(string id, string db)
        {
            ViewBag.ButtonType = "Create";

            var obj = new UWA_BRANCH();
            if (!string.IsNullOrWhiteSpace(id))
            {
                EQResultTable_v1 dbObj = Services.Db.UWA.GetByIdBranch(id, db);
                if (dbObj.Result.SUCCESS && dbObj.Result.ROWS > 0)
                {
                    obj = SevEF_v1.ConvertToList<UWA_BRANCH>(dbObj.Table).First();
                    ViewBag.ButtonType = "Update";
                }
            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult Create(UWA_BRANCH obj)
        {
            var buttonValue = Request.Form["SaveButton"];
            if (ModelState.IsValid)
            {
                bool isNew = true;
                if (buttonValue == "Create")
                {
                    isNew = true;
                }
                else
                {
                    isNew = false;
                }
                EQResult_v1 dbObj = UWA.UpdateBranch(obj, isNew);
                if (dbObj.SUCCESS && dbObj.ROWS > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, dbObj.MESSAGES);
                }
            }
            ViewBag.ButtonType = buttonValue;
            return View(obj);
        }


        public ActionResult Delete(string id, string db)
        {
            var obj = new UWA_BRANCH();
            if (!string.IsNullOrWhiteSpace(id))
            {
                EQResult_v1 dbObj = UWA.DeleteBranch(id, db);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}