using Microsoft.AspNetCore.Mvc;
using Shop.Models;

[Route("categories")]
public class CategoryController :  ControllerBase {

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Category>>> GetCategory() {
        return new List<Category>();
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> GetByIdCategory(string id) {
        return new Category();
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<List<Category>>> PostCategory([FromBody]Category model) {
        return Ok(model);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<List<Category>>> PutCategory(int id, [FromBody]Category model) {
        if(model.Id == id)
            return Ok(model);
        return NotFound();
    }

    [HttpDelete]
    [Route("{id:int}")]
    public  async Task<ActionResult<List<Category>>> DeleteCategory(int id) {
        return Ok();
    }
}