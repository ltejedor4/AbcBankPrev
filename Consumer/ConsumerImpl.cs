using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;

namespace Consumer
{
    public class ConsumerImpl : IConsumer
    {
        public void Listen(Action<string> message)
        {
            var config = new Dictionary<string, object>
            {
                {"group.id","booking_consumer" },
                {"bootstrap.servers", "localhost:9092" },
                { "enable.auto.commit", "false" }
            };

            using(var consumer=new Consumer)


            while (true)
            {
                consumer.Poll(100);
            }

        }
    }
}
