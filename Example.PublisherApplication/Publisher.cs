using System;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;

namespace Example.PublisherApplication
{
    public class Publisher : IDisposable
    {
        private AmazonSimpleNotificationServiceClient _snsClient;
        private readonly string _topicName;
        private string _topicArn;
        private bool _initialised;
        private bool _disposed;

        public Publisher(AmazonSimpleNotificationServiceClient snsClient, string topicName)
        {
            _snsClient = snsClient;
            _topicName = topicName;
        }

        public async Task Initialise()
        {
            _topicArn = (await _snsClient.CreateTopicAsync(_topicName)).TopicArn;

            _initialised = true;
        }

        public async Task PublishAsync(string message)
        {
            if (!_initialised)
                await Initialise();

            await _snsClient.PublishAsync(_topicArn, message);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _snsClient.Dispose();
                    _snsClient = null;
                }

                _disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}