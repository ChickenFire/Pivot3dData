using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace TestPOI
{
    public class Utils
    {
        public static PropertyInfo GetPropertyInfo(string propertyName, Type dataType)
        {
            var prop = dataType.GetProperties()
                .FirstOrDefault(c => c.Name.Equals(propertyName));

            return prop;
        }
    }
}
