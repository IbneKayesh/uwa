using System;
using System.Data;
using System.Web.Http;
using System.Web.Http.Description;
using UniversalWebApi.Models;

namespace UniversalWebApi.Areas.Home.Controllers
{
    public class LoginController : ApiController
    {        //Login authorization

        [HttpPost]
        //[HttpOptions]
        [Route("api/v1/Login")]
        [ResponseType(typeof(EQResultTable_v1))]
        public EQResultTable_v1 v1_Login()
        {
            EQResultTable_v1 retObj = new EQResultTable_v1();
            //find header information
            UWA_HEADER header = Services.Api.RequestHeaders.FromHttpRequestHeaders(this.Request.Headers);
            //validate header
            if (!header.IS_VALID)
            {
                retObj.Result = new EQResult_v1() { SUCCESS = false, MESSAGES = "Header token was not provided" };
                return retObj;
            }


            string branchName = header.BRANCH_TOKEN;
            string branchToken = Guid.NewGuid().ToString();

            //find branch/user information
            UWA_BRANCH Branch_Auth = Services.Db.ApiStore.getBranch(header);
            //validate branch information
            if (Branch_Auth.BRANCH_DESC == AppKeys_v1.INVALID_BRANCH_TOKEN)
            {
                retObj.Result = new EQResult_v1() { SUCCESS = false, MESSAGES = "Invalid payload or branch token" };
                return retObj;
            }

            //write branch session
            UWA_CONNECTION con = new UWA_CONNECTION();
            con.PAYLOAD_ID = header.PAYLOAD_TOKEN;
            con.BRANCH_ID = branchName;
            con.BRANCH_TOKEN = branchToken;
            con.CREATE_TIME = DateTime.Now;
            con.END_TIME = DateTime.Now.AddDays(1);

            if (Branch_Auth.BRANCH_STATUS == 1)
            {
                Services.Db.ApiStore.WriteConnection(con);
            }
            else if (Branch_Auth.BRANCH_STATUS == 2)
            {
                Services.Db.ApiStore.WriteConnection(con, false);
            }



            EQResultTable_v1 returnTable = new EQResultTable_v1();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("BRANCH");
            dataTable.Columns.Add("TOKEN");
            DataRow row = dataTable.NewRow();
            row["BRANCH"] = branchName;
            row["TOKEN"] = branchToken;
            dataTable.Rows.Add(row);

            returnTable.Table = dataTable;
            return returnTable;
        }
    }
}
