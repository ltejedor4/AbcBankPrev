using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Newtonsoft.Json;
using Pago.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pago.API.Extensions
{
    public class KafkaPubSub
    {
        private static string kafkaEndPoint = "127.0.0.1:9092";
        private static string kafkaTopic = "sendtopic";
        private static string kafkaRetTopic = "rettopic";
        public static async Task<ResponseKafka> ValuetoPay(string invoiceRef)
        {
            KafkaProducer(invoiceRef);
            var message = KafkaConsumer();
            //var message = OptionalConsumer(invoiceRef);
            var response = new ResponseKafka();

            try
            {
                var serialize = JsonConvert.DeserializeObject<FacturaResponse>(message);
                response.EstadoPeticion = HttpStatusCode.OK;
                response.Result = serialize;
            }
            catch (Exception ex)
            {
                response.EstadoPeticion = HttpStatusCode.InternalServerError;
                response.Result = null;
                response.Message = ex.Message;
            }

            return await Task.Run(() => response);
        }

        public static async Task<ResponseKafka> PayInvoice(string invoiceRef)
        {            
            KafkaProducer(invoiceRef);
            var message = KafkaConsumer();
            //var message = OptionalConsumer(invoiceRef);
            var response = new ResponseKafka();

            try
            {
                var serialize = JsonConvert.DeserializeObject<PagoResponse>(message);
                response.EstadoPeticion = HttpStatusCode.OK;
                response.Result = serialize;
            }
            catch (Exception ex)
            {
                response.EstadoPeticion = HttpStatusCode.InternalServerError;
                response.Result = null;
                response.Message = ex.Message;
            }


            return await Task.Run(() => response);
        }

        public static async Task<ResponseKafka> Compensate(string invoiceRef)
        {            
            KafkaProducer(invoiceRef);
            var message = KafkaConsumer();
            //var message = OptionalConsumer(invoiceRef);

            var response = new ResponseKafka();

            try
            {
                var serialize = JsonConvert.DeserializeObject<PagoResponse>(message);
                response.EstadoPeticion = HttpStatusCode.OK;
                response.Result = serialize;
            }
            catch (Exception ex)
            {
                response.EstadoPeticion = HttpStatusCode.InternalServerError;
                response.Result = null;
                response.Message = ex.Message;
            }


            return await Task.Run(() => response);
        }


        private static void KafkaProducer(string message)
        {
            var producerConfigSend = new Dictionary<string, object> { { "bootstrap.servers", kafkaEndPoint } };
            using (var producer = new Producer<Null, string>(producerConfigSend, null, new StringSerializer(Encoding.UTF8)))
            {
                var dr = producer.ProduceAsync(kafkaTopic, null, message).Result;

            }
        }
        private static string KafkaConsumer()
        {
            var message = "";
            var conf = new Dictionary<string, object>
            {
                { "group.id", "test-consumer-group" },
                { "bootstrap.servers", kafkaEndPoint },
                { "auto.commit.interval.ms", 5000 },
                { "auto.offset.reset", "earliest" }
            };

            using (var consumer = new Consumer<Null, string>(conf, null, new StringDeserializer(Encoding.UTF8)))
            {
                consumer.OnMessage += (_, msg)
                  => message = msg.Value; 

                consumer.OnError += (_, error)
                  => message = error.ToString();

                consumer.OnConsumeError += (_, msg)
                  => message = msg.Error.ToString();

                consumer.Subscribe(kafkaRetTopic);

                while (true)
                {
                    //var x = "";
                    consumer.Poll(TimeSpan.FromMilliseconds(100));
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        //x = message;
                        return message;
                    }
                    /*else
                    {
                        return x;
                    }*/
                    message = "";
                }
            }
        }

        private static string OptionalConsumer(string message)
        {
            return "";
        }

    }
}
