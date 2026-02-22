using Polly;
using Polly.CircuitBreaker;
using ProfanityService.Models.Dtos;

namespace CommentService.Clients;

public class ProfanityClient(HttpClient httpClient) : IProfanityClient
{
    private readonly AsyncCircuitBreakerPolicy _circuitBreaker = Policy
        .Handle<HttpRequestException>()
        .CircuitBreakerAsync(
            exceptionsAllowedBeforeBreaking: 3,
            durationOfBreak: TimeSpan.FromSeconds(30)
        );

    public async Task<bool> FilterComment(string text)
    {
        try
        {
            return await _circuitBreaker.ExecuteAsync(async () =>
            {
                var response = await httpClient.PostAsJsonAsync("/profanity", new CommentDto { Comment = text });
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<bool>();
            });
        }
        catch (BrokenCircuitException)
        {
            return false; 
        }
    }
}