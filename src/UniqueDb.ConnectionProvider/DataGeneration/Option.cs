using System;
using System.Collections;
using System.Collections.Generic;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public class Option<T> : IEnumerable<T>
    {
        public T Value { get; private set; }
        public bool HasValue { get { return this != None; } }

        public static readonly Option<T> None = new Option<T>(default(T));
        public static Option<T> Some(T value)
        {
            return new Option<T>(value);
        }

        private Option(T value)
        {
            Value = value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (HasValue)
                yield return Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TRes Match<TRes>(Func<T, TRes> some, Func<TRes> none)
        {
            if (HasValue)
                return some.Invoke(Value);
            else
                return none.Invoke();
        }

        public void Match(Action<T> some, Action none)
        {
            if (HasValue)
                some.Invoke(Value);
            else
                none.Invoke();
        }

        public static implicit operator Option<T>(T obj)
        {
            if (Equals(obj, default(T)))
                return None;
            else
                return new Option<T>(obj);
        }
    }
}
