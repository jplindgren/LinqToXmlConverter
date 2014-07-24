using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;

namespace XmlConvert {
    public static class XxmlConverterExtension {
        public static XDocument ConvertToXml(this IXmlConvertible entity) {
            IXmlConverter xmlConverter = new XmlConverter();
            return xmlConverter.Convert(entity);
        }

        public static XDocument ConvertToXmlAndSave(this IXmlConvertible entity, string path) {
            XDocument xDoc = ConvertToXml(entity);
            xDoc.Save(path);
            return xDoc;
        }

        public static XDocument ConvertToXml<T>(this IList<T> list) where T : IXmlConvertible{
            IXmlConverter xmlConverter = new XmlConverter();
            return xmlConverter.Convert(list.Cast<IXmlConvertible>().ToList());
        }

        public static XDocument ConvertToXmlAndSave<T>(this IList<T> list, string path) where T : IXmlConvertible {
            XDocument xDoc = ConvertToXml<T>(list);
            xDoc.Save(path);
            return xDoc;
        }
    }// class
}
