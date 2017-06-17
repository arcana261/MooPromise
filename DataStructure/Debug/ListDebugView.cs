using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MooPromise.DataStructure.Debug
{
#if DEBUG
    internal class ListDebugView
    {
        private object _list;
        private MethodInfo _copyToMethod;
        private PropertyInfo _countProperty;
        private Type T;

        public ListDebugView(object list)
        {
            _list = list;

            Type t_list = _list.GetType();
            T = t_list.GetGenericArguments()[0];

            MethodInfo[] methods = t_list.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] properties = t_list.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < methods.Length; i++)
            {
                if (methods[i].Name.Equals("CopyTo"))
                {
                    _copyToMethod = methods[i];
                    break;
                }
            }

            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].Name.Equals("Count"))
                {
                    _countProperty = properties[i];
                    break;
                }
            }

            if (_copyToMethod == null || _countProperty == null)
            {
                throw new InvalidOperationException("list is not IList compatible");
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public object[] Items
        {
            get
            {
                int count = (int)_countProperty.GetValue(_list, null);
                Array content = Array.CreateInstance(T, count);
                _copyToMethod.Invoke(_list, new object[] { content, 0 });

                object[] result = new object[count];

                for (int i = 0; i < count; i++)
                {
                    result[i] = content.GetValue(i);
                }

                return result;
            }
        }
    }
#endif
}
