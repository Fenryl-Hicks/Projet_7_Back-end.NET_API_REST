using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Entities;
using P7CreateRestApi.Services;
using System.Threading.Tasks;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RuleNamesController : ControllerBase
    {
        private readonly RuleNameService _service;

        public RuleNamesController(RuleNameService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rules = await _service.GetAllAsync();
            return Ok(rules);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var rule = await _service.GetByIdAsync(id);
            if (rule == null)
                return NotFound();
            return Ok(rule);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RuleName rule)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(rule);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RuleName rule)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, rule);
            if (!updated)
                return NotFound();

            return Ok(rule);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
