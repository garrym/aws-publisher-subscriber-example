using System;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SQS;

namespace Example.SubscriberApplication
{
    class Program
    {
        static void Main()
        {
            Run().GetAwaiter().GetResult();
        }

        private static async Task Run()
        {
            var credentials = new BasicAWSCredentials("accessKey", "secretKey");
            
            using (var sqsClient = new AmazonSQSClient(credentials))
            using (var snsClient = new AmazonSimpleNotificationServiceClient(credentials))
            {
                var subscriber = new Subscriber(sqsClient, snsClient, "topicName", "queueName");

                Console.WriteLine("Listening...");

                await subscriber.ListenAsync(message => {
                    Console.WriteLine("Received message: " + message);
                });
            }
        }
    }
}