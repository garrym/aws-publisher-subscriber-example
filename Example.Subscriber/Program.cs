using System;
using System.Threading.Tasks;
using Amazon;
using Example.Common;

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

            var topicName = "ExampleTopic";

            var subscriber = new Subscriber(Configuration.AccessKey, Configuration.SecretKey, RegionEndpoint.EUWest2);
            await subscriber.ListenAsync("ExampleSubscriber", topicName);
        }
    }
}
