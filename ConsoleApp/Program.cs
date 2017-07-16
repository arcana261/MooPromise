using MooPromise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Promise.DefaultFactory = PromiseBackend.MooThreadPool;
            var exit = new ManualResetEventSlim(false);

            var ctx = Promise.CreateSynchronizationContext();

            ctx.Post(() =>
            {
                Console.WriteLine("1");
                Thread.Sleep(2000);

                return Promise.Factory.StartNew(() =>
                {
                    Console.WriteLine("1.5");

                    throw new ArgumentException("hi!");
                });
            }).Catch(err =>
            {
                Console.WriteLine("error: " + err.Message);
            });

            ctx.Post(() =>
            {
                Console.WriteLine("2");
            });

            exit.Wait();

            return;
        }
    }
}
