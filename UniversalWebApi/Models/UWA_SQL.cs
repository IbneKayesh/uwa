namespace UniversalWebApi.Models
{
    public class UWA_SQL
    {
        /// <summary>
        /// SQL File Id or Number
        /// ZZZZZZ-YYYYYY
        /// FILE_NAME-SQL_ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// SQL Query
        /// select sysdate from dual
        /// </summary>
        public string SQL { get; set; }
    }
}