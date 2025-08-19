using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using P7CreateRestApi.Dtos.RuleNames; // <-- Create/Update/Response/ListItem DTOs
using P7CreateRestApi.Entities;       // <-- RuleName entity
using P7CreateRestApi.Services;       // <-- RuleNameService
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RuleNamesController : ControllerBase
    {
        private readonly RuleNameService _service;
        private readonly ILogger<RuleNamesController> _logger;
        private readonly IMapper _mapper;

        public RuleNamesController(RuleNameService service, ILogger<RuleNamesController> logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RuleNameListItemDto>>> GetAll(CancellationToken ct)
        {
            var entities = await _service.GetAllAsync();
            var list = _mapper.Map<IEnumerable<RuleNameListItemDto>>(entities);
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RuleNameResponseDto>> Get(int id, CancellationToken ct)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity is null) return NotFound();
            return Ok(_mapper.Map<RuleNameResponseDto>(entity));
        }

        [HttpPost]
        public async Task<ActionResult<RuleNameResponseDto>> Create([FromBody] CreateRuleNameRequestDto dto, CancellationToken ct)
        {
            var entity = _mapper.Map<RuleName>(dto);
            var created = await _service.CreateAsync(entity);
            var response = _mapper.Map<RuleNameResponseDto>(created);

            _logger.LogInformation("RuleName created with id {Id}", response.Id);

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRuleNameRequestDto dto, CancellationToken ct)
        {
            var entity = _mapper.Map<RuleName>(dto);
            var updated = await _service.UpdateAsync(id, entity);

            if (!updated) return NotFound();

            _logger.LogInformation("RuleName updated with id {Id}", id);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            _logger.LogInformation("RuleName deleted with id {Id}", id);
            return NoContent();
        }
    }
}
