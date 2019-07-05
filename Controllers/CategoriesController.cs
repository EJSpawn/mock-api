using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace mock_api.Controllers
{
    public class Category{
        public int id { get; set; }
        public string nmCategory { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        List<Category> categoryRepo = new List<Category>();

        public CategoriesController (){
            for(var index = 1; index <= 20; index ++ ){
                var category = new Category(){ id = index, nmCategory = "Category " + index };
                categoryRepo.Add(category);
            }
        }
        
        // GET api/categories
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(categoryRepo);
        }

        // GET api/categories/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return Ok(categoryRepo.Where(c => c.id == id).ToList());
        }

        // POST api/categories
        [HttpPost]
        public void Post([FromBody] string nmCategory)
        {
        }

        // PUT api/categories/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string nmCategory)
        {
        }

        // DELETE api/categories/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
