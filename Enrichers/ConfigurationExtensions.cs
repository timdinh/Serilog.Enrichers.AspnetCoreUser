using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog.Configuration;

namespace Serilog.Enrichers.AspnetCoreUser;

public static class ConfigurationExtensions
{
    /// <summary>
    /// Enrich serilog with username from ClaimPrincipal
    /// </summary>
    /// <param name="enrichmentConfiguration"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static LoggerConfiguration WithIdentityName(this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        if (enrichmentConfiguration == null)
            throw new ArgumentNullException(nameof (enrichmentConfiguration));

        return enrichmentConfiguration.With(new AspnetCoreIdentityNameEnricher(new HttpContextAccessor()));
    }
    
    /// <summary>
    /// Enrich with all user claims from the http request.
    /// </summary>
    /// <param name="excludeClaimTypes">Claim types to excluded from logging</param>
    public static LoggerConfiguration WithAllClaims(this LoggerEnrichmentConfiguration enrichmentConfiguration, params string[] excludeClaimTypes)
    {
        if (enrichmentConfiguration == null)
            throw new ArgumentNullException(nameof (enrichmentConfiguration));

        return enrichmentConfiguration.With(new AspnetCoreUserClaimsEnricher(new HttpContextAccessor(), excludeClaimTypes));
    }
    
    /// <summary>
    /// Enrich with all request headers
    /// </summary>
    /// <param name="excludeFields">Header field to exclude from logging</param>
    public static LoggerConfiguration WithAllRequestHeaders(this LoggerEnrichmentConfiguration enrichmentConfiguration, params string[] excludeFields)
    {
        if (enrichmentConfiguration == null)
            throw new ArgumentNullException(nameof (enrichmentConfiguration));

        return enrichmentConfiguration.With(new AllRequestHeadersEnricher(new HttpContextAccessor(), excludeFields));
    }
}

#if NET7_0_OR_GREATER
[JsonSerializable(typeof(IEnumerable<Claim>))]
[JsonSerializable(typeof(Dictionary<string, StringValues>))]
internal sealed partial class AspnetCoreEnricherJsonSerializerContext : JsonSerializerContext
{
}
#endif

internal static class SerializerOptions
{
    public static JsonSerializerOptions CompactOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
#if NET7_0_OR_GREATER
        TypeInfoResolver = AspnetCoreEnricherJsonSerializerContext.Default
#endif
    };
}