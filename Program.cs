using System.Diagnostics;
using api;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetry()
.ConfigureResource(resource => resource.AddService(serviceName: TelemetryConstants.ServiceName,
    serviceVersion: TelemetryConstants.ServiceVersion, serviceInstanceId: Environment.MachineName))
.WithTracing(_ =>
{
    _.AddSource(TelemetryConstants.ServiceName)
    .AddConsoleExporter()
    .AddAspNetCoreInstrumentation()
    .SetSampler(new AlwaysOnSampler());
});
// Logging
// builder.Logging.AddConsole().AddOpenTelemetry(options => {
//     options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName: TelemetryConstants.ServiceName,
//     serviceVersion: TelemetryConstants.ServiceVersion, serviceInstanceId: Environment.MachineName));
//     options.AddConsoleExporter();
// });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
// app.MapGet("/", () => $"OpenTelemetry trace: {Activity.Current.Id}");

app.Run();
