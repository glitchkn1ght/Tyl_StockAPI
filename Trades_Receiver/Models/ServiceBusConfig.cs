using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trades_Receiver.Models
{
    public class ServiceBusConfig
    {
        public string TopicName { get; set; }

        public string ConnectionString { get; set; }
    }
}
