using MotoDispatch.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAppConections(builder.Configuration)
    .AddUseCases()
    .AddStorage(builder.Configuration)
    .AddAndConfigureControllers()
    .AddCors(p =>
        p.AddPolicy("CORS", builder => { builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader(); })
    );

var app = builder.Build();
// app.MigrateDatabase();
app.UseDocumentation();
app.UseRequestResponseLogging();
//app.UseHttpsRedirection();
app.UseCors("CORS");
app.UseAuthentication();
app.UseAuthorization();
app.UseHealthChecks("/healthchecks-ui");
app.MapControllers();

app.Run();

public partial class Program
{
}