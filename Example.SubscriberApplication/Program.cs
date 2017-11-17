using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;

namespace Example.SubscriberApplication
{
    class Program
    {
        static void Main()
        {
            Run().Wait();
        }

        static async Task Run()
        {
            Console.WriteLine("Listening...");

            var credentials = new BasicAWSCredentials("accessKey", "secretKey");
            
            using (var sqsClient = new AmazonSQSClient(credentials))
            using (var snsClient = new AmazonSimpleNotificationServiceClient(credentials))
            {
                var subscriber = new Subscriber(sqsClient, snsClient, "topicName", "queueName");
                await subscriber.ListenAsync(async message => {
                    Console.WriteLine("Received message: " + message);
                });
            }
        }
    }
}
