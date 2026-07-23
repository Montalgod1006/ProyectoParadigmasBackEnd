using Microsoft.AspNetCore.Mvc;
using Steam2Api.Dtos.Invoice;
using Steam2Api.Services.Invoice;

namespace Steam2Api.Controller
{
    [ApiController]
    [Route("api/invoice")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _invoiceService.GetAllAsync();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneById(string id)
        {
            var response = await _invoiceService.GetOneByIdAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InvoiceCreateDto dto)
        {
            var response = await _invoiceService.CreateAsync(dto);
            return StatusCode((int)response.StatusCode, response);
        }

    
        }
    }

