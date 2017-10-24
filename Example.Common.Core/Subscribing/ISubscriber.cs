using System.Threading.Tasks;

namespace Example.Common.Subscribing
{
    public interface ISubscriber
    {
        Task ListenAsync(string applicationName, string topicName);
    }
}