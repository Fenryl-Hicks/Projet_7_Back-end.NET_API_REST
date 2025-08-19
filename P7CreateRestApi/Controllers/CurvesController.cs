using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using P7CreateRestApi.Dtos.Curves;   // <-- Create/Update/Response/ListItem DTOs
using P7CreateRestApi.Entities;      // <-- CurvePoint entity
using P7CreateRestApi.Services;      // <-- CurvePointService
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurvesController : ControllerBase
    {
        private readonly CurvePointService _service;
        private readonly ILogger<CurvesController> _logger;
        private readonly IMapper _mapper;

        public CurvesController(CurvePointService service, ILogger<CurvesController> logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurvePointListItemDto>>> GetAll(CancellationToken ct)
        {
            var entities = await _service.GetAllAsync();
            var list = _mapper.Map<IEnumerable<CurvePointListItemDto>>(entities);
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CurvePointResponseDto>> Get(int id, CancellationToken ct)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity is null) return NotFound();
            return Ok(_mapper.Map<CurvePointResponseDto>(entity));
        }

        [HttpPost]
        public async Task<ActionResult<CurvePointResponseDto>> Create([FromBody] CreateCurvePointRequestDto dto, CancellationToken ct)
        {
            // [ApiController] => 400 auto si dto invalide
            var entity = _mapper.Map<CurvePoint>(dto);
            var created = await _service.CreateAsync(entity);
            var response = _mapper.Map<CurvePointResponseDto>(created);

            _logger.LogInformation("CurvePoint created with id {Id}", response.Id);

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCurvePointRequestDto dto, CancellationToken ct)
        {
            var entity = _mapper.Map<CurvePoint>(dto);
            var updated = await _service.UpdateAsync(id, entity);

            if (!updated) return NotFound();

            _logger.LogInformation("CurvePoint updated with id {Id}", id);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            _logger.LogInformation("CurvePoint deleted with id {Id}", id);
            return NoContent();
        }
    }
}
