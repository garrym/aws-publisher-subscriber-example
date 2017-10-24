using System;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleNotificationService.Model;

namespace Example.Common.Publishing
{
    public class Publisher : Manager, IPublisher
    {
        public Publisher(string accessKey, string secretKey, RegionEndpoint region)
            : base(accessKey, secretKey, region)
        {}

        public async Task PublishAsync(string topicName, string message)
        {
            var topicArn = await CreateTopicAsync(topicName);

            using (var snsClient = GetSnsClient())
            {
                var publishRequest = new PublishRequest
                {
                    TopicArn = topicArn,
                    Message = message
                };

                var publishResponse = await snsClient.PublishAsync(publishRequest);
                if (publishResponse.HttpStatusCode != HttpStatusCode.OK)
                    throw new Exception($"Unable to publish notification. HttpStatus: {publishResponse.HttpStatusCode} received.");
            }
        }
    }
}
