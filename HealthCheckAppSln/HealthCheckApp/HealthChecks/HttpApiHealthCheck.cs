using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheckApp.HealthChecks;

public class HttpApiHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public HttpApiHealthCheck(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();

            var resp = await client.GetAsync("https://jsonplaceholder.typicode.com/posts/1", cancellationToken);

            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                return HealthCheckResult.Unhealthy();

            return HealthCheckResult.Healthy();

        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
        finally
        {
        }


    }
}
