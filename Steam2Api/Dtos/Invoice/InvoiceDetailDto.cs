namespace Steam2Api.Dtos.Invoice
{
    public class InvoiceDetailDto
    {
        public Guid InvoiceId { get; set; }

        public Guid GameId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Subtotal { get; set; }
    }
}