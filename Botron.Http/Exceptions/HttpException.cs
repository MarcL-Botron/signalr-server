using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Reflection;
using Newtonsoft.Json;

namespace Botron.Http.Exceptions
{
    public class HttpException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public object this[string parameter]
        {
            get { return this.Data[parameter]; }
            set { Data[parameter] = value; }
        }

        public HttpException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public ExceptionResponse ToResponse()
        {
            return new ExceptionResponse()
            {
                ["message"] = Message
            };
        }

        public override string ToString()
        {
            return $"{GetType()}: {Message}\nData: {JsonConvert.SerializeObject(Data, Formatting.Indented)}\n{StackTrace}";
        }

        public static HttpException Create(HttpStatusCode statusCode, string message)
        {
            return new HttpException(statusCode, message);
        }
    }
}
