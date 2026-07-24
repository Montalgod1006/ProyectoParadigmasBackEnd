using Steam2Api.Entities;

namespace Steam2Api.Dtos.Invoice
{
    public class InvoiceDto : BaseEntity
    {
        public string UserId { get; set; }

        public DateTime InvoiceDate { get; set; }

        public decimal Total { get; set; }

        public List<InvoiceDetailDto> Details { get; set; }
    }
}
