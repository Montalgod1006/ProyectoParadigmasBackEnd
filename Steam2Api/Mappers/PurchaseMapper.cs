using Steam2Api.Dtos.Purchase;
using Steam2Api.Entities;

namespace Steam2Api.Mappers
{
    public static class PurchaseMapper
    {
        public static InvoiceEntity CreateDtoToInvoiceEntity(PurchaseCreateDto dto, decimal total)
        {
            return new InvoiceEntity
            {
                Id = Guid.NewGuid().ToString(),
                UserId = dto.UserId,
                InvoiceDate = DateTime.UtcNow,
                Total = total
            };
        }

        public static InvoiceDetailEntity CreateDetailEntity(string invoiceId, string gameId, decimal unitPrice)
        {
            return new InvoiceDetailEntity
            {
                Id = Guid.NewGuid().ToString(),
                InvoiceId = invoiceId,
                GameId = gameId,
                UnitPrice = unitPrice,
                Subtotal = unitPrice
            };
        }
    }
}
