using SubscriberQueue.Models;

namespace SubscriberQueue;

public class SubscriberList
{
    
    private readonly List<Subscriber> _subscriberList = new List<Subscriber>();

    public List<Subscriber> GetList()
    {
        return _subscriberList;
    }
    
    public void Subscribe(Subscriber sub)
    {
        bool doesIdExist = _subscriberList.Where(s => s.SubscriberId == sub.SubscriberId).FirstOrDefault() != null;
        if (!doesIdExist)
        {
            _subscriberList.Add(sub);
        }
        else
        {
            Console.WriteLine("subscriber with that ID already exists");
        }
        
    }
    
    
    public void Unsubscribe(Guid guid)
    {
        
        _subscriberList.Where(s => s.SubscriberId == guid)
            .ToList().ForEach(s => s.isActive = false);

        
    }

}