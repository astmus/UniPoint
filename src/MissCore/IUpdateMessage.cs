using Telegram.Bot.Types;

namespace MissCore
{
    public interface IUpdateCallbackQuery
    {
        CallbackQuery CallbackQuery { get; }
    }
    public interface IUpdateChannelPost
    {
        Message ChannelPost { get; }
    }
    public interface IUpdateEditedChannelPost
    {
        Message EditedChannelPost { get; }
    }
    public interface IUpdateEditedMessage
    {
        Message EditedMessage { get; }
    }
    public interface IUpdateInlineQuery
    {
        InlineQuery InlineQuery { get; }
    }
    public interface IUpdateMessage
    {
        Message Message { get; }
    }
    //public interface IUpdateInlineQuery
    //{
    //    CallbackQuery? CallbackQuery { get; set; }
    //    Message? ChannelPost { get; set; }
    //    ChatJoinRequest? ChatJoinRequest { get; set; }
    //    ChatMemberUpdated? ChatMember { get; set; }
    //    ChosenInlineResult? ChosenInlineResult { get; set; }
    //    Message? EditedChannelPost { get; set; }
    //    Message? EditedMessage { get; set; }
    //    int Id { get; set; }
    //    InlineQuery? InlineQuery { get; set; }
    //    Message? Message { get; set; }
    //    ChatMemberUpdated? MyChatMember { get; set; }
    //    Poll? Poll { get; set; }
    //    PollAnswer? PollAnswer { get; set; }
    //    PreCheckoutQuery? PreCheckoutQuery { get; set; }
    //    ShippingQuery? ShippingQuery { get; set; }
    //    UpdateType Type { get; }
    //}
}
