using System.Collections.Generic;
using System.Reflection;

namespace UniqueDb.ConnectionProvider.DataGeneration.SqlManipulation
{
    public class PropertyInfoWithAttributes
    {
        public PropertyInfo PropertyInfo { get; set; }
        public IList<object> Attributes { get; set; }

        public PropertyInfoWithAttributes(PropertyInfo propertyInfo, IList<object> attributes)
        {
            PropertyInfo = propertyInfo;
            Attributes = attributes;
        }
    }
}