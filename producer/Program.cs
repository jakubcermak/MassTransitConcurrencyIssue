using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace producer
{
    using common;
    using MassTransit;

    class Program
    {
        static void Main(string[] args)
        {
            var bus = MassTransit.Bus.Factory.CreateUsingRabbitMq(c =>
            {
                c.Host("localhost", "Test");
            });

            while (true)
            {
                bus.Publish(new FoxMessage());
            }
        }
    }
}
