using System;

namespace DispatcherKafka
{
    class Program
    {
        static void Main(string[] args)
        {
            var kafkaconsumer = new KafkaConsumerImpl();
            //kafkaconsumer.Listen(Console.WriteLine);

            while (true)
            {
                kafkaconsumer.Dispatching("4567", "consultar");
                //kafkaconsumer.Dispatching("1234847291", "pagar");
                //kafkaconsumer.Dispatching("1234847291", "compensar");
            }
        }
    }
}
