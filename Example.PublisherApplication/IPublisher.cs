using System.Threading.Tasks;

namespace Example.PublisherApplication
{
    public interface IPublisher
    {
        Task PublishAsync(string message);
    }
}