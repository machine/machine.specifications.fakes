using System;
using System.Linq.Expressions;

namespace Machine.Fakes.Sdk
{
    public class Param
    {
        public static TValue Is<TValue>(Expression<Func<bool>> match)
        {
            return default(TValue);
        }

        public static TValue IsAny<TValue>()
        {
            return default(TValue);
        }

        public T IsOfType<T>()
        {
            return default(T);
        }
    }

    public class Param<T>
    {
        public T IsNull
        {
            get { return default(T); }
        }

        public T IsNotNull
        {
            get { return default(T); }
        }

        public static T IsAnything
        {
            get { return default(T); }            
        }

        public static T Is(Expression<Func<T, bool>> match)
        {
            return default(T);
        }

        public TOther IsOfType<TOther>()
        {
            return default(TOther);
        }
    }
}