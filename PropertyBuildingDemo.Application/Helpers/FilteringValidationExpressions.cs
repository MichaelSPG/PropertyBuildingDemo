using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Entities.Enums;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;
using PropertyBuildingDemo.Domain.Interfaces;

namespace PropertyBuildingDemo.Application.Helpers
{
    /// <summary>
    /// Provides methods for generating filtering expressions based on filtering parameters.
    /// </summary>
    public class FilteringValidationExpressions
    {
        /// <summary>
        /// Gets the comparison operator method based on the provided filtering parameters.
        /// </summary>
        /// <param name="filteringParameters">The filtering parameters.</param>
        /// <param name="property">The expression representing the property to filter on.</param>
        /// <returns>An expression representing the comparison between the property and the filtering value.</returns>
        private static Expression GetComparisonOperatorMethod(FilteringParameters filteringParameters, Expression property)
        {
            Expression constant;
            Expression comparison = null;
            Expression convertedProperty = property;
            if (filteringParameters.ComparisionOperator != EComparisionOperator.LessThan &&
                filteringParameters.ComparisionOperator != EComparisionOperator.LessThanEqual &&
                filteringParameters.ComparisionOperator != EComparisionOperator.GreaterThan &&
                filteringParameters.ComparisionOperator != EComparisionOperator.GreaterThanEqual && property.Type != typeof(string))
            {
                MethodInfo toStringMethod = typeof(object).GetMethod("ToString", Type.EmptyTypes);
                convertedProperty = Expression.Call(property, toStringMethod);
                constant = Expression.Constant(Convert.ChangeType(filteringParameters.Value, typeof(string)));
            }
            else
            {
                // Otherwise, use filter.Value as is
                constant = Expression.Constant(Convert.ChangeType(filteringParameters.Value, property.Type));
            }
            

            switch (filteringParameters.ComparisionOperator)
            {
                case EComparisionOperator.Contains:
                    comparison = Expression.Call(
                        convertedProperty,
                        typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                        constant);
                    break;

                case EComparisionOperator.NotContains:
                    comparison = Expression.Not(Expression.Call(
                        convertedProperty,
                        typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                        constant));
                    break;


                case EComparisionOperator.LessThan:
                    comparison = Expression.LessThan(convertedProperty, constant);
                    break;

                case EComparisionOperator.LessThanEqual:
                    comparison = Expression.LessThanOrEqual(convertedProperty, constant);
                    break;

                case EComparisionOperator.GreaterThan:
                    comparison = Expression.GreaterThan(convertedProperty, constant);
                    break;

                case EComparisionOperator.GreaterThanEqual:
                    comparison = Expression.GreaterThanOrEqual(convertedProperty, constant);
                    break;

                case EComparisionOperator.NotEqual:
                    comparison = Expression.Not(Expression.Call(
                        convertedProperty,
                        typeof(string).GetMethod("Equals", new[] { typeof(string) }),
                        constant));
                    break;

                case EComparisionOperator.Equal:
                    comparison = Expression.Call(
                        convertedProperty,
                        typeof(string).GetMethod("Equals", new[] { typeof(string) }),
                        constant);
                    break;

                case EComparisionOperator.StartsWith:
                    comparison = Expression.Call(
                        convertedProperty,
                        typeof(string).GetMethod("StartsWith", new[] { typeof(string) }),
                        constant);
                    break;

                case EComparisionOperator.EndsWith:
                    comparison = Expression.Call(
                        convertedProperty,
                        typeof(string).GetMethod("EndsWith", new[] { typeof(string) }),
                        constant);
                    break;

                default:
                    throw new NotSupportedException($"Comparison operator {filteringParameters.ComparisionOperator} is not supported.");
            }

            return comparison;
        }

        /// <summary>
        /// Generates a composite expression for filtering entities based on filtering parameters.
        /// </summary>
        /// <typeparam name="T">The type of entity being filtered.</typeparam>
        /// <param name="inFilterArgs">The filtering arguments containing filtering parameters.</param>
        /// <returns>An expression representing the combined filtering criteria.</returns>
        public static Expression<Func<T, bool>> GetSpecificationsFromFilters<T>(DefaultQueryFilterArgs inFilterArgs, ISystemLogger logger) where T : BaseEntityDb
        {
            var allProps = typeof(T).GetProperties().ToList();
            if (inFilterArgs.FilteringParameters == null || !inFilterArgs.FilteringParameters.Any())
            {
                return x => true;
            }

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression combinedCriteria = null;

            foreach (var filter in inFilterArgs.FilteringParameters)
            {
                try
                {
                    if (filter == null || string.IsNullOrWhiteSpace(filter.TargetField))
                    {
                        continue;
                    }
                    Expression columnExpression = parameter;
                    var columnParts = filter.TargetField.Split('.');
                    foreach (var part in columnParts)
                    {
                        var property = columnExpression.Type.GetProperty(part);

                        if (property == null)
                        {
                            throw new ArgumentException($"Property {part} not found on type {columnExpression.Type.Name}");
                        }

                        columnExpression = Expression.Property(columnExpression, property);
                    }
                    

                    
                    //var comparison = Expression.Equal(property, constant);
                    var comparison =
                        GetComparisonOperatorMethod(filter, columnExpression);

                    combinedCriteria = combinedCriteria == null ? comparison : Expression.AndAlso(combinedCriteria, comparison);
                }
                catch (Exception ex)
                {
                    // Element not added to filtering process
                    logger.LogExceptionMessage(ELoggingLevel.Warn, $"Column '{filter.TargetField}' is not mapped/added to specifications", ex);
                }

            }
            return combinedCriteria != null ? Expression.Lambda<Func<T, bool>>(combinedCriteria, parameter) :
                // If there are no filter criteria, return a default true expression
                x => true;
        }
    }
}
