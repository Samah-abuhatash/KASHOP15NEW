using KaSHop.DAL.models;
using KaSHop.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace KaSHop.pl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        GenericRepository<Category> repository = new GenericRepository<Category>();

        [HttpGet("")]
        public IActionResult Index()
        {
            var categories = repository.GetAll();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = repository.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost("")]
        public IActionResult Create(Category category)
        {
            repository.Create(category);
            return Ok();
        }
    }
}