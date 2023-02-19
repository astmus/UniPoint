namespace MissBot.Abstractions
{
    public delegate Task HandleDelegate(IHandleContext context);
    public delegate Task HandleContextDelegate(IContext context);
}
