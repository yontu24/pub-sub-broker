namespace EbsPublisher.Models
{
    public class PublicationDTO
    {
        public PublicationDTO(Publication publication, int sender)
        {
            OriginSender = 0;
            Sender = sender;
            Company = publication.Company;
            Value = publication.Value;
            Drop = publication.Drop;
            Variation = publication.Variation;
            Date = publication.Date;
        }

        public int OriginSender { get; set; }
        public int Sender { get; set; }

        public string Company { get; set; }

        public double Value { get; set; }

        public double Drop { get; set; }

        public double Variation { get; set; }

        public DateTime Date { get; set; }
    }
}
