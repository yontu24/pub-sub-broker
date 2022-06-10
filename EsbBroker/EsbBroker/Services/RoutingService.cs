using EsbBroker.Models;
using System.Collections.Concurrent;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;

namespace EsbBroker.Services
{
    public class RoutingService : IRoutingService
    {
        private readonly ConcurrentDictionary<Subscription, HashSet<int>> subscriptionsFromSubscribers;
        private readonly ConcurrentDictionary<Subscription, HashSet<int>> subscriptionsFromBrokers;
        private readonly List<int> brokerNeighbours;
        private readonly HttpClient client;
        private readonly List<PublicationDTO> publications;

        public RoutingService() 
        { 
            subscriptionsFromSubscribers = new ConcurrentDictionary<Subscription, HashSet<int>>();
            subscriptionsFromBrokers = new ConcurrentDictionary<Subscription, HashSet<int>>();
            publications = new List<PublicationDTO>();

            brokerNeighbours = new List<int>();
            FindOtherBrokers();

            client = new HttpClient();
        }

        private async void FindOtherBrokers()
        {
            for (int i = 5100; i < 5200; ++i)
                if (await PingPort(i))
                    brokerNeighbours.Add(i);
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

        public async void RegisterSubscriptionFromSubscriber(SubscriptionDTO subscriptionDTO, int ownPort)
        {
            Subscription newSubscription = new Subscription(subscriptionDTO);

            if (subscriptionsFromSubscribers.Keys.Any(x => x.Equals(newSubscription)))
            {
                var temp = subscriptionsFromSubscribers.Keys.First(x => x.Equals(newSubscription));
                if (!subscriptionsFromSubscribers[temp].Contains(subscriptionDTO.Sender))
                    subscriptionsFromSubscribers[temp].Add(subscriptionDTO.Sender);
            }       
            else
                subscriptionsFromSubscribers[newSubscription] = new HashSet<int>() { subscriptionDTO.Sender };

            if(CanBeForwarded(newSubscription))
            {
                int sender = subscriptionDTO.Sender;
                subscriptionDTO.Sender = ownPort;

                foreach (int port in brokerNeighbours)
                    if (port != ownPort && port != sender)
                        await client.PostAsJsonAsync($"http://localhost:{port}/Broker/forward", subscriptionDTO);
            }
        }

        public async void RegisterSubscriptionFromBroker(SubscriptionDTO subscriptionDTO, int ownPort)
        {
            Subscription newSubscription = new Subscription(subscriptionDTO);

            if (subscriptionsFromBrokers.Keys.Any(x => x.Equals(newSubscription)))
            {
                var temp = subscriptionsFromBrokers.Keys.First(x => x.Equals(newSubscription));
                if (!subscriptionsFromBrokers[temp].Contains(subscriptionDTO.Sender))
                    subscriptionsFromBrokers[temp].Add(subscriptionDTO.Sender);
            }
            else
                subscriptionsFromBrokers[newSubscription] = new HashSet<int>() { subscriptionDTO.Sender };

            if (CanBeForwarded(newSubscription))
            {
                int sender = subscriptionDTO.Sender;
                subscriptionDTO.Sender = ownPort;

                foreach (int port in brokerNeighbours)
                    if(port != ownPort && port != sender)
                        await client.PostAsJsonAsync($"http://localhost:{port}/Broker/forward", subscriptionDTO);
            }
        }

        public async void MatchPublicationAsync(PublicationDTO publicationDTO, bool searchDirect, int ownPort)
        {
            ConcurrentDictionary<Subscription, HashSet<int>> subscriptionsWithSenders = searchDirect ? subscriptionsFromSubscribers : subscriptionsFromBrokers;
            string endRoute = searchDirect ? "Subscriber" : "Broker/publish";
            Dictionary<int, bool> sentTo = new Dictionary<int, bool>();

            foreach (var sub in subscriptionsWithSenders.Keys)
            {
                bool isMatch = true;

                if (sub.Company != null)
                    isMatch = CompareField(sub.Company, publicationDTO.Company) && isMatch;
                if (sub.Value != null)
                    isMatch = CompareField(sub.Value, publicationDTO.Value) && isMatch;
                if (sub.Variation != null)
                    isMatch = CompareField(sub.Variation, publicationDTO.Variation) && isMatch;
                if (sub.Drop != null)
                    isMatch = CompareField(sub.Drop, publicationDTO.Drop) && isMatch;
                if (sub.Date != null)
                    isMatch = CompareField(sub.Date, publicationDTO.Date) && isMatch;

                if (isMatch)
                {
                    publicationDTO.OriginalSender = publicationDTO.Sender;
                    publicationDTO.Sender = ownPort;

                    foreach (var sender in subscriptionsWithSenders[sub])
                    {
                        if (!sentTo.TryGetValue(sender, out _))
                        {            
                            client.PostAsJsonAsync($"http://localhost:{sender}/{endRoute}", publicationDTO);
                            sentTo[sender] = true;
                        }
                            
                    }
                        
                }                 
            }
        }

        public async void AddPublication(PublicationDTO publicationDTO)
        {
            publications.Add(publicationDTO);
        }

        public void PrintSubscriptions(int ownPort)
        {
            using (StreamWriter sw = File.CreateText($"broker_subscriptions_{ownPort}.json"))
            {
                foreach (var subscription in subscriptionsFromBrokers)
                {
                    sw.WriteLine(JsonSerializer.Serialize(subscription));
                }

                foreach (var subscription in subscriptionsFromSubscribers)
                {
                    sw.WriteLine(JsonSerializer.Serialize(subscription));
                }

            }
        }

        public void PrintPublications(int ownPort)
        {
            using (StreamWriter sw = File.CreateText($"broker_publications_{ownPort}.json"))
            {
                foreach (var publication in publications)
                {
                    sw.WriteLine(JsonSerializer.Serialize(publication));
                }
            }
        }

        private bool CanBeForwarded(Subscription newSubscription)
        {
            foreach (var sub in subscriptionsFromBrokers.Keys)
                if (sub.Equals(newSubscription) || !AreValuesCovered(sub, newSubscription))
                        return false;

            return true;
        }

        private bool AreValuesCovered(Subscription existentSubscription, Subscription newSubscription)
        {
            if (existentSubscription.Company != null && newSubscription.Company == null 
                || !CheckFieldMatching(existentSubscription.Company, newSubscription.Company))
                return false;
            if (existentSubscription.Value != null && newSubscription.Value == null
                || !CheckFieldMatching(existentSubscription.Value, newSubscription.Value))
                return false;
            if (existentSubscription.Variation != null && newSubscription.Variation == null
                || !CheckFieldMatching(existentSubscription.Variation, newSubscription.Variation))
                return false;
            if (existentSubscription.Drop != null && newSubscription.Drop == null
                || !CheckFieldMatching(existentSubscription.Drop, newSubscription.Drop))
                return false;
            if (existentSubscription.Date != null && newSubscription.Date == null
                || !CheckFieldMatching(existentSubscription.Date, newSubscription.Date))
                return false;

            return true;
        }
        // sub1 71 <=
        // sub2 69 <
        private bool CheckFieldMatching(DoubleField existentField, DoubleField newField)
        {
            if (existentField == null)
                return true;

            switch (newField.Operator)
            {
                case "=":
                    if (existentField.Operator == "=")
                        if (existentField.Value.Equals(newField.Value))
                            return true;

                    return false;
                case "<":
                    if (existentField.Operator == "<")
                    {
                        if (existentField.Value <= newField.Value)
                            return true;

                    }
                    else if (existentField.Operator == "<=")
                    {
                        if (existentField.Value < newField.Value)
                            return true;
                    }

                    return false;
                case "<=":
                    if (existentField.Operator == "<=")
                    {
                        if (existentField.Value <= newField.Value)
                            return true;
                    }
                    else if (existentField.Operator == "<")
                    {
                        if (existentField.Value < newField.Value)
                            return true;
                    }

                    return false;
                case ">":
                    if (existentField.Operator == ">")
                    {
                        if (existentField.Value >= newField.Value)
                            return true;

                    }
                    else if (existentField.Operator == ">=")
                    {
                        if (existentField.Value > newField.Value)
                            return true;
                    }

                    return false;
                case ">=":
                    if (existentField.Operator == ">=")
                    {
                        if (existentField.Value >= newField.Value)
                            return true;
                    }
                    else if (existentField.Operator == ">")
                    {
                        if (existentField.Value > newField.Value)
                            return true;
                    }

                    return false;
            }

            return false;
        }

        private bool CheckFieldMatching(DateField existentField, DateField newField)
        {
            if (existentField == null)
                return true;

            switch (newField.Operator)
            {
                case "=":
                    if (existentField.Operator == "=")
                        if (existentField.Value.Equals(newField.Value))
                            return true;

                    return false;
                case "<":
                    if (existentField.Operator == "<")
                    {
                        if (existentField.Value <= newField.Value)
                            return true;

                    }
                    else if (existentField.Operator == "<=")
                    {
                        if (existentField.Value < newField.Value)
                            return true;
                    }

                    return false;
                case "<=":
                    if (existentField.Operator == "<=")
                    {
                        if (existentField.Value <= newField.Value)
                            return true;
                    }
                    else if (existentField.Operator == "<")
                    {
                        if (existentField.Value < newField.Value)
                            return true;
                    }

                    return false;
                case ">":
                    if (existentField.Operator == ">")
                    {
                        if (existentField.Value >= newField.Value)
                            return true;

                    }
                    else if (existentField.Operator == ">=")
                    {
                        if (existentField.Value > newField.Value)
                            return true;
                    }

                    return false;
                case ">=":
                    if (existentField.Operator == ">=")
                    {
                        if (existentField.Value >= newField.Value)
                            return true;
                    }
                    else if (existentField.Operator == ">")
                    {
                        if (existentField.Value > newField.Value)
                            return true;
                    }

                    return false;
            }

            return false;
        }

        private bool CheckFieldMatching(StringField existentField, StringField newField)
        {
            if (existentField == null)
                return true;

            switch (newField.Operator)
            {
                case "=":
                    if (existentField.Operator == "=")
                        if (existentField.Value.Equals(newField.Value))
                            return true;

                    return false;
                case "<":
                    if (existentField.Operator == "<")
                    {
                        if (existentField.Value.CompareTo(newField.Value) <= 0)
                            return true;

                    }
                    else if (existentField.Operator == "<=")
                    {
                        if (existentField.Value.CompareTo(newField.Value) < 0)
                            return true;
                    }

                    return false;
                case "<=":
                    if (existentField.Operator == "<=")
                    {
                        if (existentField.Value.CompareTo(newField.Value) >= 0)
                            return true;
                    }
                    else if (existentField.Operator == "<")
                    {
                        if (existentField.Value.CompareTo(newField.Value) > 0)
                            return true;
                    }

                    return false;
                case ">":
                    if (existentField.Operator == ">")
                    {
                        if (existentField.Value.CompareTo(newField.Value) <= 0)
                            return true;

                    }
                    else if (existentField.Operator == ">=")
                    {
                        if (existentField.Value.CompareTo(newField.Value) < 0)
                            return true;
                    }

                    return false;
                case ">=":
                    if (existentField.Operator == ">=")
                    {
                        if (existentField.Value.CompareTo(newField.Value) <= 0)
                            return true;
                    }
                    else if (existentField.Operator == ">")
                    {
                        if (existentField.Value.CompareTo(newField.Value) < 0)
                            return true;
                    }

                    return false;
            }

            return false;
        }

        // maybe find a way to use generics here
        public bool CompareField(DoubleField field, double pubValue)
        {
            switch (field.Operator)
            {
                case "=":
                    return field.Value == pubValue;
                case ">=":
                    return field.Value >= pubValue;
                case "<=":
                    return field.Value <= pubValue;
                case ">":
                    return field.Value > pubValue;
                case "<":
                    return field.Value < pubValue;
                case "!=":
                    return field.Value != pubValue;
                default:
                    return false;
            }
        }

        public bool CompareField(DateField field, DateTime pubValue)
        {
            switch (field.Operator)
            {
                case "=":
                    return field.Value == pubValue;
                case ">=":
                    return field.Value >= pubValue;
                case "<=":
                    return field.Value <= pubValue;
                case ">":
                    return field.Value > pubValue;
                case "<":
                    return field.Value < pubValue;
                case "!=":
                    return field.Value != pubValue;
                default:
                    return false;
            }
        }

        public bool CompareField(StringField field, string pubValue)
        {
            switch (field.Operator)
            {
                case "=":
                    return field.Value == pubValue;
                case ">=":
                    return field.Value.CompareTo(pubValue) <= 0;
                case "<=":
                    return field.Value.CompareTo(pubValue) >= 0;
                case ">":
                    return field.Value.CompareTo(pubValue) < 0;
                case "<":
                    return field.Value.CompareTo(pubValue) > 0;
                case "!=":
                    return field.Value != pubValue;
                default:
                    return false;
            }
        }
    }
}
