namespace MediatorLib1.Abstract;
//---------------------------------------------
// IRequest interfaces
//---------------------------------------------

public interface IRequest
{
}

public interface IRequest<TResponse>
  : IRequest
  where TResponse : class
{
}
