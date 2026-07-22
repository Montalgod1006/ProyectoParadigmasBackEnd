using Steam2Api.Dtos.Game;
using Steam2Api.Entities;

namespace Steam2Api.Mappers
{
    public static class GameMapper
    {
        public static List <GameDto> ListEntityToListDto(List<GameEntity> entities)
        { 
            return entities.Select(game => new GameDto
            {
                Id =  game.Id,
                Name = game.Name,
                Genre = game.Genre,
                Price = game.Price,
                State = game.State,
                Description = game.Description,
                ImageUrl = game.ImageUrl,
            }).ToList();
        }
        public static GameDto EntityToDto(GameEntity entity)
        {
            return new GameDto
            {
                Id =  entity.Id,
                Name = entity.Name,
                Genre = entity.Genre,
                Price = entity.Price,
                State = entity.State,
                Description = entity.Description,
                ImageUrl = entity.ImageUrl,
            };
        }
        public static GameEntity CreateDtoToEntity(GameCreateDto dto)
        {
            return new GameEntity
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Genre = dto.Genre,
                Price = dto.Price,
                State = dto.State,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl
            };
        }

        public static GameEntity EditDtoToEntity(GameEntity entity, GameEditDto dto)
        {
            entity.Name = dto.Name;
            entity.Genre = dto.Genre;
            entity.Price = dto.Price;
            entity.State = dto.State;
            entity.Description = dto.Description;
            entity.ImageUrl = dto.ImageUrl;

            return entity;
        }
    }
}
