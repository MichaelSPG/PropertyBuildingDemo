using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Entities.Enums;
using System.Linq.Expressions;
using System.Reflection;

namespace PropertyBuildingDemo.Application.Helpers
{
    public class ValidationExpressions
    {

        public static List<Expression<Func<Property, bool>>> BuildPropertyFilterExpressions(DefaultQueryFilterArgs defaultQueryFilterArgs)
        {
            var parameter = Expression.Parameter(typeof(Property), "entity");
            var filterExpressions = new List<Expression<Func<Property, bool>>>();

            foreach (var filterParam in defaultQueryFilterArgs.FilteringParameters)
            {
                var propertyAccess = GetPropertyAccess< Property>(parameter, filterParam.TargetField);
                var convertedValue = ConvertValue(filterParam.Value, propertyAccess.Type);
                var filterExpression = CreateFilterExpression<Property>(defaultQueryFilterArgs);

                filterExpressions.Add(Expression.Lambda<Func<Property, bool>>(filterExpression, parameter));
            }

            if (filterExpressions.Count == 0)
            {
                return new List<Expression<Func<Property, bool>>> { _ => true };
            }

            return filterExpressions;
        }
        private static Expression GetPropertyAccess<TEntity>(ParameterExpression parameter, string targetField)
        {
            // Get the PropertyInfo for the specified targetField.
            var propertyInfo = typeof(TEntity).GetProperty(targetField);

            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property with name '{targetField}' not found on type '{typeof(TEntity).Name}'.");
            }

            // Create an expression to access the property.
            return Expression.Property(parameter, propertyInfo);
        }

