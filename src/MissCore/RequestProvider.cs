using System.Collections.Specialized;
using System.Linq.Expressions;
using MissBot.Abstractions;
using MissBot.Abstractions.DataContext;
using MissBot.Abstractions.Entities;
using MissCore.Collections;

namespace MissCore
{
    public class BotUnitRequest<TUnit> : FormattableString, IUnitRequest<TUnit>
    {
        private string _format;
        private readonly List<object> _arguments;
        private readonly Lazy<ListDictionary> _parameters = new Lazy<ListDictionary>();
        internal BotUnitRequest(string format, IEnumerable<object> info = default)
        {
            _format = format;
            _arguments = new List<object>
                {
                    Unit<TUnit>.Key
                };
            if (info != null)
                _arguments.AddRange(info);
        }

        internal static BotUnitRequest<TUnit> Create(string format, params object[] args)
            => new BotUnitRequest<TUnit>(format, args);

        public override string Format
            => _format;
        public override object[] GetArguments()
            => _arguments.ToArray();
        public override int ArgumentCount
            => _arguments.Count;

        public RequestOptions RequestOptions { get; set; } = RequestOptions.JsonAuto;

        public override object GetArgument(int index)
                => _arguments[index];
        public object this[string key]
        {
            get => _parameters.Value[key];
            set => _parameters.Value[key] = value;
        }

        public override string ToString(IFormatProvider formatProvider)
        {
            return string.Format(formatProvider, _format, _arguments.ToArray());
        }

        public virtual string GetCommand(RequestOptions options = RequestOptions.JsonAuto)
        {
            var opt = RequestOptions == RequestOptions.Unknown ? options.TrimSnakes() : RequestOptions.TrimSnakes();
            return $"{base.ToString()} {opt}";
        }
    }

    public class RequestProvider : IRequestProvider
    {
        private readonly IBotUnitFormatProvider provider;
        public RequestProvider(IBotUnitFormatProvider formatProvider)
        {
            provider = formatProvider;
        }

        public IUnitRequest<TUnit> ReadRequest<TUnit>(Expression<Predicate<TUnit>> criteria) where TUnit : Unit
            => BotUnitRequest<TUnit>.Create(Templates.ReadAllByCriteria, BotUnit<TUnit>.CreateCriteria(criteria));

        public IUnitRequest<TUnit> FindRequest<TUnit>(string search, uint skip = 0, uint take = 0) where TUnit : Unit
            => BotUnitRequest<TUnit>.Create(Templates.Search, search, skip, take);

        public IUnitRequest<TUnit> FromRaw<TUnit>(string raw)
            => BotUnitRequest<TUnit>.Create(raw);
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
