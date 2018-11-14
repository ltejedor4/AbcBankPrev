using System;
using System.Collections.Generic;
using System.Text;

namespace Consumer
{
    public interface IConsumer
    {
        void Listen(Action<string> message);
    }
}
