using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using DispatcherKafka.Extensions;
using DispatcherKafka.Model;
using DispatcherKafka.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using static DispatcherKafka.Extensions.Enums;

namespace DispatcherKafka
{
    public class KafkaConsumerImpl : IKafkaConsumer
    {
        private static string kafkaEndPoint = "127.0.0.1:9092";
        private static string kafkaTopic = "sendtopic";
        private static string kafkaRetTopic = "rettopic";
        public void Listen()
        {
            string message = string.Empty;
            var config = new Dictionary<string, object>
            {
                {"group.id","booking_consumer" },
                {"bootstrap.servers", kafkaEndPoint },
                { "enable.auto.commit", "false" }
            };


            using (var consumer = new Consumer<Null, string>(config, null, new StringDeserializer(Encoding.UTF8)))
            {
                consumer.Subscribe(kafkaTopic);
                consumer.OnMessage += (_, msg) => message = msg.Value;

                while (true)
                {
                    consumer.Poll(100);
                    if (!string.IsNullOrEmpty(message))
                    {
                        var param = message.Split("#");
                        Dispatching(param[0], param[1], param[2]);
                        message = string.Empty;
                    }
                }
            }
        }
        public void Dispatching(string invoice, string accion, string valor)
        {
            string codConvenio = invoice.Substring(0, 4);
            //var response = RestClient.GetObject<Convenio>("http://172.20.0.2/api/v1/", $"convenio/{codConvenio}/{accion}").Result;
            var response = RestClient.GetObject<Convenio>("http://localhost:5000/api/v1/", $"convenio/{codConvenio}/{accion}").Result;

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
                        json = trans.Execute(data, "responseFacturaJs.json");
                    }
                    else if (objConvenio.Metodo == MethodHttp.POST.ToString())
                    {
                        string[] param = { invoice, valor };
                        var peticion = trans.CreateRequest("requestPaymentJs.json", param);
                        data = RestClient.ConsumeRest($"{objConvenio.BaseUrl}/{invoice}", peticion, MethodHttp.POST).Result;
                        json = trans.Execute(data, "responseResultJs.json");
                    }
                    else if (objConvenio.Metodo == MethodHttp.DELETE.ToString())
                    {
                        string[] param = { invoice, valor };
                        var peticion = trans.CreateRequest("requestPaymentJs.json", param);
                        data = RestClient.ConsumeRest($"{objConvenio.BaseUrl}/{invoice}", peticion, MethodHttp.DELETE).Result;
                        json = trans.Execute(data, "responseResultJs.json");
                    }
                }
                else if (tipoServicio == TipoServicio.SOAP)
                {
                    if (objConvenio.Metodo == MethodHttp.GET.ToString())
                    {
                        var peticion = trans.CreateRequest("requestFactura.xml", invoice);
                        data = SoapClient.ConsumeSoap(objConvenio.BaseUrl, accion, peticion).Result;
                        if (data.StatusCode != System.Net.HttpStatusCode.OK)
                            json = CallSOAP(data.Result, "responseErrorxml.json");
                        else
                            json = CallSOAP(data.Result, "responseFacturaxml.json");
                    }
                    else if (objConvenio.Metodo == MethodHttp.POST.ToString())
                    {
                        string[] param = { invoice, valor };
                        var peticion = trans.CreateRequest("requestPayment.xml", param);
                        data = SoapClient.ConsumeSoap(objConvenio.BaseUrl, accion, peticion).Result;
                        if (data.StatusCode != System.Net.HttpStatusCode.OK)
                            json = CallSOAP(data.Result, "responseErrorxml.json");
                        else
                            json = CallSOAP(data.Result, "responseResultxml.json");
                    }
                }

                var producerConfigSend = new Dictionary<string, object> { { "bootstrap.servers", kafkaEndPoint } };
                using (var producer = new Producer<Null, string>(producerConfigSend, null, new StringSerializer(Encoding.UTF8)))
                {
                    var dr = producer.ProduceAsync(kafkaRetTopic, null, json).Result;
                    Console.WriteLine($"Message send to kafka: {json}");
                }
            }
        }

        private string CallSOAP(string result, string template)
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


