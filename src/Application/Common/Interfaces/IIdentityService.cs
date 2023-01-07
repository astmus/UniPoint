namespace MissBot.Common.Interfaces;
public interface IIdentityService
{
    Task<string> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(ApiResult Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<ApiResult> DeleteUserAsync(string userId);
}

public class ApiResult
{
}
