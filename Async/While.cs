using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Async
{
    public class While<T>
    {
        private Scope<T> _owner;
        private Func<IPromise<bool>> _condition;

        internal While(Scope<T> owner, Func<IPromise<bool>> condition)
        {
            this._owner = owner;
            this._condition = condition;
        }

        public Scope<T> Do(Action<Scope<T>> block)
        {

        }
    }
}
