using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversalWebApi.Controllers
{
    public class EVNT_LIVE_MASTER
    {
        public string RFQ_ID { get; set; }
        public string RFQ_NO { get; set; }
        public string RFQ_NAME { get; set; }


        public string ITEM_ID { get; set; }
        public string ITEM_NAME { get; set; }
        public string ITEM_DESC { get; set; }
        public string ITEM_QTY { get; set; }
        public string ITEM_RATE { get; set; }
        public string ITEM_AMT { get; set; }
        public string ITEM_FILE { get; set; }


        
        public string RESP_ID { get; set; }
        public string VENDOR_ID { get; set; }
        public string ORGANIZATION_NAME { get; set; }
        public string RATINGS_VALUE { get; set; }
        public string INVITE_DATE { get; set; }
        public string RESP_STATUS { get; set; }
        public string RESP_DATE { get; set; }



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