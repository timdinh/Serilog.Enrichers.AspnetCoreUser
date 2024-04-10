using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Enrichers.AspnetCoreUser;


public class AspnetCoreUserClaimsEnricher(IHttpContextAccessor httpContextAccessor, params string[] excludeClaimTypes) : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var claims = httpContextAccessor?.HttpContext?.User?.Claims ?? [];

        var json = JsonSerializer.Serialize(
            claims.Where(x => !excludeClaimTypes.Contains(x.Type)),
            SerializerOptions.CompactOptions);
        
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Claims", json));
    }
}