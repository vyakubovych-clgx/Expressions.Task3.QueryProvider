using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToFtsRequestTranslator : ExpressionVisitor
    {
        readonly StringBuilder _resultStringBuilder;

        public ExpressionToFtsRequestTranslator()
        {
            _resultStringBuilder = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            Visit(exp);

            return _resultStringBuilder.ToString();
        }

        #region protected methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                Visit(predicate);

                return node;
            }

            if (node.Method.DeclaringType == typeof(string))
            {
                TranslateToStringBuilder(node.Object, node.Arguments[0], node.Method.Name);
                return node;
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    if (node.Left.NodeType == ExpressionType.MemberAccess)
                        TranslateToStringBuilder(node.Left, node.Right);
                    else
                        TranslateToStringBuilder(node.Right, node.Left);

                    break;
                case ExpressionType.AndAlso:
                    Visit(node.Left);
                    _resultStringBuilder.Append(";");
                    Visit(node.Right);

                    break;

                default:
                    throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _resultStringBuilder.Append(node.Member.Name);

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _resultStringBuilder.Append(node.Value);

            return node;
        }

        #endregion

        private void TranslateToStringBuilder(Expression member, Expression constant, string method = null)
        {
            Visit(member);
            _resultStringBuilder.Append(":");
            _resultStringBuilder.Append("(");
            AppendAsteriskForMethod(method, "Contains", "EndsWith");
            Visit(constant);
            AppendAsteriskForMethod(method, "Contains", "StartsWith");
            _resultStringBuilder.Append(")");
        }

        private void AppendAsteriskForMethod(string method, params string[] methodsToAddAsterisk)
        {
            if (methodsToAddAsterisk.Contains(method))
                _resultStringBuilder.Append("*");
        }
    }
}
