using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace UniversalWebApi.Models
{
    //Author: Md. Ibne Kayesh
    //Framework: ASP.NET MVC
    //Date: 15-Nov-2022
    //Version: 0.0.1
    //Last Update: 15-Nov-2022
    //Licence: Free to use only
    public class EQResultTable_v1
    {
        public DataTable Table = new DataTable();
        public EQResult_v1 Result = new EQResult_v1();
    }
}