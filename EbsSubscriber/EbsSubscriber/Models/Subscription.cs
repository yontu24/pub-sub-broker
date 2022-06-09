namespace EbsSubscriber.Models
{
    public class Subscription
    {
        public StringField Company { get; set; }

        public DoubleField Value { get; set; }

        public DoubleField Drop { get; set; }

        public DoubleField Variation { get; set; }

        public DateField Date { get; set; }
    }
}
