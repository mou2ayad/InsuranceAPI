using System;

namespace Insurance.Utilities.ErrorHandling
{
    public class ClientException :Exception
    {
        public ClientException(string message) :base(message)
        {
            
        }
    }
}
