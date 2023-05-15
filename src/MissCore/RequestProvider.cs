using System.Collections.Specialized;
using System.Linq.Expressions;
using BotService.Common;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissCore.Collections;

namespace MissCore
{
    public class RequestProvider : IRequestProvider
    {
        class UnitRequest : FormattableString, IUnitRequest
        {
            private readonly string _format;
            private readonly List<object> _arguments;
            private readonly Lazy<ListDictionary> _parameters = new Lazy<ListDictionary>();
            internal UnitRequest(string format, RequestInformation info = default)
            {
                _format = format;
                _arguments = new List<object> { string.Join(',', info.EntityFields), info.Unit, info.Entity, info.Criteria };                
            }
            public string Template
                => Format;
            public override string Format
                => _format;
            public override object[] GetArguments()
                => _arguments.ToArray();
            public override int ArgumentCount
                => _arguments.Count;
            public override object GetArgument(int index)
                => _arguments[index];
            public object this[string key]
            {
                get => _parameters.Value[key];
                set => _parameters.Value[key] = value;
            }

            public override string ToString(IFormatProvider formatProvider)
            {
                if (_arguments.Last() is ICriteria criteria)
                    _arguments[^1] = criteria.ToString(CriteriaFormat.SQL.Make(), default);
                return string.Format(formatProvider, _format, _arguments.ToArray());
            }

            public string ToRequest(RequestFormat format = RequestFormat.JsonAuto)
            {
                return $"{ToString()} {format.TrimSnakes()}";
            }
        }
        public RequestInformation Info<TUnit>(Expression<Predicate<TUnit>> criteria) where TUnit : class
        {
            return BotUnit<TUnit>.GetRequestInfo(null, criteria);
        }

        public IUnitRequest Request<TUnit>(RequestInformation info = default) where TUnit : class
        {
            if (info.Criteria == default)
                return new UnitRequest(Templates.Select, info with { Criteria = BotUnit<BotUnit>.CreateCriteria(cmd => cmd.Entity == Unit<TUnit>.Key) });
            else
                return new UnitRequest(Templates.Select, info ?? Info<BotUnit>(cmd => cmd.Entity == Unit<TUnit>.Key));
        }

        public IUnitRequest RequestUnit<TUnit>(RequestInformation info = default) where TUnit : class
           => new UnitRequest(Templates.Select, info ?? Info<BotUnit>(cmd => cmd.Entity == Unit<TUnit>.Key));
    }

    public abstract class Visitor
    {
        private readonly Expression node;

        protected Visitor(Expression node)
            => this.node = node;

        public abstract void Visit(string prefix);

        public ExpressionType NodeType => node.NodeType;
        public static Visitor CreateFromExpression(Expression node) =>
            node.NodeType switch
            {
                ExpressionType.Parameter => new ParameterVisitor((ParameterExpression)node),
                ExpressionType.Lambda => new LambdaVisitor((LambdaExpression)node),
                ExpressionType.NewArrayInit => new NewArrayVisitor((NewArrayExpression)node),
                _ => throw new NotImplementedException($"Node not processed yet: {node.NodeType}"),
            };
    }

    // Lambda Visitor
    public class LambdaVisitor : Visitor
    {
        private readonly LambdaExpression node;
        public LambdaVisitor(LambdaExpression node) : base(node) => this.node = node;

        public override void Visit(string prefix)
        {
            Console.WriteLine($"{prefix}This expression is a {NodeType} expression type");
            Console.WriteLine($"{prefix}The name of the lambda is {((node.Name == null) ? "<null>" : node.Name)}");
            Console.WriteLine($"{prefix}The return type is {node.ReturnType}");
            Console.WriteLine($"{prefix}The expression has {node.Parameters.Count} argument(s). They are:");
            // Visit each parameter:
            foreach (var argumentExpression in node.Parameters)
            {
                var argumentVisitor = CreateFromExpression(argumentExpression);
                argumentVisitor.Visit(prefix + "\t");
            }
            Console.WriteLine($"{prefix}The expression body is:");
            // Visit the body:
            var bodyVisitor = CreateFromExpression(node.Body);
            bodyVisitor.Visit(prefix + "\t");
        }
    }

    public class ParameterVisitor : Visitor
    {
        private readonly ParameterExpression node;
        public ParameterVisitor(ParameterExpression node) : base(node)
        {
            this.node = node;
        }

        public override void Visit(string prefix)
        {
            Console.WriteLine($"{prefix}This is an {NodeType} expression type");
            Console.WriteLine($"{prefix}Type: {node.Type}, Name: {node.Name}, ByRef: {node.IsByRef}");
        }
    }

    public class NewArrayVisitor : Visitor
    {
        private readonly NewArrayExpression node;
        public NewArrayVisitor(NewArrayExpression node) : base(node)
        {
            this.node = node;
        }

        public override void Visit(string prefix)
        {

            foreach (var item in node.Expressions)
            {
                var argumentVisitor = CreateFromExpression(item);
                argumentVisitor.Visit(prefix + "\t");
            }
            //Console.WriteLine($"{prefix}This is an {NodeType} expression type");
            //Console.WriteLine($"{prefix}Type: {node.Type}, Name: {node.Name}, ByRef: {node.IsByRef}");
        }
    }
}
