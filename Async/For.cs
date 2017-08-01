using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Async
{
    public class For<T>
    {
        private Scope<T> _owner;
        private Action<Scope<T>> _initial;
        private Action<T, Scope<bool>> _condition;
        private Action<T, Scope<T>> _iterator;

        internal For(Scope<T> owner, Func<IPromise<T>> initial, Action<T, Scope<bool>> condition, Action<T, Scope<T>> iterator)
        {
            this._owner = owner;
            //this._initial = initial;
            this._condition = condition;
            this._iterator = iterator;
        }

        //public Scope<T> Do(Action<T, Scope<T>> body)
        //{
        //    return _owner.Run(() =>
        //    {
        //        var initial = new Func<IPromise<T>>(() =>
        //        {
        //            return _owner.BeginImmediately<T>(newScope => _initial(newScope)).Finish().Then(result =>
        //            {
        //                if (result == null || result.State != ControlState.Return)
        //                {
        //                    return null;
        //                }

        //                return _owner.Factory.Value(result.Value);
        //            });
        //        });

        //        var condition = new Func<T, IPromise<bool>>(value =>
        //        {
        //            return _owner.BeginImmediately<bool>(newScope => _condition(value, newScope)).Finish().Then(result =>
        //            {
        //                if (result == null || result.State != ControlState.Return)
        //                {
        //                    return null;
        //                }

        //                return _owner.Factory.Value(result.Value);
        //            });
        //        });

        //        var iterator = new Func<T, IPromise<T>>(value =>
        //        {
        //            return _owner.BeginImmediately<T>(newScope => _iterator(value, newScope)).Finish().Then(result =>
        //            {
        //                if (result == null || result.State != ControlState.Return)
        //                {
        //                    return null;
        //                }

        //                return _owner.Factory.Value(result.Value);
        //            });
        //        });

        //        return next.Then(initial => _owner.Factory.Control.For(initial,
        //            condition,
        //            iterator)
        //            .Do(value => _owner.BeginImmediately<T>(newScope => body(value, newScope)).Finish()));
        //    });
        //}

        //public Scope<T> Do(Action<T> body)
        //{
        //    return Do((value, scope) => body(value));
        //}

        //public Scope<T> Do(Action body)
        //{
        //    return Do(value => body());
        //}
    }
}
