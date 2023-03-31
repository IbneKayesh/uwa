using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversalWebApi.Controllers
{
    public class EVNT_LIV_ITM
    {
        public string RFQ_ID { get; set; }
        public string ITEM_ID { get; set; }
        public string ITEM_NAME { get; set; }
        public string ITEM_DESC { get; set; }
        public string ITEM_QTY { get; set; }
        public string ITEM_RATE { get; set; }
        public string ITEM_AMT { get; set; }
        public string ITEM_FILE { get; set; }

        public List<EVNT_LIV_RSP> EVNT_LIV_RSP { get; set; }
    }
}