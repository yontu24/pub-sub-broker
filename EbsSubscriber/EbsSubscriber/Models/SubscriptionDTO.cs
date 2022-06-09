namespace EbsSubscriber.Models
{
    public class SubscriptionDTO
    {
        public SubscriptionDTO(Subscription subscription, int sender)
        {
            Company = subscription.Company;
            Value = subscription.Value;
            Drop = subscription.Drop;
            Variation = subscription.Variation;
            Date = subscription.Date;
            Sender = sender;
        }

        public int Sender { get; set; }

        public StringField Company { get; set; }

        public DoubleField Value { get; set; }

        public DoubleField Drop { get; set; }

        public DoubleField Variation { get; set; }

        public DateField Date { get; set; }
    }
}
