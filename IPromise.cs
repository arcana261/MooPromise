using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MooPromise
{
    public interface IPromiseBase
    {
        void Start();
        bool Cancel();
        Exception Error { get; }
        AsyncState State { get; }
        WaitHandle AsyncWaitHandle { get; }
    }

    public interface IPromise : IPromiseBase
    {
        IPromise Then(Action action);
        IPromise<T> Then<T>(Func<T> action);
        IPromise Then(Func<IPromise> action);
        IPromise<T> Then<T>(Func<IPromise<T>> action);
        IPromise Catch(Action<Exception> action);
        IPromise Catch(Action action);
        IPromise Finally(Action action);
        IPromise Finally(Action<Exception> action);
        IPromise Immediately { get; }
        IPromise Priority(PromisePriority priority);
        void Join();
    }

    public interface IPromise<T> : IPromiseBase
    {
        IPromise Then(Action<T> action);
        IPromise<F> Then<F>(Func<T, F> action);
        IPromise Then(Func<T, IPromise> action);
        IPromise<F> Then<F>(Func<T, IPromise<F>> action);
        IPromise Then(Action action);
        IPromise<F> Then<F>(Func<F> action);
        IPromise Then(Func<IPromise> action);
        IPromise<F> Then<F>(Func<IPromise<F>> action);
        IPromise<T> Catch(Action<Exception> action);
        IPromise<T> Catch(Action action);
        IPromise<T> Finally(Action action);
        IPromise<T> Finally(Action<Exception> action);
        IPromise<F> Cast<F>();
        IPromise Cast();
        IPromise<T> Immediately { get; }
        IPromise<T> Priority(PromisePriority priority);
        T Result { get; }
        T Join();
    }
}
