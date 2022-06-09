using System.Diagnostics.CodeAnalysis;

namespace EsbBroker.Models
{
    public class Subscription : IEqualityComparer<Subscription>
    {
        public Subscription() { }

        public Subscription(SubscriptionDTO subscriptionDTO)
        {
            Company = subscriptionDTO.Company;
            Value = subscriptionDTO.Value;
            Drop = subscriptionDTO.Drop;
            Variation = subscriptionDTO.Variation;
            Date = subscriptionDTO.Date;
        }

        public StringField Company { get; private set; }

        public DoubleField Value { get; private set; }

        public DoubleField Drop { get; private set; }

        public DoubleField Variation { get; private set; }

        public DateField Date { get; private set; }

        // changes needed here
        public bool Equals(Subscription x, Subscription y)
        {
            if (x == null || y == null)
                return false;

            return x.Company == y.Company && x.Value == y.Value && x.Drop == y.Drop && x.Variation == y.Variation && x.Date == y.Date;
        }

        public int GetHashCode([DisallowNull] Subscription obj)
        {
            return 1;
        }
    }
}
