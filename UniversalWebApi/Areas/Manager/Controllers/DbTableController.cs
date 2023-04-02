using System.Collections.Generic;
using System.Web.Mvc;
using UniversalWebApi.Areas.Manager.Models;
using UniversalWebApi.Models;
using UniversalWebApi.Services.Db;

namespace UniversalWebApi.Areas.Manager.Controllers
{
    public class DbTableController : Controller
    {
        // GET: Manager/DbTable
        public ActionResult Index(string id)
        {
            List<DbTableModel> objList = new List<DbTableModel>();
            EQResultTable_v1 dbObj = Services.Db.UWA.GetDbTable(id);
            if (dbObj.Result.SUCCESS && dbObj.Result.ROWS > 0)
            {
                objList = SevEF_v1.ConvertToList<DbTableModel>(dbObj.Table);
            }
            return View(objList);
        }

        public ActionResult Create(string id)
        {

            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectToAction("Index", "Payload",new { Area = "Manager" });
            }
            var obj = new DbTableModel();
            obj.PAYLOAD_ID = id;
            return View(obj);
        }
        [HttpPost]
        public ActionResult Create(DbTableModel obj)
        {
            if (ModelState.IsValid)
            {
                //EQResultTable_v1 dbObj = Services.Db.UWA.GetByIdPayload(obj.PAYLOAD_ID);
                //if (dbObj.Result.SUCCESS && dbObj.Result.ROWS > 0)
                //{
                //    EQResult_v1 dbObj_1 = UWA.CreateModuleTable(dbObj.Table.Rows[0]["PAYLOAD_TABLE"].ToString(), obj.TABLE_NAME);
                //    if (dbObj_1.SUCCESS)// && dbObj_1.ROWS > 0
                //    {
                //        return RedirectToAction(nameof(Index), new { id= obj.PAYLOAD_ID});
                //    }
                //    else
                //    {
                //        ModelState.AddModelError(string.Empty, dbObj_1.MESSAGES);
                //    }
                //}
                //else
                //{
                //    ModelState.AddModelError(string.Empty, dbObj.Result.MESSAGES);
                //}


                EQResult_v1 dbObj_1 = UWA.CreateModuleTable(obj.PAYLOAD_ID, obj.TABLE_NAME);
                if (dbObj_1.SUCCESS)// && dbObj_1.ROWS > 0
                {
                    return RedirectToAction(nameof(Index), new { id = obj.PAYLOAD_ID });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, dbObj_1.MESSAGES);
                }
            }
            return View(obj);
        }
    }
}