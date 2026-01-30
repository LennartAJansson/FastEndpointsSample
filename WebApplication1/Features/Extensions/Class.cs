namespace WebApplication1.Features.Extensions;

using FastEndpoints;
using FastEndpoints.Swagger;

using Scalar.AspNetCore;



//If this extension is going to be used in a Class Library containing ASP.NET parts then...
//Add a frameworkreference in your csproj like this:          
//<ItemGroup>
//<FrameworkReference Include="Microsoft.AspNetCore.App" />
//</ItemGroup>
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;

public static class CustomerEndpointExtensions
{
  extension(WebApplicationBuilder builder)
  {
    public WebApplicationBuilder AddCustomerEndpoint()
    {
      return builder;
    }
  }

  extension(IServiceCollection services)
  {
    public IServiceCollection AddCustomerEndpoint()
    {
      services
      .AddFastEndpoints()
      .SwaggerDocument(s =>
      {
        s.DocumentSettings = d =>
        {
          d.DocumentName = "v1";
          d.Title = "AuthService";
          d.Version = "v1";
        };
        s.ShortSchemaNames = true;
      })
      .AddEndpointsApiExplorer();
      return services;
    }
  }

  extension(WebApplication app)
  {
    public WebApplication UseCustomerEndpoint()
    {
      app.UseFastEndpoints();

      app.UseOpenApi(c => c.Path = "/openapi/{documentName}.json");
      app.MapScalarApiReference();
      return app;
    }
  }
}
