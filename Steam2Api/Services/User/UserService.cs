using Microsoft.EntityFrameworkCore;
using Steam2Api.Constants;
using Steam2Api.Data;
using Steam2Api.Dtos.Common;
using Steam2Api.Dtos.User;
using Steam2Api.Mappers;

namespace Steam2Api.Services.User
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseDto<List<UserDto>>> GetAllAsync()
        {
            var users = await _context.Users
                .Include(u => u.Invoices)
                .Include(u => u.Games)
                .ToListAsync();
            return new ResponseDto<List<UserDto>>
            {
                StatusCode = HttpStatusCode.Ok,
                Status = true,
                Message = HttpMessageResponse.REGISTERS_FOUND,
                Data = UserMapper.ListEntityToListDto(users)
            };
        }

        public async Task<ResponseDto<UserDto>> GetOneByIdAsync(string id)
        {
            var user = await _context.Users
                .Include(u => u.Invoices)
                .Include(u => u.Games)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (user is null)
            {
                return new ResponseDto<UserDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = HttpMessageResponse.REGISTER_NOT_FOUND
                };
            }

            return new ResponseDto<UserDto>
            {
                StatusCode = HttpStatusCode.Ok,
                Status = true,
                Message = HttpMessageResponse.REGISTER_FOUND,
                Data = UserMapper.EntityToDto(user)
            };
        }

        public async Task<ResponseDto<UserActionResponseDto>> CreateAsync(UserCreateDto dto)
        {
            var user = UserMapper.CreateDtoToEntity(dto);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return new ResponseDto<UserActionResponseDto>
            {
                StatusCode = HttpStatusCode.CREATED,
                Status = true,
                Message = HttpMessageResponse.REGISTER_CREATED,
                Data = new UserActionResponseDto { Id = user.Id }
            };
        }

        public async Task<ResponseDto<UserActionResponseDto>> EditAsync(string id, UserEditDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user is null)
            {
                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = HttpMessageResponse.REGISTER_NOT_FOUND
                };
            }

            UserMapper.EditDtoToEntity(user, dto);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new ResponseDto<UserActionResponseDto>
            {
                StatusCode = HttpStatusCode.Ok,
                Status = true,
                Message = HttpMessageResponse.REGISTER_UPDATED,
                Data = new UserActionResponseDto { Id = id }
            };
        }

        public async Task<ResponseDto<UserActionResponseDto>> DeleteAsync(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user is null)
            {
                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = HttpMessageResponse.REGISTER_NOT_FOUND
                };
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return new ResponseDto<UserActionResponseDto>
            {
                StatusCode = HttpStatusCode.Ok,
                Status = true,
                Message = HttpMessageResponse.REGISTER_DELETED,
                Data = new UserActionResponseDto { Id = id }
            };
        }
    }
}
