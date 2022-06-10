using EbsPublisher.Helpers;
using EbsPublisher.Models;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;

namespace EbsPublisher.Services
{
    public class PublicationService : IPublicationService
    {
        private readonly List<Publication> publications;
        private int broker;
        private readonly HttpClient client;

        public PublicationService()
        {
            publications = new List<Publication>();
            FindBroker();
            client = new HttpClient();
        }

        private async void FindBroker()
        {
            List<int> brokers = new List<int>();

            for (int i = 5100; i < 5200; ++i)
                if (await PingPort(i))
                    brokers.Add(i);

            if (brokers.Any())
                broker = brokers[new Random().Next(brokers.Count)];

            Console.WriteLine($"Connected to broker running on port: {broker}");
        }

        private async Task<bool> PingPort(int port)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    var task = client.ConnectAsync("127.0.0.1", port);
                    if (task.Wait(10))
                        if (client.Connected)
                            return true;

                    return false;
                }
            }
            catch (SocketException ex)
            {
                return false;
            }
        }

        public async void SendPublications(int ownPort)
        {
            GeneratePublications();

            using(StreamWriter sw = File.CreateText($"publisher_{ownPort}.json"))
            {
                foreach (var publication in publications)
                {
                    sw.WriteLine(JsonSerializer.Serialize(publication));
                    await client.PostAsJsonAsync($"http://localhost:{broker}/Broker/publish", new PublicationDTO(publication, ownPort));
                }
            }          
        }

        public void GeneratePublications()
        {
            // TODO: a value from a configuration file should be used here instead of hardcoded one
            for(int i = 0; i < 100; ++i)
            {
                publications.Add(new Publication()
                {
                    Company = FieldGenerator.GetRandomCompany(),
                    Value = FieldGenerator.GetRandomDouble(),
                    Drop = FieldGenerator.GetRandomDouble(),
                    Variation = FieldGenerator.GetRandomDouble(),
                    Date = FieldGenerator.GetRandomDate()
                });
            }
        }
    }
}
