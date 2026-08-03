using Steam2Api.Dtos.Game;
using Steam2Api.Dtos.Invoice;
using Steam2Api.Dtos.User;
using Steam2Api.Entities;

namespace Steam2Api.Mappers
{
    public static class UserMapper
    {
        public static List<UserDto> ListEntityToListDto(List<UserEntity> entities)
        {
            return entities.Select(user => new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Invoices = user.Invoices != null
                    ? user.Invoices.Select(InvoiceMapper.EntityToDto).ToList()
                    : new List<InvoiceDto>(),
               Games = user.Games != null
                    ? GameMapper.ListEntityToListDto(user.Games)
                    : new List<GameDto>()
            }).ToList();
        }

        public static UserDto EntityToDto(UserEntity entity)
        {
            return new UserDto
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Invoices = entity.Invoices != null
                    ? entity.Invoices.Select(InvoiceMapper.EntityToDto).ToList()
                    : new List<InvoiceDto>(),
                Games = entity.Games != null
                    ? GameMapper.ListEntityToListDto(entity.Games)
                    : new List<GameDto>()
            };
        }

        public static UserEntity CreateDtoToEntity(UserCreateDto dto)
        {
            return new UserEntity
            {
                Id = Guid.NewGuid().ToString(),
                UserName = dto.UserName,
                Password = dto.Password
            };
        }

        public static UserEntity EditDtoToEntity(UserEntity entity, UserEditDto dto)
        {
            entity.UserName = dto.UserName;
            entity.Password = dto.Password;
            return entity;
        }
    }
}
