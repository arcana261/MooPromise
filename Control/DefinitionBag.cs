using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Control
{
    internal class DefinitionBag
    {
        private DefinitionBag _parent;
        private IDictionary<string, Tuple<Type, object>> _bag;

        public DefinitionBag(DefinitionBag parent)
        {
            this._parent = parent;
            this._bag = new Dictionary<string, Tuple<Type, object>>();
        }

        public DefinitionBag()
            : this(null)
        {

        }

        public void Define<T>(string name)
        {
            lock (this)
            {
                if (_bag.ContainsKey(name))
                {
                    throw new InvalidOperationException("parameter value re-definition");
                }

                _bag.Add(name, Tuple.Create<Type, object>(typeof(T), null));
            }
        }

        public T Get<T>(string name)
        {
            lock (this)
            {
                Tuple<Type, object> x;

                if (!_bag.TryGetValue(name, out x))
                {
                    if (_parent != null)
                    {
                        return _parent.Get<T>(name);
                    }

                    throw new InvalidOperationException("variable not defined");
                }

                Type t = typeof(T);

                if (!x.Item1.IsAssignableFrom(t))
                {
                    throw new InvalidCastException();
                }

                return (T)x.Item2;
            }
        }
    }
}
