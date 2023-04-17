using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Telegram.Bot.Requests;
using MissCore.Entities;
using System.Runtime.CompilerServices;
using Telegram.Bot.Types;
using MediatR;
using MissBot.Abstractions;

namespace MissCore.Bot
{
    static public class BotCore
    {
        public record Cmd(string cmd = default)
        {
         
        }
        public record Cmd<TEntity> : Cmd
        {
            static readonly string EntityName;
   
            static Cmd()
            {
                Command = Query = Unit<Cmd<TEntity>>.Sample;
                EntityName = Unit<TEntity>.EntityTable;
            }
            
            public static Cmd<TInEntity> BotCommand<TInEntity>(string cmdId) where TInEntity : BotCommand
                => new Cmd<TInEntity>() { cmd = cmdId  };
            
            public static Cmd Command { get; internal set; }
            public static Cmd<TEntity> Query { get; internal set; }
            public record CmdIdent(TEntity param, Predicate<TEntity> filter = default) : Cmd<TEntity>
            {
                public static Cmd<TEntity> ByCriteria(TEntity sample, Predicate<TEntity> filter)
                    => new CmdIdent(sample, filter);

                public Cmd<TEntity> Predict(TEntity param)
                    => this with { param = param, cmd = cmd };// new SqlArged<TArgsObj>(param, Cmd);
            }
            public record CmdArg<TArgsObj>(TEntity param) : Cmd<TEntity> where TArgsObj : class
            {
                public Cmd<TEntity> Predict(TEntity param) 
                    => this with { param = param, cmd = cmd };// new SqlArged<TArgsObj>(param, Cmd);
            }           
        }
        public static Cmd<TEntity> RequestAny<TEntity>([CallerMemberName] string sql = default)
            => Cmd<TEntity>.Query with { cmd = sql };
        public record SearchRequest(string Cmd) : Cmd(Cmd)
        {
            public Cmd<TEntity> Create<TEntity>(params object[] args)
                => Cmd<TEntity>.Query with { cmd = (string.Format(Cmd, args)) };
            public static SearchRequest Request { get; internal set; } = new SearchRequest("");
        }

        public record CmdRequest<TParam> : Cmd<TParam> where TParam:class
        {
            protected static TParam sample;
            public TParam Param { get; init; }
            public virtual new Cmd<TParam> Query { get; }
            public string EntityName { get; protected set; }
            static CmdRequest()
            {
                sample ??= Unit<TParam>.Sample;
            }
            public Cmd<TParam> Predict<TEntity>(TParam param) where TEntity : class
                => this with { Param = param, cmd = cmd };// new CmdArg<TEntity>(param).Predict(param);
    }
    }
}
