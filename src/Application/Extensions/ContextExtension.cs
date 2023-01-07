using Microsoft.Extensions.DependencyInjection;
using MissBot;
using MissCore.Abstractions;
using MissCore.DataAccess;
using Telegram.Bot.Types;

public static class ContextExtension
{
    public static string UpdateId(this IUpdateInfo update)
        => $"{nameof(update.UpdateId)}{update.UpdateId}";
    public static string UserId(this IUpdateInfo update)
        => $"{nameof(update.UserId)}{update.UserId}";
    public static string ChatId(this IUpdateInfo update)
        => $"{nameof(update.ChatId)}{update.ChatId}";

    internal static IServiceScope CreateUserScope(this IServiceScopeFactory factory)
        => factory.CreateScope();
    internal static IServiceScope CreateChatScope(this IServiceScope factory)
        => factory.ServiceProvider.CreateScope();
    internal static IServiceScope CreateMessageScope(this IServiceScope factory)
        => factory.ServiceProvider.CreateScope();
    internal static IContext<User> GetOrCreateUserContext(this DataContextFactory contextFactory, IUpdateInfo update)
        => contextFactory.GetContext<User>();
    internal static IContext<Chat> GetOrCreateChatContext(this DataContextFactory contextFactory, IUpdateInfo update)
        => contextFactory.GetContext<Chat>();
    internal static IContext<User> GetUserContext(this DataContextFactory contextFactory, IUpdateInfo update)
        => contextFactory.Get<IContext<User>>(update.ToString());    
    
    internal static IServiceScope GetUserScope(this DataContextFactory contextFactory, IUpdateInfo update)
        => contextFactory.Get<IServiceScope>(update.UserId());
    internal static IServiceScope GetChatScope(this DataContextFactory userContext, IUpdateInfo update)
        => userContext.Get<IServiceScope>(update.ChatId());
    internal static IServiceScope GetUpdateScope(this DataContextFactory messageContext, IUpdateInfo update)
        => messageContext.Get<IServiceScope>(update.GetId());
    internal static T CreateContext<T>(this IServiceScope scope) where T : IContext
        => ActivatorUtilities.GetServiceOrCreateInstance<T>(scope.ServiceProvider);
}
