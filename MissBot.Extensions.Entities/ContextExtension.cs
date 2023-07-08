using Microsoft.Extensions.DependencyInjection;


namespace MissBot.Extensions;
public static class ContextExtension
{
    internal static IServiceScope CreateUserScope(this IServiceScopeFactory factory)
        => factory.CreateScope();

    internal static IServiceScope CreateChatScope(this IServiceScope factory)
        => factory.ServiceProvider.CreateScope();

    internal static IServiceScope CreateMessageScope(this IServiceScope factory)
        => factory.ServiceProvider.CreateScope();

    //internal static T CreateContext<T>(this IServiceScope scope) where T : IContext
    //    => ActivatorUtilities.GetServiceOrCreateInstance<T>(scope.ServiceProvider);
}
