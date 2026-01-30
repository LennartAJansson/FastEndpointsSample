namespace MediatorLib1.Abstract;

public interface IMediator
{
  Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    where TResponse : class;
}
