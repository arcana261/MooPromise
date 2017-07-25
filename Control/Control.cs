using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Control
{
    public class Control
    {
        private PromiseFactory _factory;

        internal Control(PromiseFactory factory)
        {
            this._factory = factory;
        }

        public While While(Func<IPromise<bool>> condition)
        {
            return new While(_factory, condition);
        }

        public While While(Func<bool> condition)
        {
            return new While(_factory, condition);
        }

        public For<T> For<T>(T seed, Func<T, IPromise<bool>> condition, Func<T, IPromise<T>> iterator)
        {
            return new For<T>(_factory, seed, condition, iterator);
        }

        public For<T> For<T>(T seed, Func<T, bool> condition, Func<T, IPromise<T>> iterator)
        {
            return For(seed, x => _factory.Value(condition(x)), iterator);
        }

        public For<T> For<T>(T seed, Func<T, IPromise<bool>> condition, Func<T, T> iterator)
        {
            return For(seed, condition, x => _factory.Value(iterator(x)));
        }

        public For<T> For<T>(T seed, Func<T, bool> condition, Func<T, T> iterator)
        {
            return For(seed, x => _factory.Value(condition(x)), x => _factory.Value(iterator(x)));
        }

        public If If(Func<IPromise<bool>> condition)
        {
            return new If(_factory, condition);
        }

        public If If(Func<bool> condition)
        {
            return new If(_factory, condition);
        }
    }
}
