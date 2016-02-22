using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlConvert {
    public enum NullValueHandling {
        /// <summary>
        /// Include null values when serializing and deserializing objects.
        /// </summary>
        Include = 0,

        /// <summary>
        /// Ignore null values when serializing and deserializing objects.
        /// </summary>
        Ignore = 1
    } //enum
}
