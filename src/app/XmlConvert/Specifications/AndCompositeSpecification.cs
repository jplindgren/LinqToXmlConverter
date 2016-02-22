using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace XmlConvert.Specifications {
    public class AndCompositeSpecification<T> : ISpecification<T> {
        public AndCompositeSpecification(ISpecification<T>[] specifications) {
            this.specifications = specifications;
        }

        public Expression<Func<T, bool>> IsSatisfiedBy() {
            var criterias = specifications.Select(x => x.IsSatisfiedBy());
            //return specifications[0].IsSatisfiedBy().And<T>(specifications[1].IsSatisfiedBy());
            var agreggatedCriterias = criterias.Aggregate((firstCriteria, secondCriteria) => firstCriteria.And<T>(secondCriteria));
            return agreggatedCriterias;
        }

        private ISpecification<T>[] specifications;
    }
}
