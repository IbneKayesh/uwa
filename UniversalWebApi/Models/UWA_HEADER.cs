namespace UniversalWebApi.Models
{
    public class UWA_HEADER
    {
        /// <summary>
        /// Database Id
        /// </summary>
        public string PAYLOAD_TOKEN { get; set; }

        /// <summary>
        /// Refresh Token
        /// BRANCH_ID.BRANCH_TOKEN
        /// </summary>
        public string BRANCH_TOKEN { get; set; }

        public bool IS_VALID { get; set; }
    }
}