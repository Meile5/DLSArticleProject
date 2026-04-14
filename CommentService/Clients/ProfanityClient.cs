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
            durationOfBreak: TimeSpan.FromSeconds(30),
            onBreak: (ex, duration) => Console.WriteLine("Circuit OPENED - profanity service is down"),
            onReset: () => Console.WriteLine("Circuit CLOSED - profanity service is back")
        );
    
    public async Task<bool> FilterComment(string text)
    {
        try
        {
            return await _circuitBreaker.ExecuteAsync(async () =>
            {
                var response = await httpClient.PostAsJsonAsync("/profanity", new ProfanityCommentDto { Comment = text });
            
                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    return true; 
            
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<bool>();
            });
        }
        catch (BrokenCircuitException)
        {
            throw new Exception("Comment service temporarily unavailable");
        }
    }
}