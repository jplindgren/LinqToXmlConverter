using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace XmlConvert {
    public interface IXmlConverter {
        XDocument Convert<T>(IList<T> list) where T : IXmlConvertible;
        string ConvertAsString<T>(IList<T> list) where T : IXmlConvertible;
        XDocument ConvertAndSave<T>(IList<T> list, string path) where T : IXmlConvertible;
        string ConvertAsStringAndSave<T>(IList<T> list, string path) where T : IXmlConvertible;

        XDocument Convert(IXmlConvertible entity);
        string ConvertAsString(IXmlConvertible entity);
        XDocument ConvertAndSave(IXmlConvertible entity, string path);
        string ConvertAsStringAndSave(IXmlConvertible entity, string path);
    }// interface
}
