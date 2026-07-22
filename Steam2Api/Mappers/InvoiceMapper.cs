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

        public static InvoiceEntity CreateDtoToEntity(InvoiceCreateDto dto)
        {
            return new InvoiceEntity
            {
                Id = Guid.NewGuid().ToString(),
                UserId = dto.UserId,
                InvoiceDate = dto.InvoiceDate,
                Total = 0
            };
        }

        public static InvoiceEntity EditDtoToEntity(InvoiceEntity entity, InvoiceEditDto dto)
        {
            entity.UserId = dto.UserId;
            entity.InvoiceDate = dto.InvoiceDate;
            return entity;
        }

        public static List<InvoiceDetailDto> DetailListEntityToListDto(List<InvoiceDetailEntity> entities)
        {
            return entities.Select(entity => new InvoiceDetailDto
            {
                Id = entity.Id,
                InvoiceId = entity.InvoiceId,
                GameId = entity.GameId,
                Quantity = entity.Quantity,
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
                Quantity = entity.Quantity,
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
                Quantity = dto.Quantity
            };
        }

        public static InvoiceDetailEntity DetailEditDtoToEntity(InvoiceDetailEntity entity, InvoiceDetailEditDto dto)
        {
            entity.InvoiceId = dto.InvoiceId;
            entity.GameId = dto.GameId;
            entity.Quantity = dto.Quantity;
            return entity;
        }
    }
}
