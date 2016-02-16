using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using System.Globalization;

[assembly: CLSCompliant(true)]
namespace XmlConvert {
    public class XmlConverter : IXmlConverter {
        private XmlConvertSettings settings;

        public XmlConverter(XmlConvertSettings settings) {
            this.settings = settings ?? XmlConvertSettings.CreateDefault();
        }

        public XDocument Convert(IXmlConvertible entity) {
            XDocument xdoc = new XDocument(CreateXElement(entity));
            return xdoc;
        }

        public string ConvertAsString(IXmlConvertible entity) {
            SaveOptions options = settings.Formatting == Formatting.Indented ? SaveOptions.None : SaveOptions.DisableFormatting;
            XDocument xdoc = Convert(entity);            
            return xdoc.ToString(options);
        }

        public XDocument ConvertAndSave(IXmlConvertible entity, string path) {
            XDocument xdoc = Convert(entity);
            xdoc.Save(path);
            return xdoc;
        }

        public string ConvertAsStringAndSave(IXmlConvertible entity, string path) {
            SaveOptions options = settings.Formatting == Formatting.Indented ? SaveOptions.DisableFormatting : SaveOptions.None;
            XDocument xdoc = ConvertAndSave(entity, path);
            return xdoc.ToString(options);
        }


        public XDocument Convert<T>(IList<T> list) where T : IXmlConvertible {
            var collectionName = list.GetType().GetGenericArguments().FirstOrDefault(type => IsXmlConvertible(type)).Name;
            XDocument xdoc = new XDocument(new XElement(string.Format(CultureInfo.CurrentCulture , "{0}Collection", collectionName),
                from _item in list
                select CreateXElement(_item)
            ));
            return xdoc;
        }

        public string ConvertAsString<T>(IList<T> list) where T : IXmlConvertible {
            SaveOptions options = settings.Formatting == Formatting.Indented ? SaveOptions.DisableFormatting : SaveOptions.None;
            XDocument xdoc = Convert(list);
            return xdoc.ToString(options);
        }

        public XDocument ConvertAndSave<T>(IList<T> list, string path) where T : IXmlConvertible {
            XDocument xdoc = Convert<T>(list);
            xdoc.Save(path);
            return xdoc;
        }

        public string ConvertAsStringAndSave<T>(IList<T> list, string path) where T : IXmlConvertible {
            SaveOptions options = settings.Formatting == Formatting.Indented ? SaveOptions.DisableFormatting : SaveOptions.None;
            XDocument xdoc = ConvertAndSave<T>(list, path);
            return xdoc.ToString(options);
        }


        #region private methods
        private static XElement CreateXElement(IXmlConvertible entity) {
            var xElement = new XElement(entity.GetType().Name,
                     from property in GetNotIgnoredProperties(entity.GetType())
                     select CreateElement(property, entity));
            return xElement;
        }

        private static object CreateElement(PropertyInfo property, IXmlConvertible entity) {
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

        private static bool IsNotNullValue(PropertyInfo property, IXmlConvertible entity) {
            return property.GetValue(entity, null) != null;
        }

        private static XElement CreateXElement(PropertyInfo property, object content) {
            return new XElement(property.Name, content);
        }

        private static XElement CreateXElement(PropertyInfo property, IXmlConvertible entity) {
            var value = property.GetValue(entity, null);
            return CreateXElement(property, value ?? string.Empty);
        }

        private static bool IsXmlConvertible(PropertyInfo property) {
            return IsXmlConvertible(property.PropertyType);
        }

        private static bool IsXmlConvertible(Type type) {
            return type.GetInterfaces().Contains(typeof(IXmlConvertible));
        }

        private static PropertyInfo[] GetNotIgnoredProperties(Type type) {
            return type.GetProperties().Where(x => NotIgnored(x)).ToArray();
        }

        private static bool NotIgnored(PropertyInfo property) {
            var attr = property.GetCustomAttributes(typeof(IgnoreXmlConvertAttribute), true);
            return attr.Length == 0;
        }

        //public XAttribute CreateXAttributeFromProperty(PropertyInfo property, IXmlConvertible entity){
        //    var value =  property.GetValue(entity, null);
        //    return new XAttribute("Value", value ?? string.Empty);
        //}

        #endregion

    }// class
}
