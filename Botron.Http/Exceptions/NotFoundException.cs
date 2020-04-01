using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Botron.Http.Exceptions
{
    public class NotFoundException : HttpException
    {
        public NotFoundException(string message)
            : base(HttpStatusCode.NotFound, message)
        {
        }
    }
}
