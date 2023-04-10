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
    public record InlineAction : IEntityAction
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

    

    public class InlineKeyBoard : InlineKeyboardMarkup
    {
        public static InlineKeyBoard Create(params string[] items)
            => new InlineKeyBoard(items.Select(s=> (InlineKeyboardButton)new InlineAction(s)));
        public InlineKeyBoard(InlineKeyboardButton inlineKeyboardButton) : base(inlineKeyboardButton)
        {
        }
        public InlineKeyBoard(InlineAction inlineKeyboardButton) : base(inlineKeyboardButton)
        {
        }
        public InlineKeyBoard(IEnumerable<InlineAction> inlineKeyboardButton) : base(inlineKeyboardButton.Select(s=> (InlineKeyboardButton)s))
        {
        }
        public InlineKeyBoard(IEnumerable<InlineKeyboardButton> inlineKeyboardRow) : base(inlineKeyboardRow)
        {
        }
      
        public InlineKeyBoard(IEnumerable<IEnumerable<InlineKeyboardButton>> inlineKeyboard) : base(inlineKeyboard)
        {
        }
    }
}
