﻿using Domain.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace Domain.Specification
{
    // Abstract base class for implementing specifications
    public abstract class SpecificationBase<T> : SpecificationBase, ISpecification<T>
    {

        public virtual List<Expression<Func<T, bool>>> Criterias { get; } = new();
        public List<Expression<Func<T, object>>> Includes { get; } = new();
        public List<string> IncludeStrings { get; } = new();
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }
        public Expression<Func<T, object>> GroupBy { get; private set; }

        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagingEnabled { get; set; }

        // Adds an include expression for related entities
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        // Adds an include string for related entities
        protected void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        // Applies a list of include strings to the specification
        protected void ApplyIncludeList(IEnumerable<string> includes)
        {
            foreach (var include in includes)
            {
                AddInclude(include);
            }
        }

        // Applies a list of include expressions to the specification

        protected void ApplyIncludeList(IEnumerable<Expression<Func<T, object>>> includes)
        {
            foreach (var include in includes)
            {
                AddInclude(include);
            }
        }

        // Applies a filter expression to the specification
        protected ISpecification<T> ApplyFilter(Expression<Func<T, bool>> expr)
        {
            Criterias.Add(expr);

            return this;
        }

        // Applies paging to the specification
        protected void ApplyPaging(int skip, int take)
        {
            if (skip < 1)
            {
                skip = 1;
            }
            Skip = (skip - 1) * take;
            Take = take;
            IsPagingEnabled = true;
        }

        // Applies an order-by expression to the specification
        protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression) =>
            OrderBy = orderByExpression;

        protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression) =>
            OrderByDescending = orderByDescendingExpression;

        protected void ApplyGroupBy(Expression<Func<T, object>> groupByExpression) =>
            GroupBy = groupByExpression;

    }

    // Abstract base class for common specification methods
    public abstract class SpecificationBase
    {
        protected static readonly MethodInfo ToLowerMethod = typeof(string).GetMethod(nameof(string.ToLower), []);
        protected static readonly MethodInfo ContainsMethod = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)]);
    }
}
