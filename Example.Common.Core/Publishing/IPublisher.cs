using System.Threading.Tasks;

namespace Example.Common.Publishing
{
    public interface IPublisher
    {
        Task PublishAsync(string topicName, string message);
    }
}