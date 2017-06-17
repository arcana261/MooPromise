using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.ExceptionHandling
{
    internal static class ExceptionUtility
    {
        public static AggregateException AggregateExceptions(string message, System.Exception e1, System.Exception e2)
        {
            if (e1 is AggregateException)
            {
                if (e2 is AggregateException)
                {
                    return new AggregateException(message, ((AggregateException)e1).InnerExceptions.Concat(((AggregateException)e2).InnerExceptions));
                }
                else
                {
                    return new AggregateException(message, ((AggregateException)e1).InnerExceptions.Concat(new System.Exception[] { e2 }));
                }
            }
            else
            {
                if (e2 is AggregateException)
                {
                    return new AggregateException(message, (new System.Exception[] { e1 }).Concat(((AggregateException)e2).InnerExceptions));
                }
                else
                {
                    return new AggregateException(message, e1, e2);
                }
            }
        }
    }
}
