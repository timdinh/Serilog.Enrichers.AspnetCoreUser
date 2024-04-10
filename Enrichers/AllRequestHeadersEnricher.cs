using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Enrichers.AspnetCoreUser;

public class AllRequestHeadersEnricher(IHttpContextAccessor httpContextAccessor, params string[] excludeHeaders) : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var headers = 
            httpContextAccessor?.HttpContext?.Request?.Headers?.ToDictionary(x => x.Key, y => y.Value)
            ?? new Dictionary<string, StringValues>();

        var json = JsonSerializer.Serialize(headers, options: SerializerOptions.CompactOptions);
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Request Headers", json));
    }
}