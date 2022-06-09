namespace EbsSubscriber.Services
{
    public interface ISubscriptionService
    {
        public void SendSubscriptionsAsync(int ownPort);
    }
}
