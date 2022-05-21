using System;
using System.Linq.Expressions;

namespace Buform
{
    public static class ExpressionExtensions
    {
        public static string GetMemberName<TValue>(this Expression<Func<TValue>>? expression)
        {
            return expression?.Body switch
            {
                MemberExpression item => item.Member.Name,
                UnaryExpression { Operand: MemberExpression item } => item.Member.Name,
                _ => throw new ArgumentOutOfRangeException(expression?.GetType().ToString(), null, null)
            };
        }
    }
}