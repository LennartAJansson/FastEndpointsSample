namespace MediatorLib1;

using System.Reflection;

using MediatorLib1.Abstract;

using Microsoft.Extensions.DependencyInjection;

//---------------------------------------------
// Extension method to register Mediator and handlers
//---------------------------------------------

public static class MediatorExtensions
{
  extension(IServiceCollection services)
  {
    public IServiceCollection AddMediator(Func<Assembly[]>? assemblies = null)
    {
      _=services.AddSingleton<IMediator, Mediator>();

      if (assemblies is null)
      {
        throw new ArgumentNullException(nameof(assemblies), "Assemblies cannot be null.");
      }

      Type openHandlerType = typeof(IRequestHandler<,>);
      foreach (Assembly assembly in assemblies())
      {
        Type[] asmTypes;
        try
        {
          asmTypes = assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
          asmTypes = [.. ex.Types.OfType<Type>()];
        }

        foreach (Type? type in asmTypes)
        {
          if (type is null || type.IsAbstract || type.IsInterface)
            continue;

          Type[] implementedHandlerInterfaces = [.. type
            .GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == openHandlerType)];

          if (implementedHandlerInterfaces.Length == 0)
            continue;

          if (type.IsGenericTypeDefinition)
          {
            // Register open generic mapping: IRequestHandler<,> -> Handler<,>
            _=services.AddScoped(openHandlerType, type);
          }
          else
          {
            // Register each closed handler interface implemented by this type
            foreach (Type? serviceType in implementedHandlerInterfaces)
            {
              _=services.AddScoped(serviceType, type);
            }
          }
        }
      }

      return services;
    }
  }
}
