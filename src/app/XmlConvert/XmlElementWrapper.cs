using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XmlConvert {
    public class XmlElementWrapper {
        public PropertyInfo PropertyInfo { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }

        public bool IsXmlConvertible() {
            return IsXmlConvertible(PropertyInfo.PropertyType);
        }

        public bool IsXmlConvertible(Type type) {
            return type.GetInterfaces().Contains(typeof(IXmlConvertible));
        }

        public bool IsNotNullValue() {
            return Value != null;
        }
    }
}
