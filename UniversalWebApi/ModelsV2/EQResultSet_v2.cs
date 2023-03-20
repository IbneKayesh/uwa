using System.Collections.Generic;

namespace UniversalWebApi.ModelsV2
{
    //Author: Md. Ibne Kayesh
    //Framework: ASP.NET MVC
    //Date: 20-Mar-2023
    //Version: 0.0.1
    //Last Update: 20-Mar-2023
    //Licence: Free to use only
    public class EQResultSet_v2
    {
        public List<EQResultSet_v02> DataSet { get; set; }
        public EQResult_v02 DataResult { get; set; }
    }
}