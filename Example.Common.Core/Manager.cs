using System;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;

namespace Example.Common
{
    public abstract class Manager
    {
        private readonly BasicAWSCredentials _credentials;
        private readonly AmazonSQSConfig _sqsConfiguration;
        private readonly AmazonSimpleNotificationServiceConfig _snsConfiguration;

        protected Manager(string accessKey, string secretKey, RegionEndpoint region)
        {
            _credentials = new BasicAWSCredentials(accessKey, secretKey);
            _sqsConfiguration = new AmazonSQSConfig { RegionEndpoint = region };
            _snsConfiguration = new AmazonSimpleNotificationServiceConfig { RegionEndpoint = region };
        }

        protected AmazonSQSClient GetSqsClient()
        {
            return new AmazonSQSClient(_credentials, _sqsConfiguration);
        }

        protected AmazonSimpleNotificationServiceClient GetSnsClient()
        {
            return new AmazonSimpleNotificationServiceClient(_credentials, _snsConfiguration);
        }

        protected async Task<string> CreateTopicAsync(string topicName)
        {
            using (var snsClient = GetSnsClient())
            {
                var createTopicRequest = new CreateTopicRequest(topicName);
                var createTopicResponse = await snsClient.CreateTopicAsync(createTopicRequest);

                if (createTopicResponse.HttpStatusCode != HttpStatusCode.OK)
                    throw new Exception($"Unable to create topic. HttpStatusCode: {createTopicResponse.HttpStatusCode}");

                return createTopicResponse.TopicArn;
            }
        }
    }
}