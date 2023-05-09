using System.Runtime.CompilerServices;
using MissBot.Abstractions;
using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions.DataAccess
{
    public record RequestResult(uint? AffectedRows, int? ErrorCode = 0, string Description = default) : Abstractions.Unit;
  //  public record SQLResult<TQuery>(IQueryUnit<TQuery> query, int ErrorCode = 0, string Description = default);
    public record BotActionRequest : BotAction<BotRequest>;
    public record SQLUnit<TUnit>(SQLCommand Command) : IQueryUnit<TUnit>
    {
        public static readonly SQLUnit<TUnit> Instance = new SQLUnit<TUnit>(Unit.Empty);

        public async Task<TResult> GetResponseAsync<TResult>(IRepository repository, CancellationToken cancel = default) where TResult : TUnit
        => await repository.HandleCommandAsync<TResult>(this.Command, cancel);

        public async Task<ICollection<TResult>> GetResponseItemsAsync<TResult>(IRepository repository, CancellationToken cancel = default) where TResult : TUnit
            => await repository.HandleCommandAsync<Unit<TResult>.Collection>(this.Command, cancel);
        

        public RequestResult Result { get; set; } 
    }
    public record BotRequest : BotEntity, IRepositoryCommand
    {
        public BotRequest(string v = default)
        {
            Command = v;
  
        }

        public BotRequest(string v, SQLType sQLType)
        {
            Command = v;
           /// Enumerable = enumerable;
            SQLType = sQLType;
        }

        public RequestResult Result { get; set; }
  
        public string Command { get; }
        public IEnumerable<string> Enumerable { get; }
        public SQLType SQLType { get; }
        public override string Entity => nameof(BotRequest);
    }


}
