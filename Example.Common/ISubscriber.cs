using System.Threading.Tasks;

namespace Example.Common
{
    public interface ISubscriber
    {
        Task ListenAsync(string applicationName, string topicName);
    }
}