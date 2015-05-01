using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public static class AutomaticPropertyNameRewrites
    {
        private static Lazy<IList<PropertyNameRewrite>> _rewriters = new Lazy<IList<PropertyNameRewrite>>(() => new List<PropertyNameRewrite>()
        {
            new RewriteNumericalName(),
            new RewriteMultiWordName()
        }); 

        public static IList<PropertyNameRewrite> Rewriters
        {
            get { return _rewriters.Value; }
        }
    }
}
