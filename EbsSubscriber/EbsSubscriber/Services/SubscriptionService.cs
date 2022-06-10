using EbsSubscriber.Helpers;
using EbsSubscriber.Models;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;

namespace EbsSubscriber.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly SubscriptionGeneratorSetup generatorSetup;
        private readonly List<Subscription> subscriptions;
        private int broker;
        private readonly HttpClient client;
        private readonly List<PublicationDTO> publications;

        // generator setup should be loaded with values from a config file
        public SubscriptionService()
        {
            subscriptions = new List<Subscription>();
            publications = new List<PublicationDTO>();

            generatorSetup = new SubscriptionGeneratorSetup()
            {
                SubscriptionsNumber = 100,
                CompanyFields = 50,
                ValueFields = 70,
                VariationFields = 30,
                DropFields = 20,
                DateFields = 40,
                CompanyEqualFields = 10,
                ValueEqualFields = 10,
                VariationEqualFields = 10,
                DropEqualFields = 10,
                DateEqualFields = 10,
            };

            for(int i = 0; i < generatorSetup.SubscriptionsNumber; i++)
                subscriptions.Add(new Subscription() { Company = null, Date = null, Drop = null, Value = null, Variation = null });

            FindBroker();
            client = new HttpClient();
        }

        private async void FindBroker()
        {
            List<int> brokers = new List<int>();

            for (int i = 5100; i < 5200; ++i)
                if(await PingPort(i))
                    brokers.Add(i);

            if (brokers.Any())
                broker = brokers[new Random().Next(brokers.Count)];

            Console.WriteLine($"Connected to broker {broker}");

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

        public async void SendSubscriptionsAsync(int ownPort)
        {
            GenerateSubscriptions();

            if (broker == 0)
                return;

            foreach (var subscription in subscriptions)
                await client.PostAsJsonAsync($"http://localhost:{broker}/Broker/subscribe", new SubscriptionDTO(subscription, ownPort));
        }

        public void GenerateSubscriptions()
        {
            List<Tuple<string, string>> subscriptionFields = new List<Tuple<string, string>>();
            Random rng = new Random();
            int index = 0;

            subscriptionFields.AddRange(Enumerable.Repeat(new Tuple<string, string>("company", "="), generatorSetup.CompanyEqualFields));
            subscriptionFields.AddRange(Enumerable.Repeat(new Tuple<string, string>("company", FieldGenerator.GetRandomOperator()), 
                generatorSetup.CompanyFields - generatorSetup.CompanyEqualFields));
            subscriptionFields.AddRange(Enumerable.Repeat(new Tuple<string, string>("value", "="), generatorSetup.ValueEqualFields));
            subscriptionFields.AddRange(Enumerable.Repeat(new Tuple<string, string>("value", FieldGenerator.GetRandomOperator()),
                generatorSetup.ValueFields - generatorSetup.ValueEqualFields));
            subscriptionFields.AddRange(Enumerable.Repeat(new Tuple<string, string>("variation", "="), generatorSetup.VariationEqualFields));
            subscriptionFields.AddRange(Enumerable.Repeat(new Tuple<string, string>("variation", FieldGenerator.GetRandomOperator()),
                generatorSetup.VariationFields - generatorSetup.VariationEqualFields));
            subscriptionFields.AddRange(Enumerable.Repeat(new Tuple<string, string>("drop", "="), generatorSetup.DropEqualFields));
            subscriptionFields.AddRange(Enumerable.Repeat(new Tuple<string, string>("drop", FieldGenerator.GetRandomOperator()),
                generatorSetup.DropFields - generatorSetup.DropEqualFields));
            subscriptionFields.AddRange(Enumerable.Repeat(new Tuple<string, string>("date", "="), generatorSetup.DateEqualFields));
            subscriptionFields.AddRange(Enumerable.Repeat(new Tuple<string, string>("date", FieldGenerator.GetRandomOperator()),
                generatorSetup.DateFields - generatorSetup.DateEqualFields));

            subscriptionFields = subscriptionFields.OrderBy(x => rng.Next()).ToList();

            for (int i = 0; i < subscriptionFields.Count; ++i)
            {
                var field = subscriptionFields[i];

                if (field.Item1 == "company")
                {
                    if (subscriptions[index].Company == null)
                        subscriptions[index].Company = new StringField() { Value = FieldGenerator.GetRandomCompany(), Operator = field.Item2 };
                    else
                        --i;

                } else if (field.Item1 == "value")
                {
                    if (subscriptions[index].Value == null)
                        subscriptions[index].Value = new DoubleField() { Value = FieldGenerator.GetRandomDouble(), Operator = field.Item2 };
                    else
                        --i;
                } else if (field.Item1 == "variation")
                {
                    if (subscriptions[index].Variation == null)
                        subscriptions[index].Variation = new DoubleField() { Value = FieldGenerator.GetRandomDouble(), Operator = field.Item2 };
                    else
                        --i;
                } else if (field.Item1 == "drop")
                {
                    if (subscriptions[index].Drop == null)
                        subscriptions[index].Drop = new DoubleField() { Value = FieldGenerator.GetRandomDouble(), Operator = field.Item2 };
                    else
                        --i;
                } else if (field.Item1 == "date")
                {
                    if (subscriptions[index].Date == null)
                        subscriptions[index].Date = new DateField() { Value = FieldGenerator.GetRandomDate(), Operator = field.Item2 };
                    else
                        --i;
                }

                index = index >= subscriptions.Count - 1 ? 0 : index + 1;
            }
        }

        public void AddPublication(PublicationDTO publicationDTO)
        {
            publications.Add(publicationDTO);
        }

        public void PrintReceivedPublications(int ownPort)
        {
            using(StreamWriter sw = File.CreateText($"subscirber_publications_{ownPort}.json"))
                foreach (var publication in publications)
                    sw.WriteLine(JsonSerializer.Serialize(publication));
        }
    }
}
