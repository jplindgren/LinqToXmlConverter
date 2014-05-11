using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;

namespace XmlConvert {
    public class XmlConverter : IXmlConverter {
        public XDocument Convert(IXmlConvertible entity) {
            XDocument xdoc = new XDocument(CreateXElement(entity));
            return xdoc;
        }

        public XDocument Convert(IList<IXmlConvertible> list) {
            XDocument xdoc = new XDocument(new XElement("Collection", 
                from _item in list
                select CreateXElement(_item)
            ));
            return xdoc;
        }

        private XElement CreateXElement(IXmlConvertible entity) {
            var xElement = new XElement(entity.GetType().Name,
                     from property in GetNotIgnoredProperties(entity.GetType())
                     select CreateElement(property, entity));
            return xElement;
        }

        private object CreateElement(PropertyInfo property, IXmlConvertible entity) {            
            if (IsXmlConvertible(property) && IsNotNullValue(property, entity)) {
                // recursive create others elements inside
                return CreateXElement(property,
                    from innerProperty in GetNotIgnoredProperties(property.PropertyType)
                    select CreateElement(innerProperty, (IXmlConvertible)property.GetValue(entity, null)));
            } else {
                //Create element with value
                return CreateXElement(property, entity);
            }
        }

        private bool IsNotNullValue(PropertyInfo property, IXmlConvertible entity) {
            return property.GetValue(entity, null) != null;
        }

        private XElement CreateXElement(PropertyInfo property, object content) {
            return new XElement(property.Name, content);
        }

        private XElement CreateXElement(PropertyInfo property, IXmlConvertible entity) {
            var value = property.GetValue(entity, null);
            return CreateXElement(property, value ?? string.Empty);
        }

        private bool IsXmlConvertible(PropertyInfo property) {
            return property.PropertyType.GetInterfaces().Contains(typeof(IXmlConvertible));
        }

        private PropertyInfo[] GetNotIgnoredProperties(Type type) {
            return type.GetProperties().Where(x => NotIgnored(x)).ToArray();
        }

        private bool NotIgnored(PropertyInfo property) {
            var attr = property.GetCustomAttributes(typeof(IgnoreXmlConvert), true);
            return attr.Length == 0;
        }

        //public XAttribute CreateXAttributeFromProperty(PropertyInfo property, IXmlConvertible entity){
        //    var value =  property.GetValue(entity, null);
        //    return new XAttribute("Value", value ?? string.Empty);
        //}

    }// class
}
