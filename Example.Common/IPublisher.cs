using System.Threading.Tasks;

namespace Example.Common
{
    public interface IPublisher
    {
        Task PublishAsync(string topicName, string message);
    }
}