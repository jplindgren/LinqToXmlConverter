using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace XmlConvert.Specifications {
    public class PropertyNotIgnoredSpecification : ISpecification<PropertyInfo>{
        private static bool NotIgnored(PropertyInfo property) {
            var attr = property.GetCustomAttributes(typeof(IgnoreXmlConvertAttribute), true);
            return attr.Length == 0;
        }

        public Expression<Func<PropertyInfo, bool>> IsSatisfiedBy() {
            return c => NotIgnored(c);
        }
    } //class
}
