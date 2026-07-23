namespace Steam2Api.Dtos.Invoice
{
    public class InvoiceDto
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public DateTime InvoiceDate { get; set; }

        public decimal Total { get; set; }

        public List<InvoiceDetailDto> Details { get; set; }
    }
}
