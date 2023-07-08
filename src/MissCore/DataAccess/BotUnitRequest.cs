using System.Linq.Expressions;

using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Bot;
using MissBot.Entities.Abstractions;
using MissCore.Bot;
using MissCore.Data;

namespace MissCore.DataAccess
{
	public static class BotUnitRequest
    {
        const string JsonNoWrap = "without_array_wrapper";
        const string RootContent = "root('Content')";
        const string JsonAuto = $" for json auto";
        const string JsonPath = $" for json path";

        public static string Format(this RequestOptions options) => options switch
        {
            RequestOptions.Unknown => $"Unknown format {options}",
            RequestOptions.JsonAuto => JsonAuto,
            RequestOptions.JsonAuto | RequestOptions.Scalar => $"{JsonAuto}, {JsonNoWrap}",
            RequestOptions.JsonAuto | RequestOptions.RootContent => $"{JsonAuto}, {RootContent}",
            RequestOptions.RootContent => $", {RootContent}",
            RequestOptions.JsonPath | RequestOptions.RootContent => $"{JsonPath},  {RootContent}",
            _ => throw new ArgumentException("Bad request options")
        };
        public static BotUnitRequest<TUnit> Create<TUnit>(TUnit unit) where TUnit : BaseBotUnit
            => new BotUnitRequest<TUnit>(unit);
    }
    public class BotUnitRequest<TUnit> : IUnitRequest<TUnit>, IBotUnitRequest where TUnit : BaseBotUnit
    {
        private readonly TUnit _unit;
        private readonly IMetaData _unitData;

        internal BotUnitRequest(TUnit unit)
        {
            _unit = unit;
            _unitData = Unit<TUnit>.ReadMetadata(unit);
            if (_unit is IParameterizedUnit pUnit)
                Params = pUnit.GetParameters().Select(s => new UnitParameter(s, _unitData[s])).Cast<IUnitParameter>().ToArray();//  UnitRequestParameter.Create<string, object>(ref s, _parameters.GetValue(s)));
        }

        public RequestOptions Options { get; set; } = RequestOptions.JsonAuto;
        public IEnumerable<IUnitParameter> Params { get; init; }

        public virtual string GetCommand()
        {
            return $"{_unitData[nameof(BotUnit.Extension)]} {Options.Format()}";
        }
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
            Console.WriteLine($"{prefix}The name of the lambda is {(node.Name == null ? "<null>" : node.Name)}");
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
