namespace UniversalWebApi.Models
{
    public class UWA_BRANCH
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
        /// User Descriptions
        /// </summary>
        public string BRANCH_DESC  { get; set; }

        /// <summary>
        /// 0: No Auth, 1: Single Auth, 2: Multiple Auth
        /// </summary>
        public int BRANCH_STATUS { get; set; }
    }
}