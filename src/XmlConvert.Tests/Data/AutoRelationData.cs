using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlConvert.Tests.Data {
    public class AutoRelationData : SampleDataClass{
        public SampleDataClass SampleDataClass { get; set; }
    }
}
