using System.Collections;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration;

public class TimeoutAttribute : Attribute
{
    private IDictionary _properties;

    public TimeoutAttribute(int timeout)
    {
        _properties            = new Hashtable();
        _properties["Timeout"] = timeout;
    }

    public IDictionary Properties
    {
        get { return _properties; }
    }
}