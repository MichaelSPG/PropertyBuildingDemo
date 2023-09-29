using PropertyBuildingDemo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LinqKit;

namespace PropertyBuildingDemo.Domain.Specification
{
    public class BaseSpecifications<TEntity> : ISpecifications<TEntity> where TEntity : class, IEntityDB
    {
        public BaseSpecifications() { }

        public BaseSpecifications(Expression<Func<TEntity, bool>> inCriteria)
        {
            this.Criteria = inCriteria;
        }
        
        public Expression<Func<TEntity, bool>>            Criteria {get; set; }
        public List<Expression<Func<TEntity, object>>>    Includes { get; } = new List<Expression<Func<TEntity, object>>>();
        public Expression<Func<TEntity, object>>          OrderBy {get; private set;}
        public Expression<Func<TEntity, object>>          OrderByDescending {get; private set; }
        public int                                  Take {get; private set; }
        public int                                  Skip {get; private set; }
        public bool                                 IsPagingEnabled {get; private set; }

        protected void  AddInclude(Expression<Func<TEntity, object>> InIncludeExpression)
        {
            Includes.Add(InIncludeExpression);
        }
        public void     AddOrderBy(Expression<Func<TEntity, object>> InOrderByExpression)
        {
            OrderBy = InOrderByExpression;
        }
        public void     AddOrderByDescending(Expression<Func<TEntity, object>> InOrderByDescendingExpression)
        {
            OrderByDescending = InOrderByDescendingExpression;
        }

        public void     ApplyingPagging(int InTake, int InSkip)
        {
            this.Take = InTake;
            this.Skip = InSkip;
        }

        public Expression<Func<TEntity, bool>> And(Expression<Func<TEntity, bool>> inQuery)
        {
            return Criteria = Criteria == null ? inQuery : Criteria.And(inQuery);
        }

        public Expression<Func<TEntity, bool>> Or(Expression<Func<TEntity, bool>> inQuery)
        {
            return Criteria = Criteria == null ? inQuery : Criteria.Or(inQuery);
        }
    }
}
