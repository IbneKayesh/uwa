using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UniversalWebApi.Models;
using UniversalWebApi.Services.Db;

namespace UniversalWebApi.Areas.Manager.Controllers
{
    public class PayloadController : Controller
    {
        // GET: Manager/Payload
        public ActionResult Index()
        {
            List<UWA_PAYLOAD> objList = new List<UWA_PAYLOAD>();
            EQResultTable_v1 dbObj = Services.Db.UWA.GetAllPayload();
            if (dbObj.Result.SUCCESS && dbObj.Result.ROWS > 0)
            {
                objList = SevEF_v1.ConvertToList<UWA_PAYLOAD>(dbObj.Table);
            }
            return View(objList);
        }
        public ActionResult Create(string id)
        {
            ViewBag.ButtonType = "Create";

            var obj = new UWA_PAYLOAD();
            obj.PAYLOAD_TABLE = Create_ModuleName();
            if (!string.IsNullOrWhiteSpace(id))
            {
                EQResultTable_v1 dbObj = Services.Db.UWA.GetByIdPayload(id);
                if (dbObj.Result.SUCCESS && dbObj.Result.ROWS > 0)
                {
                    obj = SevEF_v1.ConvertToList<UWA_PAYLOAD>(dbObj.Table).First();
                    ViewBag.ButtonType = "Update";
                }
            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult Create(UWA_PAYLOAD obj)
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
                EQResult_v1 dbObj = UWA.UpdatePayload(obj, isNew);
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
            obj.PAYLOAD_TABLE = Create_ModuleName();
            return View(obj);
        }

        public ActionResult Delete(string id)
        {
            var obj = new UWA_BRANCH();
            if (!string.IsNullOrWhiteSpace(id))
            {
                EQResult_v1 dbObj = UWA.DeletePayload(id);
            }
            return RedirectToAction(nameof(Index));
        }
        private string Create_ModuleName()
        {
            EQResultTable_v1 dbObj = Services.Db.UWA.GetAllPayload();
            return "dwa" + (dbObj.Result.ROWS + 1);
        }
    }
}