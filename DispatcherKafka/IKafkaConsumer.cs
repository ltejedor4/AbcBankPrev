using System;
using System.Collections.Generic;
using System.Text;

namespace DispatcherKafka
{
    public interface IKafkaConsumer
    {
        void Listen();
        
        void Dispatching(string invoice, string accion, string valor);
    }
}
