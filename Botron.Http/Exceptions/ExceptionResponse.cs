using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Botron.Http.Exceptions
{
    public class ExceptionResponse
    {
        public string this[string key]
        {
            get { return Errors[key]; }
            set { Errors[key] = value; }
        }

        public Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();

        int Status { get; set; } = (int)HttpStatusCode.BadRequest;
    }
}
