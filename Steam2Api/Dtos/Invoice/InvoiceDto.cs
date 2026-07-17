namespace Steam2Api.Dtos.Invoice
{
    public class InvoiceDto
    {
        public Guid UserId { get; set; }

        public DateTime InvoiceDate { get; set; }

        public decimal Total { get; set; }
    }
}