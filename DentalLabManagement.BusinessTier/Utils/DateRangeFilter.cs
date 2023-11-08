using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Utils
{
    public static class DateRangeFilter
    {
        public static Expression<Func<T, bool>> BuildFilter<T>(
            DateTime? from,
            DateTime? to,
            Expression<Func<T, DateTime>> dateSelector)
        {
            var parameter = dateSelector.Parameters.Single();
            Expression<Func<T, bool>> filter = p => true;

            if (from.HasValue)
            {
                var fromExpression = Expression.GreaterThanOrEqual(dateSelector.Body, Expression.Constant(from.Value));
                filter = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(filter.Body, fromExpression), parameter);
            }

            if (to.HasValue)
            {
                var toExpression = Expression.LessThanOrEqual(dateSelector.Body, Expression.Constant(to.Value));
                filter = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(filter.Body, toExpression), parameter);
            }

            return filter;
        }
    }
}
