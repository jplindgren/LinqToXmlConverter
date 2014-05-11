using System;
using System.Collections.Generic;

namespace XmlConvert {
    public interface IXmlConverter {
        System.Xml.Linq.XDocument Convert(IXmlConvertible entity);

        System.Xml.Linq.XDocument Convert(IList<IXmlConvertible> list);
    }// interface
}
