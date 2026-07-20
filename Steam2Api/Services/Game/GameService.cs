using Microsoft.EntityFrameworkCore;
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

        public async Task<ResponseDto<PageDto<List<GameDto>>>> GetPageAsync(string searchTerm = "", int page = 1, int pageSize = 10)
        {
            page = Math.Abs(page);
            pageSize = Math.Abs(pageSize);
            pageSize = pageSize <= 0 ? PAGE_SIZE : pageSize;
            pageSize = pageSize > PAGE_SIZE_LIMIT ? PAGE_SIZE_LIMIT : pageSize;

            int startIndex = (page - 1)* pageSize;

            IQueryable<GameEntity> gamesQuery = _context.Games;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                gamesQuery = gamesQuery.Where( x => (x.Id + " " + x.Name ).Contains(searchTerm) );
            }

            int totalRows = await gamesQuery.CountAsync(); //Esto me devuelve el conteo de todos los registros basados en el criterio de búsqueda

            var gamesEntity = await gamesQuery  
                .OrderBy(x => x.Name)
                .Skip(startIndex)
                .Take(pageSize)
                .ToListAsync();

           return new ResponseDto<PageDto<List<GameDto>>>
            {
                StatusCode = HttpStatusCode.Ok,
                Status = true,
                Message = HttpMessageResponse.REGISTERS_FOUND,
                Data = new PageDto<List<GameDto>>
                {
                    CurrentPage = page == 0 ? 1 : page,
                    PageSize = pageSize,
                    TotalItems = totalRows,
                    TotalPages = (int)Math.Ceiling((double)totalRows/pageSize),
                    Items = GameMapper.ListEntityToListDto(gamesEntity),
                    HasNextPage = startIndex +pageSize < PAGE_SIZE_LIMIT && 
                        page < (int)Math.Ceiling((double)totalRows/pageSize),
                    HasPreviousPage = page > 1
                }
            };
        }

        public async Task<ResponseDto<GameDto>> GetOneByIdAsync(string id)
        {
            var gameEntity = await _context.Games
            .FirstOrDefaultAsync(
               p => p.Id == id
            );
            if (gameEntity is null)
            {
                return new ResponseDto<GameDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Message = HttpMessageResponse.REGISTER_NOT_FOUND,
                    Status = false,
                };
            }
            return new ResponseDto<GameDto>
            {
                StatusCode = HttpStatusCode.Ok,
                Message = HttpMessageResponse.REGISTER_FOUND,
                Status = true,
                Data = GameMapper.EntityToDto(gameEntity),
            };
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

        public async Task<ResponseDto<GameActionResponseDto>> EditAsync(string id, GameEditDto dto)
        {
            var gameEntity = await _context.Games.FirstOrDefaultAsync(p => p.Id == id );

            if (gameEntity is null)
            {
                return new ResponseDto<GameActionResponseDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = HttpMessageResponse.REGISTER_NOT_FOUND
                };
            }

            var gameEntityUpdated = GameMapper.EditDtoToEntity(gameEntity, dto);

            _context.Games.Update(gameEntityUpdated);

            await _context.SaveChangesAsync();

            return new ResponseDto<GameActionResponseDto>
            {
                StatusCode = HttpStatusCode.Ok,
                Status = true,
                Message = HttpMessageResponse.REGISTER_UPDATED,
                Data = new GameActionResponseDto
                {
                    Id = id
                }
            };
        }

        public async Task<ResponseDto<GameActionResponseDto>> DeleteAsync(string id)
        {
            var gameEntity = await _context.Games.FirstOrDefaultAsync(p => p.Id == id);

            if (gameEntity is null)
            {
                return new ResponseDto<GameActionResponseDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = HttpMessageResponse.REGISTER_NOT_FOUND
                };
            }

            _context.Games.Remove(gameEntity);
            await _context.SaveChangesAsync();

            return new ResponseDto<GameActionResponseDto>
            {
                StatusCode = HttpStatusCode.Ok,
                Status = true,
                Message = HttpMessageResponse.REGISTER_DELETED,
                Data = new GameActionResponseDto
                {
                    Id = id
                }
            };
        }
    }
}