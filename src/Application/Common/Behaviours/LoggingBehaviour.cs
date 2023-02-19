//using MediatR.Pipeline;
//using Microsoft.Extensions.Logging;
//using MissBot.Common.Interfaces;

//namespace MissBot.Common.Behaviours;
//public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
//{
//    private readonly ILogger _logger;
//    private readonly ICurrentUserService _currentUserService;
//    private readonly IIdentityService _identityService;

//    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService, IIdentityService identityService)
//    {
//        _logger = logger;
//        _currentUserService = currentUserService;
//        _identityService = identityService;
//    }

//    public async Task Process(TRequest request, CancellationToken cancellationToken)
//    {
//        var requestName = typeof(TRequest).Name;
//        var userId = _currentUserService.UserId;
//        var userName = string.Empty;

//        if (userId != 0)
//        {
//            userName = await _identityService.GetUserNameAsync(userId.ToString());
//        }

//        _logger.LogInformation("MissBot Request: {Name} {@UserId} {@UserName} {@Request}",
//            requestName, userId, userName, request);
//    }
//}
