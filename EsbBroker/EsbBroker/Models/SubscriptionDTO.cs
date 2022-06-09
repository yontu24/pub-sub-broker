namespace EsbBroker.Models
{
    public class SubscriptionDTO
    {
        public int Sender { get; set; }

        public StringField Company { get; set; }

        public DoubleField Value { get; set; }

        public DoubleField Drop { get; set; }

        public DoubleField Variation { get; set; }

        public DateField Date { get; set; }
    }
}
