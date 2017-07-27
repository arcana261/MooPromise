using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Async
{
    public class While<T>
    {
        private Scope<T> _owner;
        private Action<Scope<bool>> _condition;

        internal While(Scope<T> owner, Action<Scope<bool>> condition)
        {
            this._owner = owner;
            this._condition = condition;
        }

        public Scope<T> Do(Action<Scope<T>> block)
        {
            return _owner.Run(() => _owner.Factory.Control.While(() => _owner.BeginImmediately<bool>(_condition).Finish().Then(result => result.Value)).Do(() => _owner.BeginImmediately<T>(block).Finish()));
        }

        public Scope<T> Do(Action block)
        {
            return Do(scope => block());
        }
    }
}
