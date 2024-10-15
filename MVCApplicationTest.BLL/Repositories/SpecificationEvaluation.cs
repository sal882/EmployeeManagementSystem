using Microsoft.EntityFrameworkCore;
using MVCApplicationTest.BLL.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCApplicationTest.BLL.Repositories
{
    public static class SpecificationEvaluation<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> entryPoint, ISpecification<T> spec)
        {
            var query = entryPoint;

            if(spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            query = spec.Includes.Aggregate
                (query, (currentQuery, IncludeExpression) => currentQuery.Include(IncludeExpression));


            return query;
        }
    }
}
