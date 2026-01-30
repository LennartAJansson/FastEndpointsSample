using WebApplication1.Features.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

_=builder.Services.AddCustomerEndpoint();

WebApplication app = builder.Build();

app.UseCustomerEndpoint();

app.Run();
