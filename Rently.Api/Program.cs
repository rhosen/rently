using DotNetEnv;
using Rently.API.Extensions;
using Rently.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.ConfigureServiceCollection(builder.Configuration);
builder.ConfigureSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rently API v1");
});

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
