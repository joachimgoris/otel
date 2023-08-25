using System.Diagnostics;

namespace api;

public static class Telemetry
{
    public static readonly ActivitySource OtelActivitySource = new(TelemetryConstants.ServiceName);
}
