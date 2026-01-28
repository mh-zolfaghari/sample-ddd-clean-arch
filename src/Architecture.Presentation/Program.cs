using Architecture.Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApiServices(builder.Configuration);
builder.Services.ConfigureInfrastructureServices(builder.Configuration);
builder.ConfigureApplicationServices();

builder.Services.AddOpenApi();


var app = builder.Build();

app.UseAppLocalization();
app.UseCustomExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();