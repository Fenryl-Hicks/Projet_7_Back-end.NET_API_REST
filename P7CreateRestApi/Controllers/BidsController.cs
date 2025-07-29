using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Entities;
using P7CreateRestApi.Services;
using System.Threading.Tasks;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BidsController : ControllerBase
    {
        private readonly BidService _service;

        public BidsController(BidService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bids = await _service.GetAllAsync();
            return Ok(bids);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var bid = await _service.GetByIdAsync(id);
            if (bid == null)
                return NotFound();
            return Ok(bid);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Bid bid)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(bid);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Bid bid)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, bid);
            if (!updated)
                return NotFound();

            return Ok(bid);
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
