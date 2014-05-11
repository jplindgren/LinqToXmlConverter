using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlConvert.Tests.Data {
    public class ParentSampleDataWithChildIgnored : SampleDataClass{
        public SampleChildWithIgnoredMember Child { get; set; }
    }// class
}
