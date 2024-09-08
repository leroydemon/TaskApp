using Infrastructure.Extentions;
using Authorization;
using Infrastucture.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorizationService(builder.Configuration);
builder.Services.AddCustomAuthorization();
builder.Services.AddCustomIdentity();
builder.Services.AddScopedService();
builder.Services.ServiceCollections(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    await SeedRoles.InitializeAsync(scope.ServiceProvider);
}

app.UseMiddleware<ErrorHandlingService>();
app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
