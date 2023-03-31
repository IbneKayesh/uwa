using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversalWebApi.Controllers
{
    public class EVNT_LIV_RSP
    {
        public string RFQ_ID { get; set; }
        public string RESP_ID { get; set; }
        public string VENDOR_ID { get; set; }
        public string ORGANIZATION_NAME { get; set; }
        public string RATINGS_VALUE { get; set; }
        public string INVITE_DATE { get; set; }
        public string RESP_STATUS { get; set; }
        public string RESP_DATE { get; set; }

        public string RESP_QTY { get; set; }
        public string RESP_RATE { get; set; }
        public string RESP_AMT { get; set; }

        public List<EVNT_LIV_LOG> EVNT_LIV_LOG { get; set; }
    }
}