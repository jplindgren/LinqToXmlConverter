﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlConvert.Tests.Data {
    public class SampleChildWithIgnoredMember : IXmlConvertible{
        [IgnoreXmlConvert]
        public int Id { get; set; }
        public string Description { get; set; }
    }// class
}
