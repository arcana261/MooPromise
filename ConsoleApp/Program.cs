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
            int i = 0;

            Promise.Factory.Control.While(() => i < 5).Do(() =>
            {
                Console.WriteLine(i++);
            }).Then(() =>
            {
                Console.WriteLine("!done!");
            });

            //Promise.Factory.Async.Begin<int>(ctx =>
            //{
            //    ctx.Variables.Define<int>("i", 0);

            //    ctx.While(() => ctx.Variables.Get<int>("i") < 5).Do(() =>
            //    {
            //        Console.WriteLine(ctx.Variables.Get<int>("i"));
            //        ctx.Variables.Set("i", ctx.Variables.Get<int>("i") + 1);
            //    });

            //    ctx.Return(() => ctx.Variables.Get<int>("i"));

            //}).Then(res =>
            //{
            //    Console.WriteLine("! finished !");
            //    Console.WriteLine(res);
            //}).Catch(err =>
            //{
            //    Console.WriteLine(err);
            //});


            exit.Wait();

            return;
        }
    }
}
