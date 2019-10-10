using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TPSLRawDataSimulator
{
    public class GenericComparer<T,O> : IComparer<T> where O:IComparable
    {
        public Func<T, O> CompareElementSelector { get; private set; }

        public GenericComparer(Func<T,O> selector) {
            this.CompareElementSelector = selector;
        }

        public int Compare(T x, T y)
        {
            if (this.CompareElementSelector == null)
                throw new NullReferenceException("GenericComparer.CompareElementSelector");
            if (typeof(O).GetInterfaces().Any(type => type == typeof(IComparable)))
            {
                var left = this.CompareElementSelector(x);
                var right = this.CompareElementSelector(y);
                return (left as IComparable).CompareTo(right);
            }
            else 
            {
                throw new InvalidOperationException($"The return type:{typeof(O).FullName} of selector is not comparable");
            }
        }
    }
}
