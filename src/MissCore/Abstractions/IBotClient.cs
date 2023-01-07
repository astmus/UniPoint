namespace MissCore.Abstractions
{
    public interface IBotClient
    {
        IBotClient SetupContext(IHandleContext context);
    }
}

