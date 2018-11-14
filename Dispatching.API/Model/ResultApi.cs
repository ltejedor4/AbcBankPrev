using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Dispatching.API.Model
{
    public class ResultApi
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
        public HttpStatusCode EstadoPeticion { get; set; }
    }
}
