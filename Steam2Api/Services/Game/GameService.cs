using Steam2Api.Constants;
using Steam2Api.Data;
using Steam2Api.Dtos.Common;
using Steam2Api.Dtos.Game;
using Steam2Api.Entities;
using Steam2Api.Mappers;
namespace Steam2Api.Services.Game
{
    public class GameService : IGameService
    {
        private readonly AppDbContext _context;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;

        public GameService(
            AppDbContext context,
            IConfiguration configuration
        )
        {
            _context = context;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
            PAGE_SIZE_LIMIT = configuration.GetValue<int>("PageSizeLimit");
        }

        public Task<ResponseDto<List<GameDto>>> GetPageAsync(string searchTerm = "", int page = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<GameDto>> GetOneByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<GameActionResponseDto>> CreateAsync(GameCreateDto dto)
        {
            GameEntity gameEntity = GameMapper.CreateDtoToEntity(dto);

            _context.Games.Add(gameEntity);

            await _context.SaveChangesAsync();

            var response = new ResponseDto<GameActionResponseDto>
            {
                StatusCode = HttpStatusCode.Ok,
                Status = true,
                Message = HttpMessageResponse.REGISTER_CREATED,
                Data = new GameActionResponseDto
                {
                    Id = gameEntity.Id
                }
            };

            return response;
        }

        public Task<ResponseDto<GameActionResponseDto>> EditAsync(string id, GameEditDto dto)
        {
            throw new NotImplementedException();
        }

    }
}