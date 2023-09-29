using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Entities.Enums;
using System.Linq.Expressions;
using System.Reflection;

namespace PropertyBuildingDemo.Application.Helpers
{
    public class ValidationExpressions
    {
        public static Expression<Func<Property, bool>> CreatePropertyValidationExpression(DefaultQueryFilterArgs filterArgs)
        {
            if (filterArgs == null)
            {
                throw new ArgumentNullException(nameof(filterArgs));
            }

            var parameter = Expression.Parameter(typeof(Property), "f");

            // Initialize finalExpression with a default true expression
            Expression<Func<Property, bool>> finalExpression = f => true;

            foreach (var filterParameter in filterArgs.FilteringParameters)
            {
                // Use reflection to get the property by column name
                var propertyInfo = typeof(Property).GetProperty(filterParameter.TargetField);

                if (propertyInfo == null)
                {
                    throw new ArgumentException($"Property with name '{filterParameter.TargetField}' not found.");
                }

                // Create an expression to access the property by its name
                var propertyAccess = Expression.Property(parameter, propertyInfo);

                // Create an expression to call the specified operator method on the property
                var operatorMethod = GetComparisonOperatorMethod(filterParameter.ComparisionOperator);
                var operatorCall = Expression.Call(propertyAccess, operatorMethod, Expression.Constant(filterParameter.Value));

                // Combine the current filter condition with the existing finalExpression
                if (filterArgs.LogicalOperator == ELogicalOperator.And)
                {
                    finalExpression = Expression.Lambda<Func<Property, bool>>(
                        Expression.AndAlso(Expression.Invoke(finalExpression, parameter), operatorCall), parameter);
                }
                else
                {
                    finalExpression = Expression.Lambda<Func<Property, bool>>(
                        Expression.OrElse(Expression.Invoke(finalExpression, parameter), operatorCall), parameter);
                }
            }

            return finalExpression;
        }

        private static MethodInfo GetComparisonOperatorMethod(EComparisionOperator comparisonOperator)
        {
            switch (comparisonOperator)
            {
                case EComparisionOperator.Contains:
                    return typeof(string).GetMethod("Contains", new[] { typeof(string) });
                // Add other cases and corresponding methods for different operators as needed
                default:
                    throw new ArgumentException($"Unsupported comparison operator: {comparisonOperator}");
            }
        }
    }
}
