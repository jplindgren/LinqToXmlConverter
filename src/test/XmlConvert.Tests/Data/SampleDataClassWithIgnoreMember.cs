﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlConvert.Tests.Data {   
    public class SampleDataClassWithIgnoreMember : IXmlConvertible{
        public int Id { get; set; }

        [IgnoreXmlConvertAttribute]
        public string Description { get; set; }
    }// class
}
