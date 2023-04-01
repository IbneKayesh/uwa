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
    }
}