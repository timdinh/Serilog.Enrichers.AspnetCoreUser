using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Enrichers.AspnetCoreUser;


public class AspnetCoreIdentityNameEnricher(IHttpContextAccessor httpContextAccessor) : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var user = httpContextAccessor?.HttpContext?.User;
        
        var name = user?.Identity?.Name;
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Identity Name", name ?? "Not Available"));
        
        var id = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
                 ?? user?.Claims?.FirstOrDefault(x => x.Type == Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.NameId)?.Value;
        
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Identity ID", id ?? "Not Available"));
    }
}