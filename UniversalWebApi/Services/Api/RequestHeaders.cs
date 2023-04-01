using System.Linq;
using System.Net.Http.Headers;
using UniversalWebApi.Models;

namespace UniversalWebApi.Services.Api
{
    public class RequestHeaders
    {
        public static UWA_HEADER FromHttpRequestHeaders(HttpRequestHeaders headers)
        {
            UWA_HEADER requestHeaders = new UWA_HEADER();
            requestHeaders.IS_VALID = true;

            if (headers.Contains("payloadIndex"))
            {
                requestHeaders.PAYLOAD_TOKEN = headers.GetValues("payloadIndex").First();
            }
            else
            {
                requestHeaders.PAYLOAD_TOKEN = AppKeys_v1.INVALID_PAYLOAD_TOKEN;
                requestHeaders.IS_VALID = false;
            }


            if (headers.Contains("branchIndex"))
            {
                requestHeaders.BRANCH_TOKEN = headers.GetValues("branchIndex").First();
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