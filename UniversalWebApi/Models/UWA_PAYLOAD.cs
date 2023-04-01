using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversalWebApi.Models
{
    public class UWA_PAYLOAD
    {
        /// <summary>
        /// Database Id
        /// </summary>
        public string PAYLOAD_ID { get; set; }

        /// <summary>
        /// Database connection string
        /// </summary>
        public string DB_CONNECTION { get; set; }

        /// <summary>
        /// Descriptions of database
        /// </summary>
        public string DB_DESC { get; set; }

        /// <summary>
        /// SQL file table
        /// </summary>
        public string PAYLOAD_TABLE { get; set; }

    }
}