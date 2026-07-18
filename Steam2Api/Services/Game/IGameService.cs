using Steam2Api.Dtos.Common;
using Steam2Api.Dtos.Game;
using Steam2Api.Entities;

namespace Steam2Api.Services.Game
{
    public interface IGameService
    {
        Task<ResponseDto<List<GameDto>>> GetPageAsync(string searchTerm = "", int page = 1, int pageSize = 10);
        Task<ResponseDto<GameDto>> GetOneByIdAsync(string id);
        Task<ResponseDto<GameActionResponseDto>> CreateAsync(GameCreateDto dto);
        Task<ResponseDto<GameActionResponseDto>> EditAsync(string id, GameEditDto dto);
    }
}