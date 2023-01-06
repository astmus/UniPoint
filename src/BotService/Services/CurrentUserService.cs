
using MissBot.Application.Common.Interfaces;

public class CurrentUserService : ICurrentUserService
{
    private readonly IUpdateContextAccessor _httpContextAccessor;

    public CurrentUserService(IUpdateContextAccessor httpContextAccessor = null)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long UserId => _httpContextAccessor?.UserId ?? 0;
}
