using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Example.SubscriberApplication
{
    public class Subscriber : IDisposable
    {
        private AmazonSQSClient _sqsClient;
        private AmazonSimpleNotificationServiceClient _snsClient;
        private readonly string _topicName;
        private string _topicArn;
        private readonly string _queueName;
        private string _queueUrl;
        private bool _initialised;
        private bool _disposed;

        public Subscriber(AmazonSQSClient sqsClient, AmazonSimpleNotificationServiceClient snsClient, string topicName, string queueName)
        {
            _sqsClient = sqsClient;
            _snsClient = snsClient;
            _topicName = topicName;
            _queueName = queueName;
        }

        public async Task Initialise()
        {
            _topicArn = (await _snsClient.CreateTopicAsync(_topicName)).TopicArn;
            _queueUrl = (await _sqsClient.CreateQueueAsync(_queueName)).QueueUrl;
            await SubscribeTopicToQueue();

            _initialised = true;
        }

        private async Task SubscribeTopicToQueue()
        {
            var currentSubscriptions = (await _snsClient.ListSubscriptionsByTopicAsync(_topicArn)).Subscriptions;
            if (currentSubscriptions.Any())
            {
                var queueArn = (await _sqsClient.GetQueueAttributesAsync(_queueUrl, new List<string> { "QueueArn" })).QueueARN;
                var existingSubscription = currentSubscriptions.FirstOrDefault(x => x.Endpoint == queueArn);
                if (existingSubscription != null)
                {
                    return;
                }
            }

            await _snsClient.SubscribeQueueAsync(_topicArn, _sqsClient, _queueUrl);
        }

        public async Task ListenAsync(Action<Message> messageHandler)
        {
            if (!_initialised)
                await Initialise();

            while (true)
            {
                var response = await _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest { QueueUrl = _queueUrl, WaitTimeSeconds = 10 });
                foreach (var message in response.Messages)
                {
                    messageHandler(message);
                    await _sqsClient.DeleteMessageAsync(_queueUrl, message.ReceiptHandle);
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _sqsClient.Dispose();
                    _sqsClient = null;

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