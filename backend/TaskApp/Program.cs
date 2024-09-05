using Infrastructure.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScopedService(); 
builder.Services.ServiceCollections(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
