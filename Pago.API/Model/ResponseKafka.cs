using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Pago.API.Model
{
    public class ResponseKafka
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
        public HttpStatusCode EstadoPeticion { get; set; }
    }
}
