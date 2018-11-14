using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pago.API
{
    public class PagoResponse
    {        
        public string Invoice { get; set; }
        public string Message { get; set; }        
    }
}
