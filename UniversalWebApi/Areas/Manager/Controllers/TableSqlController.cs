using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        public ActionResult Create(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectToAction("Index", "Payload", new { Area = "Manager" });
            }
            var obj = new TableSqlModel();
            obj.TABLE_NAME = id;
            obj.ID = null;
            return View(obj);
        }
        [HttpPost]
        public ActionResult Create(TableSqlModel obj)
        {
            if (ModelState.IsValid)
            {
                EQResult_v1 dbObj_1 = UWA.CreateSql(obj);
                if (dbObj_1.SUCCESS && dbObj_1.ROWS > 0)
                {
                    return RedirectToAction(nameof(Index), new { id = obj.TABLE_NAME });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, dbObj_1.MESSAGES);
                }
            }
            return View(obj);
        }




        public ActionResult Delete(string id, string tabl)
        {
            var obj = new UWA_BRANCH();
            if (!string.IsNullOrWhiteSpace(id) && !string.IsNullOrWhiteSpace(tabl))
            {
                EQResult_v1 dbObj = UWA.DeleteTableSql(id, tabl);
            }
            return RedirectToAction(nameof(Index), new { id = tabl });
        }

        public ActionResult TextJson(string id, string res)
        {
            string json = "{\n";
            json += $"\"RESOURCE\" : \"{id.Split('_')[1]}.{res}\"\n";
            json += "\"METHOD\" : \"GET or POST\"\n";
            json += "\"PARAM_LIST\" :\t[";

            EQResultTable_v1 dbObj = Services.Db.UWA.GetDbTableSqlById(id, res);
            if (dbObj.Result.SUCCESS && dbObj.Result.ROWS > 0)
            {
                List<string> paramList = FindSqlParameters(dbObj.Table.Rows[0]["SQL"].ToString(), "@");
                int i = 0;
                foreach (string param in paramList)
                {
                    json += "\n\t\t{\n";
                    json += $"\t\t\"TEXT\" : \"{param}\"\n";
                    json += $"\t\t\"VALUE\" : \"Value{i}\"\n";
                    i++;
                    if (i < paramList.Count)
                    {
                        json += "\t\t},";
                    }
                }
                if(paramList.Count > 0)
                {
                    json += "\t\t}\n";
                    json += "\t\t\t\t]\n}";
                }
                else
                {
                    json += "]\n}";
                }
            }
            else
            {
                json += "]\n}";
            }
            ViewBag.TJ = json;
            return View();
        }
        public static List<string> FindSqlParameters(string sql, string separator)
        {
            List<string> parameters = new List<string>();
            Regex regex = new Regex($@"{separator}\w+");
            MatchCollection matches = regex.Matches(sql);
            foreach (Match match in matches)
            {
                parameters.Add(match.Value);
            }
            return parameters;
        }
    }
}