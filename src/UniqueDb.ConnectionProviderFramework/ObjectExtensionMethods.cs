using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniqueDb.ConnectionProvider
{
    public static class ObjectExtensionMethods
    {
        public static T As<T>(object o)
        {
            return (T) o;
        }
    }
}
