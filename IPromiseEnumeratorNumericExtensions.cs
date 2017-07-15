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

        public static IPromise<int> Min(this IPromiseEnumerable<int> items)
        {
            return items.First().Then(first => items.Aggregate((prev, current) => Math.Min(prev, current), first));
        }

        public static IPromise<long> Min(this IPromiseEnumerable<long> items)
        {
            return items.First().Then(first => items.Aggregate((prev, current) => Math.Min(prev, current), first));
        }

        public static IPromise<float> Min(this IPromiseEnumerable<float> items)
        {
            return items.First().Then(first => items.Aggregate((prev, current) => Math.Min(prev, current), first));
        }

        public static IPromise<double> Min(this IPromiseEnumerable<double> items)
        {
            return items.First().Then(first => items.Aggregate((prev, current) => Math.Min(prev, current), first));
        }

        public static IPromise<decimal> Min(this IPromiseEnumerable<decimal> items)
        {
            return items.First().Then(first => items.Aggregate((prev, current) => Math.Min(prev, current), first));
        }

        public static IPromise<int?> Min(this IPromiseEnumerable<int?> items)
        {
            var filtered = items.Where(x => x.HasValue).Select(x => x.Value);

            return filtered.Empty().Then(empty => empty ? items.Factory.Value<int?>(null) : filtered.Min().Then(v => ((int?)v)));
        }

        public static IPromise<long?> Min(this IPromiseEnumerable<long?> items)
        {
            var filtered = items.Where(x => x.HasValue).Select(x => x.Value);

            return filtered.Empty().Then(empty => empty ? items.Factory.Value<long?>(null) : filtered.Min().Then(v => ((long?)v)));
        }

        public static IPromise<float?> Min(this IPromiseEnumerable<float?> items)
        {
            var filtered = items.Where(x => x.HasValue).Select(x => x.Value);

            return filtered.Empty().Then(empty => empty ? items.Factory.Value<float?>(null) : filtered.Min().Then(v => ((float?)v)));
        }

        public static IPromise<double?> Min(this IPromiseEnumerable<double?> items)
        {
            var filtered = items.Where(x => x.HasValue).Select(x => x.Value);

            return filtered.Empty().Then(empty => empty ? items.Factory.Value<double?>(null) : filtered.Min().Then(v => ((double?)v)));
        }

        public static IPromise<decimal?> Min(this IPromiseEnumerable<decimal?> items)
        {
            var filtered = items.Where(x => x.HasValue).Select(x => x.Value);

            return filtered.Empty().Then(empty => empty ? items.Factory.Value<decimal?>(null) : filtered.Min().Then(v => ((decimal?)v)));
        }

        public static IPromise<int> Max(this IPromiseEnumerable<int> items)
        {
            return items.First().Then(first => items.Aggregate((prev, current) => Math.Max(prev, current), first));
        }

        public static IPromise<long> Max(this IPromiseEnumerable<long> items)
        {
            return items.First().Then(first => items.Aggregate((prev, current) => Math.Max(prev, current), first));
        }

        public static IPromise<float> Max(this IPromiseEnumerable<float> items)
        {
            return items.First().Then(first => items.Aggregate((prev, current) => Math.Max(prev, current), first));
        }

        public static IPromise<double> Max(this IPromiseEnumerable<double> items)
        {
            return items.First().Then(first => items.Aggregate((prev, current) => Math.Max(prev, current), first));
        }

        public static IPromise<decimal> Max(this IPromiseEnumerable<decimal> items)
        {
            return items.First().Then(first => items.Aggregate((prev, current) => Math.Max(prev, current), first));
        }

        public static IPromise<int?> Max(this IPromiseEnumerable<int?> items)
        {
            var filtered = items.Where(x => x.HasValue).Select(x => x.Value);

            return filtered.Empty().Then(empty => empty ? items.Factory.Value<int?>(null) : filtered.Max().Then(v => ((int?)v)));
        }

        public static IPromise<long?> Max(this IPromiseEnumerable<long?> items)
        {
            var filtered = items.Where(x => x.HasValue).Select(x => x.Value);

            return filtered.Empty().Then(empty => empty ? items.Factory.Value<long?>(null) : filtered.Max().Then(v => ((long?)v)));
        }

        public static IPromise<float?> Max(this IPromiseEnumerable<float?> items)
        {
            var filtered = items.Where(x => x.HasValue).Select(x => x.Value);

            return filtered.Empty().Then(empty => empty ? items.Factory.Value<float?>(null) : filtered.Max().Then(v => ((float?)v)));
        }

        public static IPromise<double?> Max(this IPromiseEnumerable<double?> items)
        {
            var filtered = items.Where(x => x.HasValue).Select(x => x.Value);

            return filtered.Empty().Then(empty => empty ? items.Factory.Value<double?>(null) : filtered.Max().Then(v => ((double?)v)));
        }

        public static IPromise<decimal?> Max(this IPromiseEnumerable<decimal?> items)
        {
            var filtered = items.Where(x => x.HasValue).Select(x => x.Value);

            return filtered.Empty().Then(empty => empty ? items.Factory.Value<decimal?>(null) : filtered.Max().Then(v => ((decimal?)v)));
        }
    }
}
