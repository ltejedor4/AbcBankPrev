using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Dispatching.API.Extensions;
using Dispatching.API.Model;
using Dispatching.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dispatching.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DispatcherController : ControllerBase
    {

        [HttpGet]
        [Route("{invoice}/{accion}")]
        public ActionResult ConsultaPago(string invoice, string accion)
        {
            string codConvenio = invoice.Substring(0, 4);
            var response = RestClient.GetObject<Convenio>("http://localhost:51350/api/v1/", $"convenio/{codConvenio}/{accion}").Result;

            string json = "";
            if (response.IsSuccess)
            {
                var objConvenio = (Convenio)response.Result;
                var tipoServicio = (TipoServicio)Enum.Parse(typeof(TipoServicio), objConvenio.CasoUso);

                var data = new TransformResult();
                var trans = new Transformation();
                if (tipoServicio == TipoServicio.REST)
                {
                    if (objConvenio.Metodo == MethodHttp.GET.ToString())
                    {
                        data = RestClient.ConsumeRest($"{objConvenio.BaseUrl}/{invoice}").Result;
                        json = trans.Execute(data, "responseFactura.json");
                    }
                    else if (objConvenio.Metodo == MethodHttp.POST.ToString())
                    {
                        data = RestClient.ConsumeRest($"{objConvenio.BaseUrl}", invoice, MethodHttp.POST).Result;
                        json = trans.Execute(data, "responseResult.json");
                    }
                    else if (objConvenio.Metodo == MethodHttp.DELETE.ToString())
                    {
                        data = RestClient.ConsumeRest($"{objConvenio.BaseUrl}", invoice, MethodHttp.DELETE).Result;
                        json = trans.Execute(data, "responseResult.json");
                    }
                }
                else if (tipoServicio == TipoServicio.SOAP)
                {
                    if (objConvenio.Metodo == MethodHttp.GET.ToString())
                    {
                        var peticion = trans.CreateRequest("requestFactura.xml", invoice);
                        data = SoapClient.ConsumeSoap(objConvenio.BaseUrl, accion, peticion).Result;
                        if (data.StatusCode != System.Net.HttpStatusCode.OK)
                            json = CallSOAP(data.Result, "responseError.json");
                        else
                            json = CallSOAP(data.Result, "responseFactura.json");
                    }
                    else if (objConvenio.Metodo == MethodHttp.POST.ToString())
                    {
                        var peticion = trans.CreateRequest("requestPayment.xml", invoice);
                        data = SoapClient.ConsumeSoap(objConvenio.BaseUrl, accion, peticion).Result;
                        if (data.StatusCode != System.Net.HttpStatusCode.OK)
                            json = CallSOAP(data.Result, "responseError.json");
                        else
                            json = CallSOAP(data.Result, "responseFactura.json");
                    }
                }

            }
            return Ok(json);

            /*
            //Dictionary<string, string> parametros,
            var tipo = (TipoServicio)tipoServicio;
            if (tipo == TipoServicio.REST)
            {
                //var pago = await RestClient.GetObject<string>(url, "");
                List<string> lista = new List<string>();
                lista.Add("fasdfa");
                lista.Add("ffas");
                return Ok(lista);
            }
            else if (tipo == TipoServicio.SOAP)
            {
                SoapClient soap = new SoapClient(url.Replace("http:/", "http://"), "Add", "http://tempuri.org/Add");
                soap.Params.Add("intA", "5");
                soap.Params.Add("intB", "6");
                soap.Params.Add("idFactura", "");
                soap.Invoke();

                var result = soap.ResultString;
                return Ok(result);
            }
            else
                return NotFound(new { Message = $"The kind of service with id {tipoServicio} not found." });*/
        }



        private static string CallSOAP(string result, string template)
        {
            var data = new TransformResult();
            var trans = new Transformation();
            var xm = XElement.Parse(result);
            var simpleXml = trans.RemoveAllNamespacesXml(xm);

            simpleXml = simpleXml.Replace("amp;", "").Replace("&lt;", "<").Replace("&gt;", ">");

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(simpleXml);

            data.MediaType = "application/json";
            data.Result = JsonConvert.SerializeXmlNode(xmlDoc);

            return trans.Execute(data, template);
        }
    }
}