        public static Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>(DefaultQueryFilterArgs filterArgs)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "entity");
            var propertyValidationExpression = CreatePropertyValidationExpression(filterArgs);

            if (filterArgs.LogicalOperator == ELogicalOperator.And)
            {
                return Expression.Lambda<Func<TEntity, bool>>(
                    Expression.AndAlso(propertyValidationExpression.Body, Expression.Invoke(propertyValidationExpression, parameter)),
                    parameter
                );
            }
            else
            {
                return Expression.Lambda<Func<TEntity, bool>>(
                    Expression.OrElse(propertyValidationExpression.Body, Expression.Invoke(propertyValidationExpression, parameter)),
                    parameter
                );
            }
        }

        /// <summary>
        /// Creates a validation expression for filtering properties based on the given filter arguments.
        /// </summary>
        /// <param name="filterArgs">The filter arguments containing filtering parameters.</param>
        /// <returns>An expression representing the validation criteria.</returns>
        public static Expression<Func<Property, bool>> CreatePropertyValidationExpression(DefaultQueryFilterArgs filterArgs)
        {
            // Check for null filterArgs and throw an ArgumentNullException if null
            if (filterArgs == null)
            {
                throw new ArgumentNullException(nameof(filterArgs));
            }

            // Create a parameter expression representing the Property entity
            var parameter = Expression.Parameter(typeof(Property), "f");

            // Initialize finalExpression with a default true expression
            Expression<Func<Property, bool>> finalExpression = f => true;

            foreach (var filterParameter in filterArgs.FilteringParameters)
            {
                // Use reflection to get the property by column name
                var propertyInfo = typeof(Property).GetProperty(filterParameter.TargetField);

                if (propertyInfo == null)
                {
                    // Throw an ArgumentException if the property with the specified name is not found
                    throw new ArgumentException($"Column with name '{filterParameter.TargetField}' not found.");
                }

                // Create an expression to access the property by its name
                var propertyAccess = Expression.Property(parameter, propertyInfo);

                // Convert the filter parameter's value to the property's type (if necessary)
                var convertedValue = Expression.Constant(ConvertValue(filterParameter.Value, propertyInfo.PropertyType));

                // Create an expression to call the specified operator method on the property
                var operatorMethod = GetComparisonOperatorMethod(filterParameter.ComparisionOperator, propertyInfo.PropertyType);
                var operatorCall = Expression.Call(propertyAccess, operatorMethod, convertedValue);

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

        /// <summary>
        /// Converts the given value to the specified target type, handling type conversions as needed.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <param name="targetType">The target type to which the value should be converted.</param>
        /// <returns>The converted value.</returns>
        private static object ConvertValue(object value, Type targetType)
        {
            // Check if the value is null, and return null if it is
            if (value == null)
            {
                return null;
            }

            // Check if the target type is DateTime and the value is a string
            if (targetType == typeof(DateTime) && value is string dateString)
            {
                // Attempt to convert the string to DateTime
                if (DateTime.TryParse(dateString, out DateTime dateValue))
                {
                    return dateValue;
                }
            }
            // Check if the value is already of the target type
            else if (targetType.IsInstanceOfType(value))
            {
                return value;
            }
            // Check if the value implements IConvertible for other conversions
            else if (value is IConvertible convertible)
            {
                // Use IConvertible to convert the value to the target type
                return convertible.ToType(targetType, null);
            }

            // If no conversion is possible, return the original value
            return value;
        }

        /// <summary>
        /// Retrieves the MethodInfo for the specified comparison operator.
        /// </summary>
        /// <param name="comparisonOperator">The comparison operator.</param>
        /// <returns>The MethodInfo for the specified operator.</returns>
        //private static MethodInfo GetComparisonOperatorMethod(EComparisionOperator comparisonOperator)
        //{
        //    switch (comparisonOperator)
        //    {
        //        case EComparisionOperator.Contains:
        //        case EComparisionOperator.NotContains:
        //            // For string.Contains, use the Contains method of the string type
        //            return typeof(string).GetMethod("Contains");
        //        case EComparisionOperator.LessThan:
        //            // For LessThan, use the LessThan method of the Expression type
        //            return typeof(Expression).GetMethod("LessThan", new[] { typeof(Expression), typeof(Expression) });
        //        case EComparisionOperator.LessThanEqual:
        //            // For LessThanOrEqual, use the LessThanOrEqual method of the Expression type
        //            return typeof(Expression).GetMethod("LessThanOrEqual", new[] { typeof(Expression), typeof(Expression) });
        //        case EComparisionOperator.GreaterThan:
        //            // For GreaterThan, use the GreaterThan method of the Expression type
        //            return typeof(Expression).GetMethod("GreaterThan", new[] { typeof(Expression), typeof(Expression) });
        //        case EComparisionOperator.GreaterThanEqual:
        //            // For GreaterThanOrEqual, use the GreaterThanOrEqual method of the Expression type
        //            return typeof(Expression).GetMethod("GreaterThanOrEqual", new[] { typeof(Expression), typeof(Expression) });
        //        case EComparisionOperator.NotEqual:
        //            // For NotEqual, use the NotEqual method of the Expression type
        //            return typeof(Expression).GetMethod("NotEqual", new[] { typeof(Expression), typeof(Expression) });
        //        case EComparisionOperator.Equal:
        //            // For Equal, use the Equal method of the Expression type
        //            return typeof(Expression).GetMethod("Equal", new[] { typeof(Expression), typeof(Expression) });
        //        case EComparisionOperator.StartsWith:
        //            // For StartsWith, use the StartsWith method of the string type
        //            return typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
        //        case EComparisionOperator.EndsWith:
        //            // For EndsWith, use the EndsWith method of the string type
        //            return typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
        //        default:
        //            // Throw an exception for an invalid comparison operator
        //            throw new ArgumentException($"Invalid comparison operator: {comparisonOperator}");
        //    }
        //}

        private static MethodInfo GetComparisonOperatorMethod(EComparisionOperator comparisonOperator, Type propertyType)
        {
            switch (comparisonOperator)
            {
                case EComparisionOperator.Contains:
                case EComparisionOperator.NotContains:
                    // For string.Contains, use the Contains method of the string type
                    return typeof(string).GetMethod("Contains", new[] { typeof(string) });
                case EComparisionOperator.LessThan:
                    // For LessThan, use the LessThan method of the Expression type
                    return typeof(Expression).GetMethod("LessThan", new[] { propertyType, propertyType });
                case EComparisionOperator.LessThanEqual:
                    // For LessThanOrEqual, use the LessThanOrEqual method of the Expression type
                    return typeof(Expression).GetMethod("LessThanOrEqual", new[] { propertyType, propertyType });
                case EComparisionOperator.GreaterThan:
                    // For GreaterThan, use the GreaterThan method of the Expression type
                    return typeof(Expression).GetMethod("GreaterThan", new[] { propertyType, propertyType });
                case EComparisionOperator.GreaterThanEqual:
                    // For GreaterThanOrEqual, use the GreaterThanOrEqual method of the Expression type
                    return typeof(Expression).GetMethod("GreaterThanOrEqual", new[] { propertyType, propertyType });
                case EComparisionOperator.NotEqual:
                    // For NotEqual, use the NotEqual method of the Expression type
                    return typeof(Expression).GetMethod("NotEqual", new[] { propertyType, propertyType });
                case EComparisionOperator.Equal:
                    // For Equal, use the Equals method of the Object type
                    return typeof(object).GetMethod("Equals", new[] { typeof(object), typeof(object) });
                case EComparisionOperator.StartsWith:
                    // For StartsWith, use the StartsWith method of the string type
                    return typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                case EComparisionOperator.EndsWith:
                    // For EndsWith, use the EndsWith method of the string type
                    return typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                default:
                    // Throw an exception for an invalid comparison operator
                    throw new ArgumentException($"Invalid comparison operator: {comparisonOperator}");
            }
        }
    }
}
