using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS.Model;
using Newtonsoft.Json;

namespace Example.Common
{
    public class Subscriber : Manager, ISubscriber
    {
        public Subscriber(string accessKey, string secretKey, RegionEndpoint region)
            : base(accessKey, secretKey, region)
        {}

        private async Task<string> SubscribeQueueToTopicAsync(string topicArn, string queueUrl)
        {
            using (var sqsClient = GetSqsClient())
            using (var snsClient = GetSnsClient())
            {
                var subscriptionArn = await snsClient.SubscribeQueueAsync(topicArn, sqsClient, queueUrl);
                return subscriptionArn;
            }
        }

        private async Task<string> CreateQueueAsync(string queueName)
        {
            var createQueueRequest = new CreateQueueRequest(queueName);

            using (var client = GetSqsClient())
            {
                var createQueueResponse = await client.CreateQueueAsync(createQueueRequest);
                if (createQueueResponse.HttpStatusCode != HttpStatusCode.OK)
                    throw new Exception($"Unable to create queue. HttpStatus: {createQueueResponse.HttpStatusCode} received.");

                return createQueueResponse.QueueUrl;
            }
        }

        public async Task ListenAsync(string applicationName, string topicName)
        {
            var queueName = $"{topicName}-{applicationName}";
            var topicArn = await CreateTopicAsync(topicName);
            var queueUrl = await CreateQueueAsync(queueName);
            var subscriptionArn = await SubscribeQueueToTopicAsync(topicArn, queueUrl);

            using (var sqsClient = GetSqsClient())
            {
                while (true)
                {
                    var receiveMessageRequest = new ReceiveMessageRequest { MaxNumberOfMessages = 10, QueueUrl = queueUrl };
                    var responseMessage = await sqsClient.ReceiveMessageAsync(receiveMessageRequest);
                    if (responseMessage.HttpStatusCode == HttpStatusCode.OK)
                    {
                        // TODO: Throw if not OK?
                        await ProcessMessagesAsync(queueUrl, responseMessage.Messages);
                    }
                    await Task.Delay(2000);
                    // TODO: Delete messages
                }
            }
        }

        private async Task<bool> ProcessMessagesAsync(string queueUrl, IList<Message> messages)
        {
            if (!messages.Any())
                return true;

            foreach (var message in messages)
                await ProcessMessageAsync(queueUrl, message);

            return true;
        }

        private async Task ProcessMessageAsync(string queueUrl, Message message)
        {
            var messageBody = JsonConvert.DeserializeObject<MessageBody>(message.Body);
            Console.WriteLine($"Received message: {messageBody.Message}");

            await DeleteMessageAsync(queueUrl, message);
            Console.WriteLine("Deleted");
        }

        private async Task DeleteMessageAsync(string queueUrl, Message message)
        {
            using (var sqsClient = GetSqsClient())
            {
                var deleteMessageResponse = await sqsClient.DeleteMessageAsync(queueUrl, message.ReceiptHandle);
                if (deleteMessageResponse.HttpStatusCode != HttpStatusCode.OK)
                    throw new Exception($"Unable to delete messages from queue. HttpStatus: {deleteMessageResponse.HttpStatusCode} received.");
            }
        }
    }
}
