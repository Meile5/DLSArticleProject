using System.Net.Http.Json;
using NewsletterService.Exceptions;
using NewsletterService.Models.Dtos;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Wrap;
using Serilog;

namespace NewsletterService.Clients;

public class SubscriberClient : ISubscriberClient
{
    // Policies are static so their state (e.g. circuit-breaker failure count) is
    // shared across all SubscriberClient instances. HttpClient factory creates a
    // new typed client per injection; a per-instance breaker would reset each time.
    private static readonly AsyncRetryPolicy _retry = Policy
        .Handle<HttpRequestException>()
        .Or<TaskCanceledException>()
        .WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(200 * Math.Pow(2, attempt - 1)),
            onRetry: (ex, delay, attempt, _) =>
                Log.Logger.Warning("Retry {Attempt} calling SubscriberService after {Delay}ms: {Error}",
                    attempt, delay.TotalMilliseconds, ex.Message));

    private static readonly AsyncCircuitBreakerPolicy _circuitBreaker = Policy
        .Handle<HttpRequestException>()
        .Or<TaskCanceledException>()
        .CircuitBreakerAsync(
            exceptionsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30),
            onBreak: (ex, duration) =>
                Log.Logger.Error("Circuit OPENED - SubscriberService is down for {Duration}s: {Error}",
                    duration.TotalSeconds, ex.Message),
            onReset: () => Log.Logger.Information("Circuit CLOSED - SubscriberService is back"));

    private static readonly AsyncPolicyWrap _resiliencePipeline = Policy.WrapAsync(_retry, _circuitBreaker);

    private readonly HttpClient _httpClient;

    public SubscriberClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<SubscribersDto>> GetSubscribersAsync(CancellationToken ct = default)
    {
        try
        {
            return await _resiliencePipeline.ExecuteAsync(async token =>
            {
                var response = await _httpClient.GetAsync("api/v1/Subscribers", token);
                response.EnsureSuccessStatusCode();

                var subscribers = await response.Content.ReadFromJsonAsync<List<SubscribersDto>>(token);
                return (IReadOnlyList<SubscribersDto>)(subscribers ?? new List<SubscribersDto>());
            }, ct);
        }
        catch (BrokenCircuitException ex)
        {
            throw new SubscriberServiceUnavailableException(
                "SubscriberService is unavailable (circuit open). Skipping this operation.", ex);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            throw new SubscriberServiceUnavailableException(
                "SubscriberService call failed after retries.", ex);
        }
    }
}
