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
            ManualResetEventSlim exit = new ManualResetEventSlim(false);
            Promise.SetDefaultFactory(PromiseBackend.MooThreadPool);

            //Enumerable.Range(0, 10).Where

            exit.Wait();

            return;
        }
    }
}
