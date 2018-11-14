using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DispatcherKafka.Model
{
    public class TransformResult
    {
        public string Result { get; set; }
        public string MediaType { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
