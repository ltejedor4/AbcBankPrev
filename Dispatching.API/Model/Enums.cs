using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dispatching.API.Model
{
    public enum TipoServicio
    {
        REST=1,
        SOAP=2
    }

    public enum MethodHttp
    {
        GET=1,
        POST=2,
        PUT=3,
        DELETE=4
    }
}
