using DispatcherKafka.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherKafka.Services
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
    }
}
