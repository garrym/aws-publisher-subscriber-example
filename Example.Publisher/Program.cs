using System;
using System.Threading.Tasks;
using Amazon;
using Example.Common;
using Example.Common.Publishing;

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

                await publisher.PublishAsync("ExampleTopic", message);

                Console.WriteLine("Message sent");
            }
        }
    }
}
