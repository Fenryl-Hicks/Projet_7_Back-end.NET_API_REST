using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Entities;
using P7CreateRestApi.Services;
using System.Threading.Tasks;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurvesController : ControllerBase
    {
        private readonly CurvePointService _service;

        public CurvesController(CurvePointService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var curves = await _service.GetAllAsync();
            return Ok(curves);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var curve = await _service.GetByIdAsync(id);
            if (curve == null)
                return NotFound();
            return Ok(curve);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CurvePoint curve)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(curve);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CurvePoint curve)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, curve);
            if (!updated)
                return NotFound();

            return Ok(curve);
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
