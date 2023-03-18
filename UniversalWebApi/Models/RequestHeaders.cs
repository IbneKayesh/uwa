using System.Linq;
using System.Net.Http.Headers;

namespace UniversalWebApi.Models
{
    public class RequestHeaders
    {
        public static UWA_HEADER FromHttpRequestHeaders(HttpRequestHeaders headers)
        {
            UWA_HEADER requestHeaders = new UWA_HEADER();
            requestHeaders.IS_VALID = true;

            if (headers.Contains("payload-index"))
            {
                requestHeaders.PAYLOAD_TOKEN = headers.GetValues("payload-index").First();
            }
            else
            {
                requestHeaders.PAYLOAD_TOKEN = AppKeys_v1.INVALID_PAYLOAD_TOKEN;
                requestHeaders.IS_VALID = false;
            }


            if (headers.Contains("branch-index"))
            {
                requestHeaders.BRANCH_TOKEN = headers.GetValues("branch-index").First();
            }
            else
            {
                requestHeaders.BRANCH_TOKEN = AppKeys_v1.INVALID_BRANCH_TOKEN;
                requestHeaders.IS_VALID = false;
            }

            return requestHeaders;
        }
    }
}