using System;
using System.Threading.Tasks;
using Amazon.SQS.Model;

namespace Example.SubscriberApplication
{
    public interface ISubscriber
    {
        Task ListenAsync(Func<Message, Task> messageHandler);
    }
}