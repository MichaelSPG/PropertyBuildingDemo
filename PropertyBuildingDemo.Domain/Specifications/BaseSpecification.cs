using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Domain.Specification
{
    public class BaseSpecifications<T> : ISpecification<T>
    {
        public BaseSpecifications() { }
        public Expression<Func<T, bool>>            Criteria {get;}
        public List<Expression<Func<T, object>>>    Includes { get; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>>          OrderBy {get; private set;}
        public Expression<Func<T, object>>          OrderByDescending {get; private set; }
        public int                                  Take {get; private set; }
        public int                                  Skip {get; private set; }
        public bool                                 IsPagingEnabled {get; private set; }

        protected void  AddIncludes(Expression<Func<T, object>> InIncludeExpression)
        {
            Includes.Add(InIncludeExpression);
        }
        public void     AddOrderBy(Expression<Func<T, object>> InOrderByExpression)
        {
            OrderBy = InOrderByExpression;
        }
        public void     AddOrderByDescending(Expression<Func<T, object>> InOrderByDescendingExpression)
        {
            OrderByDescending = InOrderByDescendingExpression;
        }

        public void     ApplyingPagging(int InTake, int InSkip)
        {
            this.Take = InTake;
            this.Skip = InSkip;
        }
    }
}
