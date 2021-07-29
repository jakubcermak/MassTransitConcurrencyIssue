using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp16
{
    using System.Threading;
    using common;
    using GreenPipes;
    using MassTransit;
    using MassTransit.Contracts;

    class Program
    {
        static async Task Main(string[] args)
        {
            ThreadPool.SetMinThreads(50, 50);

            var bus = MassTransit.Bus.Factory.CreateUsingRabbitMq(c =>
            {
                c.Host("localhost", "Test");

                c.ReceiveEndpoint("fox-queue", cf =>
                {
                    cf.Instance(new MyConsumer());
                    //cf.Consumer(() => new MyConsumer());
                    //cf.Consumer<MyConsumer>();

                    cf.PrefetchCount = 40;
                    cf.UseConcurrencyLimit(5);
                });
            });

            await bus.StartAsync();

            Console.WriteLine("bla");
            Console.ReadLine();

        }

        public static volatile int concurrency = 0;
    }

    class MyConsumer : IConsumer<FoxMessage>
    {
        static int counter = 0;

        int id;

        public MyConsumer()
        {
            id = Interlocked.Increment(ref counter);
        }

        public Task Consume(ConsumeContext<FoxMessage> context)
        {
            Interlocked.Increment(ref Program.concurrency);

            Console.WriteLine($"[{id}] starting fox {context.Message.Id} (c {Program.concurrency})");

            //Thread.Sleep(10000);

            Task.Run(() =>
            {
                Thread.Sleep(10000);

            }).Wait();

            Console.WriteLine($"[{id}] finish fox {context.Message.Id}  (c {Program.concurrency})");

            Interlocked.Decrement(ref Program.concurrency);


            return Task.CompletedTask;
        }
    }
}
