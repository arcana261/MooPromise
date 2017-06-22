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

            Promise.Enumerable(Promise.Factory.StartNew(() => (new int[] { 1, 2, 3 })))
                .Where(x =>
                {
                    if (x == 3)
                    {
                        throw new InvalidOperationException();
                    }

                    return x % 2 == 1;
                })
                .Catch(err =>
                {
                    Console.WriteLine(err);
                })
                .Select(x => x * 2)
                .Each(x =>
                {
                    Console.WriteLine(x);
                })
                ;

            ManualResetEventSlim exit = new ManualResetEventSlim(false);
            //Promise.SetDefaultFactory(PromiseBackend.MooThreadPool);

            //Promise.SetDefaultFactory(PromiseBackend.MooThreadPool);
            //var ctx = Promise.CreateSynchronizationContext();
            //Random _rnd = new Random();

            //ctx.Post(() =>
            //{
            //    return Promise.Factory.StartNew(() =>
            //    {
            //        Console.WriteLine("1 started");
            //        Thread.Sleep(1000 + _rnd.Next(1000));
            //        Console.WriteLine(1);
            //    });
            //});

            //ctx.Post(() =>
            //{
            //    return Promise.Factory.StartNew(() =>
            //    {
            //        Console.WriteLine("2 started");
            //        Thread.Sleep(1000 + _rnd.Next(1000));
            //        Console.WriteLine(2);
            //    });
            //});

            //ctx.Post(() =>
            //{
            //    return Promise.Factory.StartNew(() =>
            //    {
            //        Console.WriteLine("3 started");
            //        Thread.Sleep(1000 + _rnd.Next(1000));
            //        Console.WriteLine(3);
            //    });
            //});

            //ctx.Post(() =>
            //{
            //    return Promise.Factory.StartNew(() =>
            //    {
            //        Console.WriteLine("4 started");
            //        Thread.Sleep(1000 + _rnd.Next(1000));
            //        Console.WriteLine(4);
            //    });
            //});

            //ctx.Post(() =>
            //{
            //    return Promise.Factory.StartNew(() =>
            //    {
            //        Console.WriteLine("done!");
            //    });
            //});

            exit.Wait();

            return;
        }
    }
}
