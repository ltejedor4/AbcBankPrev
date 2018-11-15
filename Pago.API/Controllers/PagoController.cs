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
            //Todo: producir envento para kafka
            var result = await KafkaPubSub.ValuetoPay($"{invoice}#consultar");
            return Ok("todo cool");
        }

        /// <summary>
        /// Pay a invoice
        /// </summary>
        /// <param name="invoice">number that identified a invoice</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> PayInvoice(string invoice)
        {
            //Todo: producir envento para kafka
            //var result = await KafkaPubSub.PayInvoice($"{invoice}#consultar");
            
            return Ok("todo pagado");

        }

        /// <summary>
        /// Reverse a pay already done
        /// </summary>
        /// <param name="invoice">number that identified a invoice</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> Compensate(string invoice)
        {
            //Todo: producir envento para kafka
            //var result = await KafkaPubSub.Compensate($"{invoice}#consultar");
            return Ok("todo compensado");
        }

    }
}