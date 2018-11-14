using DispatcherKafka.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static DispatcherKafka.Extensions.Enums;

namespace DispatcherKafka.Services
{
    public class RestClient
    {
        public static async Task<TransformResult> ConsumeRest(string url)
        {
            var client = new HttpClient();
            var result = new TransformResult();
            try
            {
                var service = await client.GetAsync(url);

                if (service.IsSuccessStatusCode)
                {
                    result.Result = await service.Content.ReadAsStringAsync();
                    result.MediaType = service.Content.Headers.ContentType.MediaType;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in Consume Rest: " + ex.Message);
            }
            return result;
        }

        public static async Task<TransformResult> ConsumeRest(string url, string json, MethodHttp method)
        {
            var client = new HttpClient();
            var result = new TransformResult();

            var request = new HttpRequestMessage
            {
                Method = method == MethodHttp.POST ? HttpMethod.Post : HttpMethod.Delete,
                RequestUri = new Uri(url),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            try
            {
                var service = await client.SendAsync(request);

                if (service.IsSuccessStatusCode)
                {
                    result.Result = await service.Content.ReadAsStringAsync();
                    result.MediaType = service.Content.Headers.ContentType.MediaType;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in Consume Rest: " + ex.Message);
            }

            return result;
        }

        public static async Task<ResultApi> GetList<T>(string url, string controller)
        {
            try
            {
                ResultApi Respuesta = new ResultApi();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var response = await client.GetAsync(controller);
                    var result = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        return new ResultApi
                        {
                            IsSuccess = false,
                            Message = result
                        };
                    }

                    var list = JsonConvert.DeserializeObject<List<T>>(result);
                    return new ResultApi
                    {
                        IsSuccess = true,
                        Result = list
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResultApi
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public static async Task<ResultApi> GetObject<T>(string url, string controller)
        {
            try
            {

                ResultApi Respuesta = new ResultApi();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var response = await client.GetAsync(controller);
                    var result = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        return new ResultApi
                        {
                            IsSuccess = false,
                            Message = result
                        };
                    }

                    var Obj = JsonConvert.DeserializeObject<T>(result);
                    return new ResultApi
                    {
                        IsSuccess = true,
                        Result = Obj
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResultApi
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public static async Task<ResultApi> Post<T>(string url, string controller, object obj)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(obj);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    client.BaseAddress = new Uri(url);
                    var response = await client.PostAsync(controller, content);
                    var result = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        return new ResultApi
                        {
                            IsSuccess = false,
                            Message = result
                        };
                    }

                    var objeto = JsonConvert.DeserializeObject<T>(result);
                    return new ResultApi
                    {
                        IsSuccess = true,
                        Result = objeto
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResultApi
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

    }
}
