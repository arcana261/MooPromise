using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise
{
    public interface IntervalHandleBase
    {
        void Cancel();
        bool IsCanceled { get; }
        int Timeout { get; }
        PromiseFactory Factory { get; }
        PromisePriority Priority { get; }
    }

    public interface IntervalHandle : IntervalHandleBase
    {
        IntervalHandle Then(Action handler);
        IntervalHandle Then(Func<IPromise> handler);
        IntervalHandle<T> Then<T>(Func<IPromise<T>> handler);
        IntervalHandle<T> Then<T>(Func<T> handler);
        IntervalHandle Catch(Action<Exception> handler);
        IntervalHandle Catch(Action handler);
        IntervalHandle Finally(Action<Exception> handler);
        IntervalHandle Finally(Action handler);
    }

    public interface IntervalHandle<T> : IntervalHandleBase
    {
        IntervalHandle Then(Action<T> handler);
        IntervalHandle Then(Func<T, IPromise> handler);
        IntervalHandle<E> Then<E>(Func<T, E> handler);
        IntervalHandle<E> Then<E>(Func<T, IPromise<E>> handler);
        IntervalHandle<T> Catch(Action<Exception> handler);
        IntervalHandle<T> Catch(Action handler);
        IntervalHandle<T> Finally(Action<Exception> handler);
        IntervalHandle<T> Finally(Action handler);
        IntervalHandle Then(Action handler);
        IntervalHandle Then(Func<IPromise> handler);
        IntervalHandle<E> Then<E>(Func<E> handler);
        IntervalHandle<E> Then<E>(Func<IPromise<E>> handler);
        IntervalHandle Cast();
        IntervalHandle<E> Cast<E>();
    }
}
