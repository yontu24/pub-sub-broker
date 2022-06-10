using EbsSubscriber.Models;

namespace EbsSubscriber.Services
{
    public interface ISubscriptionService
    {
        public void SendSubscriptionsAsync(int ownPort);

        public void AddPublication(PublicationDTO publicationDTO);

        public void PrintReceivedPublications(int ownPort);
    }
}
