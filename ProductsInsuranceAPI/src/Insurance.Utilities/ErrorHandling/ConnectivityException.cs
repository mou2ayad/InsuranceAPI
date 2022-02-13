using System;
using System.Net;

namespace Insurance.Utilities.ErrorHandling
{
    public class ConnectivityException :Exception
    {
       
        public HttpStatusCode StatusCode {get; }

        public ConnectivityException(HttpStatusCode statusCode) :base($"Connectivity error, status code: {(int)statusCode}")
        {
            StatusCode = statusCode;
        }

        public ConnectivityException(string message)
            : base(message)
        {
        }

        public ConnectivityException(string message, Exception exception)
            : base(message)
        {
        }
    }
}