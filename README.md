# Usage

1. Add Serilog to your `.csproj` file
```
<ItemGroup>
  <PackageReference Include="Serilog" Version="3.1.1" />
  <PackageReference Include="Serilog.AspNetCore" Version="8.0.1" />
</ItemGroup>
```

2. Configure logging in your `Program.cs`

```
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(logBuilder => logBuilder.AddSerilog(CreateSerilog(), dispose: true));


Serilog.ILogger CreateSerilog() =>
    new LoggerConfiguration()
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
        .Enrich.WithIdentityName()
        .Enrich.WithAllClaims()         // you can also pass in a list of claim type to exclude
        .Enrich.WithAllRequestHeaders() // you can also pass in a list of header keys to exclude
        .MinimumLevel.Debug()
        .CreateLogger();
```
