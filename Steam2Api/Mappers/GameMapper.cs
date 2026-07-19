using Steam2Api.Dtos.Game;
using Steam2Api.Entities;

namespace Steam2Api.Mappers
{
    public static class GameMapper
    {
        public static GameEntity CreateDtoToEntity(GameCreateDto dto)
        {
            return new GameEntity
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Genre = dto.Genre,
                Price = dto.Price,
                State = dto.State,
                Description = dto.Description
            };
        }

        public static GameEntity EditDtoToEntity(GameEntity entity, GameEditDto dto)
        {
            entity.Name = dto.Name;
            entity.Genre = dto.Genre;
            entity.Price = dto.Price;
            entity.State = dto.State;
            entity.Description = dto.Description;

            return entity;
        }
    }
}