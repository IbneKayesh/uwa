using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversalWebApi.Models;

namespace UniversalWebApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        public ActionResult Test()
        {
            List<EVNT_LIVE_MASTER> objList = new List<EVNT_LIVE_MASTER>();
            string payload = @"Data Source=KYSAEH\SQLEXPRESS;Initial Catalog=wsDB;User ID=sa;Password=123;";
            string sql = @"SELECT * FROM RFQ_MASTER RFD WHERE RFD.RFQ_ID='c33dc468-9e03-4783-aa4d-bdca71e061f4'";
            EQResultTable_v1 dbObj = Execute_v1.ExecuteQuery(payload, sql);
            objList = SevEF_v1.ConvertToList<EVNT_LIVE_MASTER>(dbObj.Table);


            //var m1 = objList.GroupBy(x => x.RFQ_ID);

            //EVNT_LIV_M objM = new EVNT_LIV_M();
            //objM.RFQ_ID = m1.First().First().RFQ_ID;
            //objM.RFQ_NO = m1.First().First().RFQ_NO;
            //objM.RFQ_NAME = m1.First().First().RFQ_NAME;

            //List<EVNT_LIV_ITM> evnt_liv_itm = new List<EVNT_LIV_ITM>();

            //var itm = objList.GroupBy(x => x.ITEM_ID);

            //foreach (var sitem in itm)
            //{
            //    evnt_liv_itm.Add(new EVNT_LIV_ITM
            //    {
            //        RFQ_ID = sitem.First().RFQ_ID,
            //        ITEM_ID = sitem.First().ITEM_ID,
            //        ITEM_NAME = sitem.First().ITEM_NAME,
            //        ITEM_DESC = sitem.First().ITEM_DESC,
            //        ITEM_QTY = sitem.First().ITEM_QTY,
            //        ITEM_RATE = sitem.First().ITEM_RATE,
            //        ITEM_AMT = sitem.First().ITEM_AMT,
            //        ITEM_FILE = sitem.First().ITEM_FILE
            //    });
            //}
            //objM.EVNT_LIV_ITM = evnt_liv_itm;



            //foreach (var _evnt_liv_itm in evnt_liv_itm)
            //{
            //    List<EVNT_LIV_RSP> objRsp = new List<EVNT_LIV_RSP>();
            //    var _data = objList.Where(x => x.ITEM_ID == _evnt_liv_itm.ITEM_ID).GroupBy(x => x.VENDOR_ID);
            //    foreach (var data in _data)
            //    {
            //        objRsp.Add(new EVNT_LIV_RSP
            //        {
            //            RFQ_ID = data.First().RFQ_ID,
            //            RESP_ID = data.First().RESP_ID,
            //            VENDOR_ID = data.First().VENDOR_ID,
            //            ORGANIZATION_NAME = data.First().ORGANIZATION_NAME,
            //            RATINGS_VALUE = data.First().RATINGS_VALUE,
            //            INVITE_DATE = data.First().INVITE_DATE,
            //            RESP_STATUS = data.First().RESP_STATUS,
            //            RESP_DATE = data.First().RESP_DATE
            //        });
            //    }
            //    _evnt_liv_itm.EVNT_LIV_RSP = objRsp;
            //}

            //foreach (var rfq_item in objM.EVNT_LIV_ITM)
            //{
            //    foreach (var log_item in rfq_item.EVNT_LIV_RSP)
            //    {
            //        var log = objList.Where(x => x.VENDOR_ID == log_item.VENDOR_ID && x.ITEM_ID == rfq_item.ITEM_ID);

            //        List<EVNT_LIV_LOG> logObj = new List<EVNT_LIV_LOG>();

            //        foreach (var _li in log)
            //        {
            //            logObj.Add(new EVNT_LIV_LOG
            //            {
            //                RESP_ID = _li.RESP_ID,
            //                ITEM_ID = _li.ITEM_ID,
            //                NEG_ID = _li.NEG_ID,
            //                ITEM_ID_REF = _li.ITEM_ID_REF,
            //                PROP_RATE = _li.PROP_RATE,
            //                PROP_AMT = _li.PROP_AMT,
            //                RESP_QTY = _li.RESP_QTY,
            //                RESP_RATE = _li.RESP_RATE,
            //                RESP_AMT = _li.RESP_AMT,
            //                RESP_NOTE = _li.RESP_NOTE,
            //                IS_ACTIVE = _li.IS_ACTIVE,
            //            });
            //        }
            //        log_item.EVNT_LIV_LOG = logObj;
            //    }
            //}



            EVNT_LIV_M objM = new EVNT_LIV_M();
            var m1 = objList.GroupBy(x => x.RFQ_ID).FirstOrDefault();
            if (m1 != null)
            {
                objM.RFQ_ID = m1.First().RFQ_ID;
                objM.RFQ_NO = m1.First().RFQ_NO;
                objM.RFQ_NAME = m1.First().RFQ_NAME;
            }

            var evnt_liv_itm = objList.GroupBy(x => x.ITEM_ID)
                                      .Select(sitem => new EVNT_LIV_ITM
                                      {
                                          RFQ_ID = sitem.First().RFQ_ID,
                                          ITEM_ID = sitem.First().ITEM_ID,
                                          ITEM_NAME = sitem.First().ITEM_NAME,
                                          ITEM_DESC = sitem.First().ITEM_DESC,
                                          ITEM_QTY = sitem.First().ITEM_QTY,
                                          ITEM_RATE = sitem.First().ITEM_RATE,
                                          ITEM_AMT = sitem.First().ITEM_AMT,
                                          ITEM_FILE = sitem.First().ITEM_FILE,
                                          EVNT_LIV_RSP = objList.Where(x => x.ITEM_ID == sitem.First().ITEM_ID && x.IS_ACTIVE != "0")
                                                               .GroupBy(x => x.VENDOR_ID)
                                                               .Select(data => new EVNT_LIV_RSP
                                                               {
                                                                   RFQ_ID = data.First().RFQ_ID,
                                                                   RESP_ID = data.First().RESP_ID,
                                                                   VENDOR_ID = data.First().VENDOR_ID,
                                                                   ORGANIZATION_NAME = data.First().ORGANIZATION_NAME,
                                                                   RATINGS_VALUE = data.First().RATINGS_VALUE,
                                                                   INVITE_DATE = data.First().INVITE_DATE,
                                                                   RESP_STATUS = data.First().RESP_STATUS,
                                                                   RESP_DATE = data.First().RESP_DATE,

                                                                   RESP_QTY = data.First().RESP_QTY,
                                                                   RESP_RATE = data.First().RESP_RATE,
                                                                   RESP_AMT = data.First().RESP_AMT,


                                                                   EVNT_LIV_LOG = objList.Where(x => x.VENDOR_ID == data.First().VENDOR_ID && x.ITEM_ID == sitem.First().ITEM_ID)
                                                                                        .Select(_li => new EVNT_LIV_LOG
                                                                                        {
                                                                                            RESP_ID = _li.RESP_ID,
                                                                                            ITEM_ID = _li.ITEM_ID,
                                                                                            NEG_ID = _li.NEG_ID,
                                                                                            ITEM_ID_REF = _li.ITEM_ID_REF,
                                                                                            PROP_QTY = _li.PROP_QTY,
                                                                                            PROP_RATE = _li.PROP_RATE,
                                                                                            PROP_AMT = _li.PROP_AMT,
                                                                                            RESP_QTY = _li.RESP_QTY,
                                                                                            RESP_RATE = _li.RESP_RATE,
                                                                                            RESP_AMT = _li.RESP_AMT,
                                                                                            RESP_NOTE = _li.RESP_NOTE,
                                                                                            IS_ACTIVE = _li.IS_ACTIVE,
                                                                                        }).ToList()
                                                               }).ToList()
                                      }).ToList();

            objM.EVNT_LIV_ITM = evnt_liv_itm;
            return View(objM);
        }
    }
}
