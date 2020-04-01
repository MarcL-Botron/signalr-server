using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Botron.Http.Exceptions
{
    public class BadRequestException : HttpException
    {
        public BadRequestException(string message)
            : base(HttpStatusCode.BadRequest, message)
        {

        }
    }
}
