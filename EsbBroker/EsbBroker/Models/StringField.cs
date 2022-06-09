namespace EsbBroker.Models
{
    public class StringField : ISubscriptionField<string>
    {
        public string Value { get; set; }
        public string Operator { get; set; }
    }
}
