using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversalWebApi.Controllers
{
    public class EVNT_LIV_LOG
    {
        public string RESP_ID { get; set; }
        public string ITEM_ID { get; set; }
        public string NEG_ID { get; set; }
        public string ITEM_ID_REF { get; set; }
        public string PROP_QTY { get; set; }
        public string PROP_RATE { get; set; }
        public string PROP_AMT { get; set; }
        public string RESP_QTY { get; set; }
        public string RESP_RATE { get; set; }
        public string RESP_AMT { get; set; }
        public string RESP_NOTE { get; set; }
        public string IS_ACTIVE { get; set; }
    }
}