using System.Data;
using UniversalWebApi.Models;

namespace UniversalWebApi.ModelsV2
{
    public class EQResultSet_v02
    {
        public string Name { get; set; }
        public DataTable Table { get; set; }
        public EQResult_v1 Result { get; set; }
    }
}