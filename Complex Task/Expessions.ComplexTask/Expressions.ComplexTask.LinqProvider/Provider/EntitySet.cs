using System.Collections;
using System.Linq.Expressions;
using Expressions.ComplexTask.LinqProvider.Entities;

namespace Expressions.ComplexTask.LinqProvider.Provider;

public class EntitySet<TEntity> : IQueryable<TEntity> where TEntity : BaseEntity
{
    public Type ElementType => typeof(TEntity);
    public Expression Expression => Expression.Constant(this);
    public IQueryProvider Provider { get; }

    public EntitySet(string connectionString)
    {
        Provider = new LinqProvider(new ExpressionToSqlTranslator(), new DbProvider(connectionString));
    }

    public IEnumerator<TEntity> GetEnumerator()
        => Provider.Execute<IEnumerable<TEntity>>(Expression).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => Provider.Execute<IEnumerable>(Expression).GetEnumerator();
}