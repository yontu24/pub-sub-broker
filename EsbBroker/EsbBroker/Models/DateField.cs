namespace EsbBroker.Models
{
    public class DateField : ISubscriptionField<DateTime>
    {
        public DateTime Value { get; set; }

        public string Operator { get; set; }
    }
}
