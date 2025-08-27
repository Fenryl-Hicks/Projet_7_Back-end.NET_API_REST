using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using P7CreateRestApi.Dtos.Bids;   // <-- Create/Update/Response/ListItem DTOs
using P7CreateRestApi.Entities;    // <-- Bid entity
using P7CreateRestApi.Services;    // <-- BidService
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace P7CreateRestApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BidsController : ControllerBase
    {
        private readonly BidService _service;
        private readonly ILogger<BidsController> _logger;
        private readonly IMapper _mapper;

        public BidsController(BidService service, ILogger<BidsController> logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BidListItemDto>>> GetAll(CancellationToken ct)
        {
            var entities = await _service.GetAllAsync();
            var list = _mapper.Map<IEnumerable<BidListItemDto>>(entities);
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BidResponseDto>> Get(int id, CancellationToken ct)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity is null) return NotFound();
            return Ok(_mapper.Map<BidResponseDto>(entity));
        }

        [HttpPost]
        public async Task<ActionResult<BidResponseDto>> Create([FromBody] CreateBidRequestDto dto, CancellationToken ct)
        {
            // [ApiController] => 400 auto si dto invalide
            var entity = _mapper.Map<Bid>(dto);
            var created = await _service.CreateAsync(entity);
            var response = _mapper.Map<BidResponseDto>(created);

            _logger.LogInformation("Bid created with id {Id} for account {Account}", response.Id, response.Account);

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBidRequestDto dto, CancellationToken ct)
        {
            var entity = _mapper.Map<Bid>(dto);
            var updated = await _service.UpdateAsync(id, entity);

            if (!updated) return NotFound();

            _logger.LogInformation("Bid updated with id {Id}", id);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            _logger.LogInformation("Bid deleted with id {Id}", id);
            return NoContent();
        }
    }
}
