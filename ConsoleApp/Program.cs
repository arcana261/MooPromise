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

            var arr1 = new int[] { 1, 2, 3, 4, 5 };
            var arr2 = new int[] { 2, 4, 4 };

            arr1.Promesify().Intersect(arr2).Each(x => Console.WriteLine(x)).Then(() => Console.WriteLine("done!"));

            exit.Wait();

            return;
        }
    }
}
