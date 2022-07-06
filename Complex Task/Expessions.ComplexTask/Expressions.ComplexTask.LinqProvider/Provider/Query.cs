using System.Collections;
using System.Linq.Expressions;

namespace Expressions.ComplexTask.LinqProvider.Provider;

public class Query<T> : IQueryable<T>
{
    private readonly LinqProvider _provider;

    public Query(Expression expression, LinqProvider provider)
    {
        Expression = expression;
        _provider = provider;
    }

    public Type ElementType => typeof(T);

    public Expression Expression { get; }

    public IQueryProvider Provider => _provider;

    public IEnumerator<T> GetEnumerator()
    {
        return _provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _provider.Execute<IEnumerable>(Expression).GetEnumerator();
    }
}