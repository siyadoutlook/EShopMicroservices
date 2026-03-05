using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Central_Handler;
using FluentValidation;
using Marten;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddMarten(options => { options.Connection(builder.Configuration.GetConnectionString("DefaultConnection")!); })
    .UseLightweightSessions();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddCarter(new DependencyContextAssemblyCatalog(typeof(Program).Assembly));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();
app.UseExceptionHandler();

app.Run();