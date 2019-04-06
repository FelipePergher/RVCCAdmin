using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LigaCancer.Code.Interface
{

    public interface ISpecification<T>
    {
        List<Expression<Func<T, object>>> Includes { get; }

        List<string> IncludeStrings { get; }
    }
}

