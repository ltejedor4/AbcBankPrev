using Microsoft.AspNetCore.Mvc;
using Pago.API.Extensions;
using Pago.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pago.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PagoController : ControllerBase
    {
        /// <summary>
        /// Check balance
        /// </summary>
        /// <param name="invoice">number that identified a invoice</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{invoice}")]
        public async Task<ActionResult> ValueToPay(string invoice)
        {            
            var result = await KafkaPubSub.ValuetoPay($"{invoice}#consultar#0");
            return Ok(result);
        }

        /// <summary>
        /// Pay a invoice
        /// </summary>
        /// <param name="factura">object with essential factua data</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> PayInvoice(FacturaResponse factura)
        {            
            var result = await KafkaPubSub.PayInvoice($"{factura.Invoice}#pagar#{factura.Amount}");            
            return Ok(result);

        }

        /// <summary>
        /// Reverse a pay already done
        /// </summary>
        /// <param name="factura">object with essential factua data</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> Compensate(FacturaResponse factura)
        {            
            var result = await KafkaPubSub.Compensate($"{factura.Invoice}#compensar#{factura.Amount}");
            return Ok(result);
        }

    }
}