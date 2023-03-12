namespace UniversalWebApi.Models
{
    public class SF_BASE_LINK
    {
        /// <summary>
        /// Link ID/ File Name
        /// </summary>
        public string LINK_ID { get; set; }

        /// <summary>
        /// Database Connection String
        /// </summary>
        public string DB_CONNECTION { get; set; }

        /// <summary>
        /// Description of Connections
        /// </summary>
        public string DB_NAME_DESC { get; set; }

        /// <summary>
        /// Sql Folder Path
        /// </summary>
        public string FILE_LOCATION { get; set; }
    }
}