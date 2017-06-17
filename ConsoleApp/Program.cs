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

            var factory = Promise.CreateFactory(PromiseBackend.MooThreadPool);

            var result = factory.StartNew(() =>
            {
                //throw new ArgumentException();
                return 1;
            }).Then(x =>
            {
                x = x;

                return factory.StartNew(() =>
                {
                    return 5;
                }).Then(y =>
                {
                    //throw new ArgumentException();
                    return y * y;
                }).Catch(err =>
                {
                    Console.WriteLine(err.StackTrace);
                });
            }).Finally(() =>
            {
                Console.WriteLine();
            }).Then(y =>
            {
                Console.WriteLine(y);
            }).Then(() =>
            {
                return "salam";
            }).Immediately.Finally(() =>
            {
                //throw new IndexOutOfRangeException();
                Console.WriteLine();
            }).Catch(error =>
            {
                Console.WriteLine(error.StackTrace);
            }).Finally(() =>
            {
                //throw new InvalidCastException();
                Console.WriteLine();
            }).Then(x =>
            {
                Console.WriteLine(x);
                exit.Set();
            }).Catch(error =>
            {
                Console.WriteLine(error.StackTrace);
            });

            exit.Wait();

            return;
        }
    }
}
