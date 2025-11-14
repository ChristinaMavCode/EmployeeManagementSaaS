
var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders().AddSerilog(logger);

builder.Services.AddOpenApi();
builder.Services.AddDefaultConfiguration();
builder.Services.AddHttpConfiguration();
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateSkillCommand).Assembly));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddValidatorsFromAssemblyContaining<CreateSkillCommandValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseMiddleware<PerformanceLoggingMiddleware>();
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.ContentType = "application/json";

        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (contextFeature != null)
        {
            if (contextFeature.Error is ValidationException validationException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new
                {
                    Errors = validationException.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                });
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new
                {
                    Message = "Internal Server Error."
                });
            }
        }
    });
});

var versionedEndpointRouteBuilder = app.NewVersionedApi();
versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getskills",
    async ([FromServices] IMediator mediator) =>
    {
        var result = await mediator.Send(new GetAllSkillsQuery());
        return result is not null ? Results.Ok(result) : Results.NotFound();
    })
    .WithName("GetSkills").HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/createskill",
    async ([FromBody] CreateSkillCommand command, [FromServices] IMediator mediator) =>
    {
        var skill = await mediator.Send(command);
        return Results.Created(string.Empty, skill);
    })
    .WithName("CreateSkill").HasApiVersion(1.0);

app.Run();