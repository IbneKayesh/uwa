using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversalWebApi.Controllers
{
    public class EVNT_LIV_M
    {
        public string RFQ_ID { get; set; }
        public string RFQ_NO { get; set; }
        public string RFQ_NAME { get; set; }

        public List<EVNT_LIV_ITM> EVNT_LIV_ITM { get; set; }
    }
}