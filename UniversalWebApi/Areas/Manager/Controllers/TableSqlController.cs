using System.Collections.Generic;
using System.Web.Mvc;
using UniversalWebApi.Areas.Manager.Models;
using UniversalWebApi.Models;
using UniversalWebApi.Services.Db;

namespace UniversalWebApi.Areas.Manager.Controllers
{
    public class TableSqlController : Controller
    {
        // GET: Manager/TableSql
        public ActionResult Index(string id)
        {
            List<TableSqlModel> objList = new List<TableSqlModel>();
            EQResultTable_v1 dbObj = Services.Db.UWA.GetDbTableSql(id);
            if (dbObj.Result.SUCCESS && dbObj.Result.ROWS > 0)
            {
                objList = SevEF_v1.ConvertToList<TableSqlModel>(dbObj.Table);
            }
            return View(objList);
        }
    }
}