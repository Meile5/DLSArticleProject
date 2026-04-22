namespace NewsletterService.Exceptions;

public class SubscriberServiceUnavailableException : Exception
{
    public SubscriberServiceUnavailableException(string message, Exception? inner = null)
        : base(message, inner) { }
}
