using System;

namespace DispatcherKafka
{
    class Program
    {
        static void Main(string[] args)
        {
            var kafkaconsumer = new KafkaConsumerImpl();
            kafkaconsumer.Listen();
            

            //SOAP
            //kafkaconsumer.Dispatching("1234847291", "consultar","0");
            //kafkaconsumer.Dispatching("1234847291", "pagar","464564");
            //kafkaconsumer.Dispatching("1234847291", "compensar","546545");

            //REST
            //kafkaconsumer.Dispatching("2567", "consultar","0");
            //kafkaconsumer.Dispatching("2567", "pagar", "45644");
            //kafkaconsumer.Dispatching("2567", "compensar", "87878");
        }
    }
}
