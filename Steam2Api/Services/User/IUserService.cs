using Steam2Api.Dtos.Common;
using Steam2Api.Dtos.User;

namespace Steam2Api.Services.User
{
    public interface IUserService
    {
        Task<ResponseDto<List<UserDto>>> GetAllAsync();
        Task<ResponseDto<UserDto>> GetOneByIdAsync(string id);
        Task<ResponseDto<UserActionResponseDto>> CreateAsync(UserCreateDto dto);
        Task<ResponseDto<UserActionResponseDto>> EditAsync(string id, UserEditDto dto);
        Task<ResponseDto<UserActionResponseDto>> DeleteAsync(string id);
    }
}
