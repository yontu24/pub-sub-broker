namespace EsbBroker.Models
{
    public class DoubleField : ISubscriptionField<double>
    {
        public double Value { get; set; }

        public string Operator { get; set; }
    }
}
