using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DispatcherKafka.Model
{
    public class ResultApi
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
        public HttpStatusCode EstadoPeticion { get; set; }
    }
}
