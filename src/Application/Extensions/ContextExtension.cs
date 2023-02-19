using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissCore.DataAccess;
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
    internal static IContext<User> GetOrCreateUserContext(this DataContextFactory contextFactory, IUpdateInfo update)
        => contextFactory.GetContext<User>();
    internal static IContext<Chat> GetOrCreateChatContext(this DataContextFactory contextFactory, IUpdateInfo update)
        => contextFactory.GetContext<Chat>();
    internal static IContext<User> GetUserContext(this DataContextFactory contextFactory, IUpdateInfo update)
        => contextFactory.Get<IContext<User>>(update.ToString());    
   

    internal static T CreateContext<T>(this IServiceScope scope) where T : IContext
        => ActivatorUtilities.GetServiceOrCreateInstance<T>(scope.ServiceProvider);
}
