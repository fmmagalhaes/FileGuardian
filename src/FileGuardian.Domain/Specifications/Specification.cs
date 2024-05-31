using System.Linq.Expressions;

namespace FileGuardian.Domain.Specifications;

public abstract class Specification<T>
{
    public static readonly Specification<T> All = new IdentitySpecification<T>();

    public Specification<T> And(Specification<T> specification) => new AndSpecification<T>(this, specification);

    public abstract Expression<Func<T, bool>> ToExpression();
}

internal sealed class AndSpecification<T>(Specification<T> left, Specification<T> right) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpression = left.ToExpression();
        var rightExpression = right.ToExpression();

        // Create a new parameter expression to be used as the common parameter
        var paramExpr = Expression.Parameter(typeof(T));

        // Invoke both expressions with the new parameter
        var andExpression = Expression.AndAlso(
            Expression.Invoke(leftExpression, paramExpr),
            Expression.Invoke(rightExpression, paramExpr));

        return Expression.Lambda<Func<T, bool>>(andExpression, paramExpr);
    }
}

internal sealed class IdentitySpecification<T> : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        return x => true;
    }
}
