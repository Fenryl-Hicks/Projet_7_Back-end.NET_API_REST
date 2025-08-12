using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Entities;
using P7CreateRestApi.Services;
using System.Threading.Tasks;

namespace P7CreateRestApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TradesController : ControllerBase
    {
        private readonly TradeService _service;

        public TradesController(TradeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var trades = await _service.GetAllAsync();
            return Ok(trades);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var trade = await _service.GetByIdAsync(id);
            if (trade == null)
                return NotFound();
            return Ok(trade);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Trade trade)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(trade);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Trade trade)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, trade);
            if (!updated)
                return NotFound();

            return Ok(trade);
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
