namespace MediatorLib1;

using MediatorLib1.Abstract;

using Microsoft.Extensions.DependencyInjection;

//---------------------------------------------
// Mediator implementation
//---------------------------------------------

public class Mediator(IServiceProvider serviceProvider)
  : IMediator
{
  public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    where TResponse : class
  {
    using IServiceScope scope = serviceProvider.CreateScope();

    //Find that specific IRequestHandler for that specific request
    // e.g., IRequestHandler<IRequest<TResponse>, TResponse>
    Type? handlerType = typeof(IRequestHandler<,>)
      .MakeGenericType(request.GetType(), typeof(TResponse));

    if (handlerType is null)
      throw new ArgumentNullException(nameof(request), "Handler type cannot be null.");

    //This is a bit dirty, dynamic should always be avoided, but in this case it's acceptable
    dynamic? handler = scope.ServiceProvider.GetService(handlerType);

    //If no handler found, throw. Else invoke the handler and return the result
    return handler is null
      ? throw new ArgumentNullException(nameof(request), "Handler cannot be null.")
      : (TResponse)await handler.Handle((dynamic)request);
  }
}
