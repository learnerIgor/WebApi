using Serilog;
using Serilog.Events;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Todos.Service;
using Common.Api;
using Common.Repositories;
using System.Text.Json.Serialization;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.File("Logs/Information-.txt", LogEventLevel.Information, rollingInterval: RollingInterval.Day)
    .WriteTo.File("Logs/Log-Error-.txt", LogEventLevel.Error, rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers().AddJsonOptions(j => j.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddTodosServices();

    builder.Services.AddFluentValidationAutoValidation();

    builder.Services.AddSwaggerGen();

    builder.Host.UseSerilog();

    builder.Services.AddTodosDatabase(builder.Configuration);

    var app = builder.Build();

    app.UseExceptionsHandler();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Error(ex, "Run error");
}