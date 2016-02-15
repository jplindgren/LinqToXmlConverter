using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlConvert {
    public class XmlConvertSettings {
        public Formatting Formatting { get; set; }

        public static XmlConvertSettings CreateDefault(){
            return new XmlConvertSettings();
        }
    } //class
}
