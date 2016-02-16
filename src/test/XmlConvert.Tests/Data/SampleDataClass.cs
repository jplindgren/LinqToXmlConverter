using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlConvert.Tests.Data {
    public class SampleDataClass : IXmlConvertible{
        public int Id { get; set; }
        public string Description { get;set; }
    }// class
}
