using KaSHop.DAL.models;
using KaSHop.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace KaSHop.pl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        GenericRepository<Brands> repository = new GenericRepository<Brands>();

        [HttpGet("")]
        public IActionResult Index()
        {
            var brands = repository.GetAll();
            return Ok(brands);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var brand = repository.GetById(id);

            if (brand == null)
            {
                return NotFound();
            }

            return Ok(brand);
        }

        [HttpPost("")]
        public IActionResult Create(Brands brand)
        {
            repository.Create(brand);
            return Ok();
        }
    }
}