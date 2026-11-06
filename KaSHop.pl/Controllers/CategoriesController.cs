using KaSHop.BLL;
using KaSHop.DAL.DTO.Requet;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace KaSHop.pl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(
            CancellationToken cancellationToken)
        {
            var categories = await _service.GetAllAsync(cancellationToken);

            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _service.GetByIdAsync(id, cancellationToken);

            return result is not null ? Ok(result) : BadRequest();
        }

        [HttpPost("")]
        public async Task<IActionResult> Create(
      CategoryRequest request,
      CancellationToken cancellationToken)
        {
            var result = await _service.CreateAsync(request, cancellationToken);

            return result ? Ok() : BadRequest();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
    int id,
    CancellationToken cancellationToken)
        {
            var result = await _service.DeleteAsync(id, cancellationToken);

            return result ? Ok() : BadRequest();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
    int id,
    [FromBody] CategoryRequest request,
    CancellationToken cancellationToken)
        {
            var result = await _service.UpdateAsync(
                id,
                request,
                cancellationToken
            );

            return result ? Ok() : BadRequest();
        }
    }
}