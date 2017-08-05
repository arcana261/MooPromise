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

            Promise.Factory.Async.Begin<int>(ctx =>
            {
                int i = 0;

                ctx.While(() => i < 5).Do(w =>
                {
                    Console.WriteLine(i++);

                    w.If(() => i > 2).Do(q =>
                    {
                        Console.WriteLine("--> " + i.ToString());
                        q.Return(55);
                    }).Else.Do(() => Console.WriteLine("<<<<<<<<>>>>>> " + i.ToString()));
                });

                ctx.Run(() => Console.WriteLine("done!"));

                ctx.Return(() => i);

                ctx.Run(() => Console.WriteLine("this should not run!"));

            }).Then(x =>
            {
                Console.WriteLine("finished: " + x.ToString());
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
