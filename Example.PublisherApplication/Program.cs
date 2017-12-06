using System;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.Runtime;

namespace Example.PublisherApplication
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
            
            using (var snsClient = new AmazonSimpleNotificationServiceClient(credentials))
            {
                var publisher = new Publisher(snsClient, "topicName");

                while (true)
                {
                    Console.WriteLine("Type a message to send:");
                    var message = Console.ReadLine();

                    await publisher.PublishAsync(message);
                }
            }
        }
    }
}