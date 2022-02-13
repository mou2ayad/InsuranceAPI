using System.Linq;
using System.Net;

namespace Insurance.Utilities.ErrorHandling
{
    public static class HttpStatusCodeExtension
    {
        private static readonly HttpStatusCode[] ConnectionIssuesStatusCodes =
        {
            HttpStatusCode.BadGateway, HttpStatusCode.GatewayTimeout, HttpStatusCode.ServiceUnavailable
        };

        public static bool IsConnectionIssueStatusCode(this HttpStatusCode code) => 
            ConnectionIssuesStatusCodes.Contains(code);
    }
}
