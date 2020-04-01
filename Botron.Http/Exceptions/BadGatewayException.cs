using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Botron.Http.Exceptions
{
    public class BadGatewayException : HttpException
    {
        public BadGatewayException(string message)
            : base(HttpStatusCode.BadGateway, message)
        {

        }
    }
}
