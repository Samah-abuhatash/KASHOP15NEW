using KaSHop.DAL.Repositories;
using KaSHop.DAL.models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KaSHop.pl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IGenericRepository<Brands> repository;

        public BrandsController(IGenericRepository<Brands> repository)
        {
            this.repository = repository;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var brands = await repository.GetAllAsync();
            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await repository.GetByIdAsync(id);

            if (brand == null)
            {
                return NotFound();
            }

            return Ok(brand);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] Brands brand)
        {
            await repository.CreateAsync(brand);
            return Ok();
        }
    }
}