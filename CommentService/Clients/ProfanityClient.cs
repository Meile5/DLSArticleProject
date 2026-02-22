using CommentService.Models.Dtos;
using Polly;
using Polly.CircuitBreaker;

namespace CommentService.Clients;

public class ProfanityClient(HttpClient httpClient) : IProfanityClient
{
    private readonly AsyncCircuitBreakerPolicy _circuitBreaker = Policy
        .Handle<HttpRequestException>()
        .CircuitBreakerAsync(
            exceptionsAllowedBeforeBreaking: 3,
            durationOfBreak: TimeSpan.FromSeconds(30)
        );

    // Note:
    // When I was testing to create a comment with the profanity service down, i got:
    // System.Net.Http.HttpRequestException: Response status code does not indicate success: 403 (Forbidden).
    // I think it's throwing because of: response.EnsureSuccessStatusCode();
    // Need to check this
    
    public async Task<bool> FilterComment(string text)
    {
        try
        {
            return await _circuitBreaker.ExecuteAsync(async () =>
            {
                var response = await httpClient.PostAsJsonAsync("/profanity", new ProfanityCommentDto { Comment = text });
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