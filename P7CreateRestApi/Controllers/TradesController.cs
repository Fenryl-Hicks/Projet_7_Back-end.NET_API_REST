using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using P7CreateRestApi.Dtos.Trades; // <-- Create/Update/Response/ListItem DTOs
using P7CreateRestApi.Entities;    // <-- Trade entity
using P7CreateRestApi.Services;    // <-- TradeService
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace P7CreateRestApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TradesController : ControllerBase
    {
        private readonly TradeService _service;
        private readonly ILogger<TradesController> _logger;
        private readonly IMapper _mapper;

        public TradesController(TradeService service, ILogger<TradesController> logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradeListItemDto>>> GetAll(CancellationToken ct)
        {
            var entities = await _service.GetAllAsync();
            var list = _mapper.Map<IEnumerable<TradeListItemDto>>(entities);
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TradeResponseDto>> Get(int id, CancellationToken ct)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity is null) return NotFound();
            return Ok(_mapper.Map<TradeResponseDto>(entity));
        }

        [HttpPost]
        public async Task<ActionResult<TradeResponseDto>> Create([FromBody] CreateTradeRequestDto dto, CancellationToken ct)
        {
            var entity = _mapper.Map<Trade>(dto);
            var created = await _service.CreateAsync(entity);
            var response = _mapper.Map<TradeResponseDto>(created);

            _logger.LogInformation("Trade created with id {Id} for account {Account}", response.Id, response.Account);

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTradeRequestDto dto, CancellationToken ct)
        {
            var entity = _mapper.Map<Trade>(dto);
            var updated = await _service.UpdateAsync(id, entity);
            if (!updated) return NotFound();

            _logger.LogInformation("Trade updated with id {Id}", id);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            _logger.LogInformation("Trade deleted with id {Id}", id);
            return NoContent();
        }
    }
}
