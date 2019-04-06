using LigaCancer.Code.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LigaCancer.Code
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification(params Expression<Func<T, object>>[] includes)
        {
            Includes.AddRange(includes);
        }

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        public List<string> IncludeStrings { get; } = new List<string>();

    }
}
