using Microsoft.AspNetCore.HttpLogging;
using MotoDispatch.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHttpLogging(x => x = new HttpLoggingOptions())
    .AddAppConections(builder.Configuration)
    .AddUseCases()
    .AddStorage(builder.Configuration)
    .AddAndConfigureControllers()
    .AddCors(p =>
        p.AddPolicy("CORS", builder => { builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader(); })
    );

var app = builder.Build();
// app.UseHttpLogging();
// app.MigrateDatabase();
app.UseDocumentation();
//app.UseHttpsRedirection();
app.UseCors("CORS");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program
{
}