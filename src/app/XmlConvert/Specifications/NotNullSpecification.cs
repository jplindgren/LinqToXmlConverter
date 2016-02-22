using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlConvert.Specifications {
    public class NotNullSpecification : ISpecification<XmlElementWrapper>{
        public System.Linq.Expressions.Expression<Func<XmlElementWrapper, bool>> IsSatisfiedBy() {
            return element => element.Value != null;
        }
    }
}
