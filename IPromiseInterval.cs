using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise
{
    public interface IPromiseIntervalBase
    {
        void Cancel();
        bool IsCanceled { get; }
        int Timeout { get; }
        PromiseFactory Factory { get; }
        PromisePriority Priority { get; }
    }

    public interface IPromiseInterval : IPromiseIntervalBase
    {
        IPromiseInterval Then(Action handler);
        IPromiseInterval Then(Func<IPromise> handler);
        IPromiseInterval<T> Then<T>(Func<IPromise<T>> handler);
        IPromiseInterval<T> Then<T>(Func<T> handler);
        IPromiseInterval Catch(Action<Exception> handler);
        IPromiseInterval Catch(Action handler);
        IPromiseInterval Finally(Action<Exception> handler);
        IPromiseInterval Finally(Action handler);
    }

    public interface IPromiseInterval<T> : IPromiseIntervalBase
    {
        IPromiseInterval Then(Action<T> handler);
        IPromiseInterval Then(Func<T, IPromise> handler);
        IPromiseInterval<E> Then<E>(Func<T, E> handler);
        IPromiseInterval<E> Then<E>(Func<T, IPromise<E>> handler);
        IPromiseInterval<T> Catch(Action<Exception> handler);
        IPromiseInterval<T> Catch(Action handler);
        IPromiseInterval<T> Finally(Action<Exception> handler);
        IPromiseInterval<T> Finally(Action handler);
        IPromiseInterval Then(Action handler);
        IPromiseInterval Then(Func<IPromise> handler);
        IPromiseInterval<E> Then<E>(Func<E> handler);
        IPromiseInterval<E> Then<E>(Func<IPromise<E>> handler);
        IPromiseInterval Cast();
        IPromiseInterval<E> Cast<E>();
    }
}
