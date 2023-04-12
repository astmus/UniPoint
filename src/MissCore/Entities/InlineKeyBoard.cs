using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissBot.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MissCore.Entities
{
    public record InlineAction : Unit<InlineAction>, IEntityAction
    {
        public InlineAction(string data)
        {
            var items = data.Split(";");
            Text = items[0];
            Data = items.Skip(1)?.FirstOrDefault();
        }

        public record Result(string state) : BotEntity<InlineAction>.Response
        {
            public override ChatId ChatId { get; }
        }

        public string Text { get; set; }
        public string Data { get; set; }

        public static implicit operator InlineAction(string data) =>
            new InlineAction(data);
        public static implicit operator InlineKeyboardButton(InlineAction s) =>
            InlineKeyboardButton.WithCallbackData(s.Text, s.Data);
    }



    public class InlineKeyBoard
    {
        public static InlineKeyBoard Init(InlineAction action = default)
            => new InlineKeyBoard().Append(action);
        public static InlineKeyBoard Append(ref InlineKeyBoard board, InlineAction action)
        => board !=null ? board.Append(action) : Init(action);
        public InlineKeyboardMarkup GetKeyboard
            => new InlineKeyboardMarkup(actions);
        List<InlineKeyboardButton> actions = new List<InlineKeyboardButton>();
        

        public InlineKeyBoard Append(InlineAction action)
        {
            actions.Add(action);
            return this;
        }        
    }
}
