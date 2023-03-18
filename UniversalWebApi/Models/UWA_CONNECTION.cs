using System;

namespace UniversalWebApi.Models
{
    public class UWA_CONNECTION
    {
        /// <summary>
        /// Database Id
        /// </summary>
        public string PAYLOAD_ID { get; set; }
        /// <summary>
        /// User Id
        /// </summary>
        public string BRANCH_ID { get; set; }

        /// <summary>
        /// User token
        /// </summary>
        public string BRANCH_TOKEN { get; set; }

        /// <summary>
        /// Token create time
        /// </summary>
        public DateTime CREATE_TIME { get; set; }

        /// <summary>
        /// Token end time
        /// </summary>
        public DateTime END_TIME { get; set; }
    }
}