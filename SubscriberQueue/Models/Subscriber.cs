using System.Diagnostics;
using Serilog;

namespace SubscriberQueue.Models;

public class Subscriber
{
    public Guid SubscriberId { get; set; }
    public string Email { get; set; }
    public bool isActive { get; set; }

    public void RecieveMail(string contents)
    {
        //I think sending actual mail to actual email addresses is
        //out of the scope of the project, so I just did this
        Console.WriteLine("You got mail: " + contents);
    }
    
}