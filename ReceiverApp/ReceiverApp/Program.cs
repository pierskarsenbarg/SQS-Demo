using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiverApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var queueHelper = new QueueReceiverHelper();
            queueHelper.DoWork();
        }
    }
}
