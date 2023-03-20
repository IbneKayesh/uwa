using UniversalWebApi.Models;

namespace UniversalWebApi.ModelsV2
{
    public class EQResult_v02
    {
        public bool SUCCESS { get; set; }
        public string MESSAGES { get; set; }
        public int TABLES { get; set; }
        public EQResult_v02()
        {
            SUCCESS = true;
            MESSAGES = AppKeys_v1.SUCCESS_MESSAGES;
            TABLES = 0;
        }
    }
}