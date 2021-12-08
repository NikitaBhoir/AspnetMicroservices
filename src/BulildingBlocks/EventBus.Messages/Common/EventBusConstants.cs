using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Common
{
   public static class EventBusConstants
    {
        public const string BasketCheckoutQueue = "basketcheckout-queue";//provide queue name,this queue name will be show in rabbitmq portal// thos is constanst value so it should not change
    }
}
