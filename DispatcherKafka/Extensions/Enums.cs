using System;
using System.Collections.Generic;
using System.Text;

namespace DispatcherKafka.Extensions
{
    public class Enums
    {
        public enum TipoServicio
        {
            REST = 1,
            SOAP = 2
        }

        public enum MethodHttp
        {
            GET = 1,
            POST = 2,
            PUT = 3,
            DELETE = 4
        }
    }
}
