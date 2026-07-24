using Steam2Api.Dtos.Invoice;
using Steam2Api.Entities;

namespace Steam2Api.Mappers
{
    public static class InvoiceMapper
    {
        public static List<InvoiceDto> ListEntityToListDto(List<InvoiceEntity> entities)
        {
            return entities.Select(EntityToDto).ToList();
        }

        public static InvoiceDto EntityToDto(InvoiceEntity entity)
        {
            return new InvoiceDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                InvoiceDate = entity.InvoiceDate,
                Total = entity.Total,
                Details = entity.InvoiceDetails != null
                    ? DetailListEntityToListDto(entity.InvoiceDetails)
                    : new List<InvoiceDetailDto>()
            };
        }

        public static List<InvoiceDetailDto> DetailListEntityToListDto(List<InvoiceDetailEntity> entities)
        {
            return entities.Select(entity => new InvoiceDetailDto
            {
                Id = entity.Id,
                InvoiceId = entity.InvoiceId,
                GameId = entity.GameId,
                UnitPrice = entity.UnitPrice,
                Subtotal = entity.Subtotal
            }).ToList();
        }

        public static InvoiceDetailDto DetailEntityToDto(InvoiceDetailEntity entity)
        {
            return new InvoiceDetailDto
            {
                Id = entity.Id,
                InvoiceId = entity.InvoiceId,
                GameId = entity.GameId,
                UnitPrice = entity.UnitPrice,
                Subtotal = entity.Subtotal
            };
        }

        public static InvoiceDetailEntity DetailCreateDtoToEntity(string invoiceId, InvoiceDetailCreateDto dto)
        {
            return new InvoiceDetailEntity
            {
                Id = Guid.NewGuid().ToString(),
                InvoiceId = invoiceId,
                GameId = dto.GameId,
            };
        }
    }
}
