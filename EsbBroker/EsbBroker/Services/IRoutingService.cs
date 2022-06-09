using EsbBroker.Models;

namespace EsbBroker.Services
{
    public interface IRoutingService
    {
        public void RegisterSubscriptionFromSubscriber(SubscriptionDTO subscriptionDTO, int ownPort);
        public void RegisterSubscriptionFromBroker(SubscriptionDTO subscriptionDTO, int ownPort);
        public void MatchPublicationAsync(PublicationDTO publicationDTO, bool searchDirect);
    }
}
