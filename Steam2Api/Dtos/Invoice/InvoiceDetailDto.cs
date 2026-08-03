namespace Steam2Api.Dtos.Invoice
{
    public class InvoiceDetailDto
    {
        public string Id { get; set; }

        public string InvoiceId { get; set; }

        public string GameId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Subtotal { get; set; }
    }
}
