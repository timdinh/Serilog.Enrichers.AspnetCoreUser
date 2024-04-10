using Microsoft.VisualStudio.TestPlatform.TestHost;
using Serilog;
using Serilog.Enrichers.AspnetCoreUser;
using Xunit.Abstractions;

namespace Tests;


public class Tests(ITestOutputHelper output) //: IAsyncLifetime
{

    [Fact]
    public async Task Test_WithIdentityName()
    {
        // Arrange
        var logger = new LoggerConfiguration()
            .WriteTo.TestOutput(output)
            .Enrich.WithIdentityName()
            .CreateLogger();
        
        
        // Act
        logger.Information("Test");
        
        // Assert
        // Assert that the log contains the identity name
    }

    
}