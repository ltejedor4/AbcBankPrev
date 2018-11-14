using Dispatching.API.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace Dispatching.API.Services
{
    public class SoapClient
    {

        public static async Task<TransformResult> ConsumeSoap(string url, string soapAction, string request)
        {
            var result = new TransformResult();

            try
            {
                using (var client = new HttpClient())
                {                    
                    client.DefaultRequestHeaders.Add("SOAPAction", soapAction);

                    var content = new StringContent(request, Encoding.UTF8, "text/xml");
                    using (var response = await client.PostAsync(url, content))
                    {
                        
                        var soapResponse = await response.Content.ReadAsStringAsync();

                        result.StatusCode = response.StatusCode;
                        result.Result = soapResponse;
                        result.MediaType = "text/xml";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in Consume Soap: " + ex.Message);
            }

            return result;
        }


        public string Url { get; set; }
        public string MethodName { get; set; }   
        public string SOAPAction { get; set; }
        public Dictionary<string, string> Params = new Dictionary<string, string>();
        public XDocument ResultXML;
        public string ResultString;

        public SoapClient()
        {
        }

        public SoapClient(string url, string methodName, string soapAction)
        {
            Url = url;
            MethodName = methodName;
            SOAPAction = soapAction;
        }

        public void Invoke()
        {
            Invoke(true);
        }

        /// <summary>
        /// Invokes service
        /// </summary>
        /// <param name="encode">Added parameters will encode? (default: true)</param>
        public void Invoke(bool encode)
        {           
            string soapStr = "";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            req.Headers.Add("SOAPAction", SOAPAction);
            req.ContentType = "application/soap+xml;charset=\"utf-8\"";
            req.Accept = "application/soap+xml";
            req.Method = "POST";

            using (Stream stm = req.GetRequestStream())
            {
                string postValues = "";
                foreach (var param in Params)
                {
                    if (encode)
                        postValues += string.Format("<tem:{0}>{1}</tem:{0}>", HttpUtility.UrlEncode(param.Key), HttpUtility.UrlEncode(param.Value));
                    else
                        postValues += string.Format("<tem{0}>{1}</tem:{0}>", param.Key, param.Value);
                }
                
                soapStr = $"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:tem=\"http://tempuri.org/\">" +
                $"<soap:Body><tem:{MethodName}>{postValues}</tem:{MethodName}></soap:Body></soap:Envelope>";                
                using (StreamWriter stmw = new StreamWriter(stm))
                {
                    stmw.Write(soapStr);
                }
            }

            using (StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                string result = responseReader.ReadToEnd();
                ResultXML = XDocument.Parse(result);
                ResultString = result;
            }
        }

    }
}
