namespace EsbBroker.Models
{
    public interface ISubscriptionField<T>
    {
        public T Value { get; set; }

        public string Operator { get; set; }
    }
}
