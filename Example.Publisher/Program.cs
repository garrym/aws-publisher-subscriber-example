using System;
using System.Threading.Tasks;
using Amazon;
using Example.Common;

namespace Example.PublisherApplication
{
    class Program
    {
        static void Main()
        {
            Run().Wait();
        }

        private static async Task Run()
        {
            var publisher = new Publisher(Configuration.AccessKey, Configuration.SecretKey, RegionEndpoint.EUWest2);

            while (true)
            {
                Console.WriteLine("Type a message to send:");
                var message = Console.ReadLine();

                var topicName = "ExampleTopic";

                await publisher.PublishAsync(topicName, message);

                Console.WriteLine("Message sent");
            }
        }
    }
}
