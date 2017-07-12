using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise
{
    public static class IPromiseEnumeratorNumericExtensions
    {
        public static IPromise<int> Sum(this IPromiseEnumerable<int> items)
        {
            return items.Aggregate((prev, current) => prev + current, (int)0);
        }

        public static IPromise<long> Sum(this IPromiseEnumerable<long> items)
        {
            return items.Aggregate((prev, current) => prev + current, (long)0);
        }

        public static IPromise<decimal> Sum(this IPromiseEnumerable<decimal> items)
        {
            return items.Aggregate((prev, current) => prev + current, (decimal)0);
        }

        public static IPromise<float> Sum(this IPromiseEnumerable<float> items)
        {
            return items.Aggregate((prev, current) => prev + current, (float)0);
        }

        public static IPromise<double> Sum(this IPromiseEnumerable<double> items)
        {
            return items.Aggregate((prev, current) => prev + current, (double)0);
        }

        public static IPromise<int> Sum(this IPromiseEnumerable<int?> items)
        {
            return items.Sum(x => x.HasValue ? x.Value : (int)0);
        }

        public static IPromise<long> Sum(this IPromiseEnumerable<long?> items)
        {
            return items.Sum(x => x.HasValue ? x.Value : (long)0);
        }

        public static IPromise<decimal> Sum(this IPromiseEnumerable<decimal?> items)
        {
            return items.Sum(x => x.HasValue ? x.Value : (decimal)0);
        }

        public static IPromise<float> Sum(this IPromiseEnumerable<float?> items)
        {
            return items.Sum(x => x.HasValue ? x.Value : (float)0);
        }

        public static IPromise<double> Sum(this IPromiseEnumerable<double?> items)
        {
            return items.Sum(x => x.HasValue ? x.Value : (double)0);
        }

        public static IPromise<double> Average(this IPromiseEnumerable<int> items)
        {
            return items.Sum().Then(sum => items.Count().Then(count => ((double)sum) / ((double)count)));
        }

        public static IPromise<double> Average(this IPromiseEnumerable<long> items)
        {
            return items.Sum().Then(sum => items.Count().Then(count => ((double)sum) / ((double)count)));
        }

        public static IPromise<double> Average(this IPromiseEnumerable<decimal> items)
        {
            return items.Sum().Then(sum => items.Count().Then(count => ((double)sum) / ((double)count)));
        }

        public static IPromise<double> Average(this IPromiseEnumerable<float> items)
        {
            return items.Sum().Then(sum => items.Count().Then(count => ((double)sum) / ((double)count)));
        }

        public static IPromise<double> Average(this IPromiseEnumerable<double> items)
        {
            return items.Sum().Then(sum => items.Count().Then(count => ((double)sum) / ((double)count)));
        }

        public static IPromise<double> Average(this IPromiseEnumerable<int?> items)
        {
            return items.Sum().Then(sum => items.Count().Then(count => ((double)sum) / ((double)count)));
        }

        public static IPromise<double> Average(this IPromiseEnumerable<long?> items)
        {
            return items.Sum().Then(sum => items.Count().Then(count => ((double)sum) / ((double)count)));
        }

        public static IPromise<double> Average(this IPromiseEnumerable<decimal?> items)
        {
            return items.Sum().Then(sum => items.Count().Then(count => ((double)sum) / ((double)count)));
        }

        public static IPromise<double> Average(this IPromiseEnumerable<float?> items)
        {
            return items.Sum().Then(sum => items.Count().Then(count => ((double)sum) / ((double)count)));
        }

        public static IPromise<double> Average(this IPromiseEnumerable<double?> items)
        {
            return items.Sum().Then(sum => items.Count().Then(count => ((double)sum) / ((double)count)));
        }
    }
}
