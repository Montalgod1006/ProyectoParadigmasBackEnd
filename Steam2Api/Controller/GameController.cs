using Microsoft.AspNetCore.Mvc;
using Steam2Api.Dtos.Game;
using Steam2Api.Services.Game;

namespace Steam2Api.Controller
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string searchTerm = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _gameService.GetPageAsync(searchTerm, page, pageSize);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneById(string id)
        {
            var response = await _gameService.GetOneByIdAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GameCreateDto dto)
        {
            var response = await _gameService.CreateAsync(dto);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] GameEditDto dto)
        {
            var response = await _gameService.EditAsync(id, dto);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _gameService.DeleteAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
