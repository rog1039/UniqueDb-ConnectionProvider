using System;
using System.Collections.Generic;
using System.Linq;

namespace UniqueDb.ConnectionProvider.DataGeneration;

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
        
    public static string GetNameWithRewriting(string name)
    {
        var rewriters = AutomaticPropertyNameRewrites.Rewriters.FirstOrDefault(x => x.ShouldRewrite(name));
            
        return rewriters != null 
            ? rewriters.Rewrite(name) 
            : name;
    }
}