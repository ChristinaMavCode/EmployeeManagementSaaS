
var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders().AddSerilog(logger);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization();

builder.Services.AddOpenApi();
builder.Services.AddDefaultConfiguration();
builder.Services.AddHttpConfiguration();
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateSkillCommand).Assembly));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddValidatorsFromAssemblyContaining<CreateSkillCommandValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseMiddleware<PerformanceLoggingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors();

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

versionedEndpointRouteBuilder.MapPost("/api/v{version:apiVersion}/login", 
    async ([FromBody] LoginDto login, [FromServices] IAuthService authService) =>
    {
        var token = await authService.Login(login);
        return token is null ? Results.Unauthorized() : Results.Ok(new { Token = token });
    }).WithName("Login").HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getskills",
    async ([FromServices] IMediator mediator) =>
    {
        var result = await mediator.Send(new GetAllSkillsQuery());
        return result is not null ? Results.Ok(result) : Results.NotFound();
    })
    .WithName("GetSkills").HasApiVersion(1.0).RequireAuthorization();

versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/createskill",
    async ([FromBody] CreateSkillCommand command, [FromServices] IMediator mediator) =>
    {
        var skill = await mediator.Send(command);
        return Results.Created(string.Empty, skill);
    })
    .WithName("CreateSkill").HasApiVersion(1.0).RequireAuthorization();

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getemployees",
    async ([FromServices] IMediator mediator) =>
    {
        var result = await mediator.Send(new GetAllEmployeesQuery());
        return result is not null ? Results.Ok(result) : Results.NotFound();
    })
    .WithName("GetEmployees").HasApiVersion(1.0).RequireAuthorization();

versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/assignskilltoemployee",
    async ([FromBody] AssignSkillToEmployeeCommand command, [FromServices] IMediator mediator) =>
    {
        var skill = await mediator.Send(command);
        return Results.Created(string.Empty, skill);
    })
    .WithName("AssignSkillToEmployee").HasApiVersion(1.0).RequireAuthorization();

app.Run();