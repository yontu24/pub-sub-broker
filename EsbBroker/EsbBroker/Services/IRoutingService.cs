using EsbBroker.Models;

namespace EsbBroker.Services
{
    public interface IRoutingService
    {
        public void RegisterSubscriptionFromSubscriber(SubscriptionDTO subscriptionDTO, int ownPort);
        public void RegisterSubscriptionFromBroker(SubscriptionDTO subscriptionDTO, int ownPort);
        public void MatchPublicationAsync(PublicationDTO publicationDTO, bool searchDirect, int ownPort);
        public void AddPublication(PublicationDTO publicationDTO);
        public void PrintSubscriptions(int ownPort);
        public void PrintPublications(int ownPort);
    }
}
