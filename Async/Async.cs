using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Async
{
    public class Async
    {
        private PromiseFactory _factory;

        internal Async(PromiseFactory factory)
        {
            this._factory = factory;
        }

        public IPromise<T> Begin<T>(Action<Scope<T>> block)
        {
            Scope<T> scope = new Scope<T>(_factory);
            block(scope);

            return scope.Finish().Then(result =>
            {
                if (result == null || result.State != ControlState.Return)
                {
                    throw new InvalidOperationException();
                }

                return result.Value;
            });
        }
    }
}
