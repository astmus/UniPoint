//using FluentValidation;

//using ValidationException = MissBot.Common.Exceptions.ValidationException;

//namespace MissBot.Common.Behaviours;
//public class ValidationBehaviour<TRequest, TResponse> : MediatR.IPipelineBehavior<TRequest, TResponse> where TRequest : MediatR.IRequest<TResponse>
//{
//    private readonly IEnumerable<IValidator<TRequest>> _validators;

//    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
//    {
//        _validators = validators;
//    }

//    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, MediatR.RequestHandlerDelegate<TResponse> next)
//    {
//        if (_validators.Any())
//        {
//            var context = new ValidationContext<TRequest>(request);

//            var validationResults = await Task.WhenAll(
//                _validators.Select(v =>
//                    v.ValidateAsync(context, cancellationToken)));

//            var failures = validationResults
//                .Where(r => r.Errors.Any())
//                .SelectMany(r => r.Errors)
//                .ToList();

//            if (failures.Any())
//                throw new ValidationException(failures);
//        }
//        return await next();
//    }


//}
