namespace UniversalWebApi.Models
{
    public class SQL_PLIST_v2
    {
        public string TABLE { get; set; }
        public string SQL { get; set; }
        public object[] iPARAMS { get; set; }
        public object[] oPARAMS { get; set; }
    }
}