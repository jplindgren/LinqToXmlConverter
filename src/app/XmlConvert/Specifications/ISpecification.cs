﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace XmlConvert.Specifications {
    public interface ISpecification<T> {
        Expression<Func<T, bool>> IsSatisfiedBy();
    }
}
