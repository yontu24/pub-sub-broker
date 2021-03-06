namespace EsbBroker.Models
{
    public class PublicationDTO
    {
        public int Sender { get; set; }
        public int OriginalSender { get; set; }

        public string Company { get; set; }

        public double Value { get; set; }

        public double Drop { get; set; }

        public double Variation { get; set; }

        public DateTime Date { get; set; }
    }
}
