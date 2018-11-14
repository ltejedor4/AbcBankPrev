using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Dispatching.API.Model
{
    public class TransformResult
    {
        public string Result { get; set; }
        public string MediaType { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
