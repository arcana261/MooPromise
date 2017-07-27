using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Async
{
    public class DefinitionBag
    {
        private class Value
        {
            public Type Type { get; set; }
            public object Variable { get; set; }

            public Value(Type type, object value)
            {
                this.Type = type;
                this.Variable = value;
            }
        }

        private DefinitionBag _parent;
        private IDictionary<string, Value> _bag;

        public DefinitionBag(DefinitionBag parent)
        {
            this._parent = parent;
            this._bag = new Dictionary<string, Value>();
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

                _bag.Add(name, new Value(typeof(T), null));
            }
        }

        public void Define<T>(string name, T value)
        {
            lock (this)
            {
                Define<T>(name);
                Set<T>(name, value);
            }
        }

        public T Get<T>(string name)
        {
            lock (this)
            {
                Value x;

                if (!_bag.TryGetValue(name, out x))
                {
                    if (_parent != null)
                    {
                        return _parent.Get<T>(name);
                    }

                    throw new InvalidOperationException("variable not defined");
                }

                Type t = typeof(T);

                if (!x.Type.IsAssignableFrom(t))
                {
                    throw new InvalidCastException();
                }

                return (T)x.Variable;
            }
        }

        public void Set<T>(string name, T value)
        {
            lock (this)
            {
                Value x;

                if (!_bag.TryGetValue(name, out x))
                {
                    if (_parent != null)
                    {
                        _parent.Set(name, value);
                        return;
                    }

                    throw new InvalidOperationException("variable not defined");
                }

                Type t = typeof(T);

                if (!x.Type.IsAssignableFrom(t))
                {
                    throw new InvalidCastException();
                }

                x.Variable = (object)value;
            }
        }
    }
}
