using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pago.API.Model
{
    public class FacturaResponse
    {
        public string Invoice { get; set; }
        public double Amount { get; set; }
    }
}
