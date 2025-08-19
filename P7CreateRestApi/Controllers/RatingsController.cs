using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using P7CreateRestApi.Dtos.Ratings; // <-- Create/Update/Response/ListItem DTOs
using P7CreateRestApi.Entities;     // <-- Rating entity
using P7CreateRestApi.Services;     // <-- RatingService
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly RatingService _service;
        private readonly ILogger<RatingsController> _logger;
        private readonly IMapper _mapper;

        public RatingsController(RatingService service, ILogger<RatingsController> logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RatingListItemDto>>> GetAll(CancellationToken ct)
        {
            var entities = await _service.GetAllAsync();
            var list = _mapper.Map<IEnumerable<RatingListItemDto>>(entities);
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RatingResponseDto>> Get(int id, CancellationToken ct)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity is null) return NotFound();
            return Ok(_mapper.Map<RatingResponseDto>(entity));
        }

        [HttpPost]
        public async Task<ActionResult<RatingResponseDto>> Create([FromBody] CreateRatingRequestDto dto, CancellationToken ct)
        {
            // [ApiController] => 400 auto si dto invalide
            var entity = _mapper.Map<Rating>(dto);
            var created = await _service.CreateAsync(entity);
            var response = _mapper.Map<RatingResponseDto>(created);

            _logger.LogInformation("Rating created with id {Id} (OrderNumber={Order})", response.Id, response.OrderNumber);

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRatingRequestDto dto, CancellationToken ct)
        {
            var entity = _mapper.Map<Rating>(dto);
            var updated = await _service.UpdateAsync(id, entity);

            if (!updated) return NotFound();

            _logger.LogInformation("Rating updated with id {Id}", id);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            _logger.LogInformation("Rating deleted with id {Id}", id);
            return NoContent();
        }
    }
}
