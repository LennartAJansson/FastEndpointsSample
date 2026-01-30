namespace WebApplication1.Features.Customers.Create;



using FastEndpoints;

using FluentResults;

sealed class CreateCustomerEndpoint
  : Endpoint<CreateCustomerRequest, CreateCustomerResponse>
{
  public override void Configure()
  {
    Post("/");
    Group<CreateCustomerGroup>();
    AllowAnonymous();
  }

  public override async Task HandleAsync(CreateCustomerRequest r, CancellationToken c)
  {
    var handlerResult = await r.ToCommand().ExecuteAsync(c);
    if (handlerResult.IsFailed)
    {
      await Send.NotFoundAsync(c);
      return;
    }
  
    await Send.OkAsync(handlerResult.Value.ToResponse(), c);
  }
}

sealed class CreateCustomerGroup
  : Group
{
  public CreateCustomerGroup()
  {
    Configure("/CreateCustomergroup", ep =>
    {
      //ep.Description(x => x.WithTags("CreateCustomerGroup"));
    });
  }
}

sealed class CreateCustomerSummary
  : Summary<CreateCustomerEndpoint>
{
  public CreateCustomerSummary()
  {
    Summary = "CreateCustomerEndpoint";
    Description = "CreateCustomerEndpoint.";

    Response<CreateCustomerResponse>(200, "OK", example: new CreateCustomerResponse("aName"));
  }
}

sealed record CreateCustomerRequest(string Name);

sealed record CreateCustomerResponse(string Name);

sealed class CreateCustomerValidator
  : Validator<CreateCustomerRequest>
{
  public CreateCustomerValidator()
  {
    // RuleFor(...);	
  }
}

static class CreateCustomerCommandMapper
{
  public static CreateCustomerCommand ToCommand(this CreateCustomerRequest r) => new(r.Name);
}

static class CreateCustomerResultMapper
{
  public static CreateCustomerResponse ToResponse(this CreateCustomerResult r) => new(r.Name);
}

public sealed record CreateCustomerCommand(string Name)
  : ICommand<Result<CreateCustomerResult>>;

public sealed record CreateCustomerResult(string Name);

public sealed class CreateCustomerHandler()
  : CommandHandler<CreateCustomerCommand, Result<CreateCustomerResult>>
{
  public override async Task<Result<CreateCustomerResult>> ExecuteAsync(CreateCustomerCommand cmd, CancellationToken ct)
  {
    //Convert to entity and save to database etc...
    return Result.Ok(new CreateCustomerResult(cmd.Name));
  }
}