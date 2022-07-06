using System.Linq.Expressions;
using System.Text;
using Expressions.ComplexTask.LinqProvider.Entities;

namespace Expressions.ComplexTask.LinqProvider.Provider;

public class ExpressionToSqlTranslator : ExpressionVisitor
{
    private readonly StringBuilder _resultSqlQueryBuilder = new("SELECT * FROM ");

    public string Translate(Expression expression)
    {
        AppendString($"{GetEntityType(expression)}s");
        AppendSpace();
        Visit(expression);
        return _resultSqlQueryBuilder.ToString().Trim();
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (node.Method.DeclaringType == typeof(Queryable)
            && node.Method.Name == "Where")
        {
            AppendString("WHERE");
            AppendSpace();
            var predicate = node.Arguments[1];
            Visit(predicate);
            return node;
        }

        return base.VisitMethodCall(node);
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        Visit(node.Left);
        AppendString(node.NodeType switch
        {
            ExpressionType.Equal => "=",
            ExpressionType.NotEqual => "<>",
            ExpressionType.GreaterThan => ">",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LessThan => "<",
            ExpressionType.LessThanOrEqual => "<=",
            ExpressionType.AndAlso => "AND",
            _ => string.Empty
        });
        AppendSpace();
        Visit(node.Right);
        return node;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        AppendString(node.Member.Name);
        AppendSpace();
        return base.VisitMember(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (!node.Type.Name.Contains("EntitySet"))
        {
            AppendApostropheIfString(node);
            AppendString($"{node.Value}");
            AppendApostropheIfString(node);
            AppendSpace();
        }

        return node;
    }

    private static string GetEntityType(Expression expression)
    {
        var type = expression.Type;
        while (true)
        {
            if (type.IsSubclassOf(typeof(BaseEntity))) return type.Name;
            type = type.GenericTypeArguments[0];
        }
    }

    private void AppendString(string s)
    {
        _resultSqlQueryBuilder.Append(s);
    }

    private void AppendApostropheIfString(ConstantExpression node)
    {
        if (node.Type == typeof(string))
            AppendString("'");
    }

    private void AppendSpace()
    {
        AppendString(" ");
    }
}