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

            int x = 0;

            Promise.Factory.Control.For(0, i => i < 10, i => i + 1).Do(i => Console.WriteLine(i)).Then(() =>
            {
                Console.WriteLine("finished!");
            });

            exit.Wait();

            return;
        }
    }
}
