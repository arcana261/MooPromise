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
            //Promise.DefaultFactory = PromiseBackend.MooThreadPool;

            Promise.SetDefaultFactory(1, 1);

            var exit = new ManualResetEventSlim(false);
            int ctr = 0;

            IntervalHandle handle = null;
            handle = Promise.Factory.SetInterval(() =>
            {
                var x = ctr++;

                Console.WriteLine("hello, from timeout!: " + x);

                return Promise.Factory.SetTimeout(2000, () => x);
            }, 1000).Then(x =>
            {
                if (x == 5)
                {
                    throw new ArgumentException();
                }

                Console.WriteLine("---> 1: " + x);
            }).Then(() =>
            {
                Console.WriteLine("---> 2");
            }).Catch(err =>
            {
                Console.WriteLine(err);
                handle.Cancel();
            });

            Promise.Factory.StartNew(() =>
            {
                Console.WriteLine("hello task!");
            });

            exit.Wait();

            return;
        }
    }
}
