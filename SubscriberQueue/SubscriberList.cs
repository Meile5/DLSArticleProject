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
        bool doesEmailExist = _subscriberList.Where(s => s.Email == sub.Email).FirstOrDefault() != null;
        if (!doesIdExist && !doesEmailExist)
        {
            _subscriberList.Add(sub);
        }
        else
        {
            Console.WriteLine("subscriber with that ID or email already exists");
        }
        
    }
    
    
    public void Unsubscribe(Guid guid)
    {
        //since the subscribe code prevents multiple subscribers with the same ID, this should be fine
        _subscriberList.Where(s => s.SubscriberId == guid)
            .ToList().ForEach(s => s.isActive = false);
        
    }
    
    public void Unsubscribe(string email)
    {
        _subscriberList.Where(s => s.Email == email)
            .ToList().ForEach(s => s.isActive = false);
    }

}