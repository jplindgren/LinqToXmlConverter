using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;

namespace XmlConvert {
    public static class XmlConverterExtension {
        public static XDocument ConvertToXml(this IXmlConvertible entity) {
            return ConvertToXml(entity, null);
        }

        public static XDocument ConvertToXml(this IXmlConvertible entity, XmlConvertSettings settings) {
            IXmlConverter xmlConverter = new XmlConverter(settings);
            return xmlConverter.Convert(entity);
        }

        public static string Convert(this IXmlConvertible entity) {
            return Convert(entity, null);
        }

        public static string Convert(this IXmlConvertible entity, XmlConvertSettings settings) {
            IXmlConverter xmlConverter = new XmlConverter(settings);
            return xmlConverter.ConvertAsString(entity);
        }

        public static string ConvertAndSave(this IXmlConvertible entity, string path, XmlConvertSettings settings) {
            IXmlConverter xmlConverter = new XmlConverter(settings);
            return xmlConverter.ConvertAsStringAndSave(entity, path);
        }

        public static string ConvertAndSave(this IXmlConvertible entity, string path) {
            return ConvertAndSave(entity, path, null);
        }

        public static XDocument ConvertToXmlAndSave(this IXmlConvertible entity, string path) {
            return ConvertToXmlAndSave(entity, path, null);
        }

        public static XDocument ConvertToXmlAndSave(this IXmlConvertible entity, string path, XmlConvertSettings settings) {
            IXmlConverter xmlConverter = new XmlConverter(settings);
            return xmlConverter.ConvertAndSave(entity, path);            
        }

        public static XDocument ConvertToXml<T>(this IList<T> list) where T : IXmlConvertible {
            return ConvertToXml<T>(list, null);
        }

        public static XDocument ConvertToXml<T>(this IList<T> list, XmlConvertSettings settings) where T : IXmlConvertible {
            IXmlConverter xmlConverter = new XmlConverter(settings);
            return xmlConverter.Convert<T>(list);
        }

        public static string Convert<T>(this IList<T> list) where T : IXmlConvertible {
            return Convert<T>(list, null);
        }

        public static string Convert<T>(this IList<T> list, XmlConvertSettings settings) where T : IXmlConvertible {
            IXmlConverter xmlConverter = new XmlConverter(settings);
            return xmlConverter.ConvertAsString<T>(list);
        }

        public static XDocument ConvertToXmlAndSave<T>(this IList<T> list, string path) where T : IXmlConvertible {
            return ConvertToXmlAndSave<T>(list, path, null);
        }

        public static XDocument ConvertToXmlAndSave<T>(this IList<T> list, string path, XmlConvertSettings settings) where T : IXmlConvertible {
            IXmlConverter xmlConverter = new XmlConverter(settings);
            return xmlConverter.ConvertAndSave<T>(list, path);
        }

        public static string ConvertAndSave<T>(this IList<T> list, string path) where T : IXmlConvertible {
            return ConvertAndSave<T>(list, path, null);
        }

        public static string ConvertAndSave<T>(this IList<T> list, string path, XmlConvertSettings settings) where T : IXmlConvertible {
            IXmlConverter xmlConverter = new XmlConverter(settings);
            return xmlConverter.ConvertAsStringAndSave<T>(list, path);
        }
    }// class
}
