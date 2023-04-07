using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using Telegram.Bot.Types;

public static class ContextExtension
{
    public static string UpdateId(this IUpdateInfo update)
        => $"{nameof(update.UpdateId)}{update.UpdateId}";

    internal static IServiceScope CreateUserScope(this IServiceScopeFactory factory)
        => factory.CreateScope();
    internal static IServiceScope CreateChatScope(this IServiceScope factory)
        => factory.ServiceProvider.CreateScope();
    internal static IServiceScope CreateMessageScope(this IServiceScope factory)
        => factory.ServiceProvider.CreateScope();
    internal static IContext<User> GetOrCreateUserContext(this MissCore.DataAccess.DataContextFactory contextFactory, IUpdateInfo update)
        => contextFactory.GetContext<User>();
    internal static IContext<Telegram.Bot.Types.Chat> GetOrCreateChatContext(this MissCore.DataAccess.DataContextFactory contextFactory, IUpdateInfo update)
        => contextFactory.GetContext<Telegram.Bot.Types.Chat>();
    internal static IContext<User> GetUserContext(this MissCore.DataAccess.DataContextFactory contextFactory, IUpdateInfo update)
        => contextFactory.Get<IContext<User>>(update.ToString());    
   

    internal static T CreateContext<T>(this IServiceScope scope) where T : IContext
        => ActivatorUtilities.GetServiceOrCreateInstance<T>(scope.ServiceProvider);
}
