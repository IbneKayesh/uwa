using System;
using System.Collections.Generic;
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
    public class EQResult_v1
    {
        public bool SUCCESS { get; set; }
        public string MESSAGES { get; set; }
        public int ROWS { get; set; }
        public EQResult_v1()
        {
            SUCCESS = true;
            MESSAGES = AppKeys_v1.SUCCESS_MESSAGES;
            ROWS = 0;
        }
    }
}