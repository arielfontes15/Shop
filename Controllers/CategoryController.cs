using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("categories")]
    public class CategoryController :  ControllerBase {

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Category>>> GetCategory(
            [FromServices] DataContext context
        ) {
            var categories = await context.Categories.AsNoTracking().ToListAsync();
            return Ok(categories);
        }
        
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> GetByIdCategory(
            int id,
            [FromServices] DataContext context
        ) {
            try
            {
                var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
                return Ok(category);
            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Não foi possivel encontrar esta categoria."});
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<List<Category>>> PostCategory(
            [FromBody]Category model,
            [FromServices] DataContext context
        ) {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Categories.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch
            {
                return BadRequest(new { message = "Não foi possivel criar a categoria."});
            }

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Category>>> PutCategory(
            int id,
            [FromBody]Category model,
            [FromServices] DataContext context
        ) {
            if(model.Id != id)
                return NotFound(new { message = "Categoria não encontrada." });
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este registro ja foi atualizado."});
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel atualizar a categoria."});
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public  async Task<ActionResult<List<Category>>> DeleteCategory(
            int id,
            [FromServices] DataContext context
        ) {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if(category == null)
                return BadRequest(new { message = "Não foi possivel remover a categoria."});
            try
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok(new { message = "Categoria removida com sucesso." });
            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Não foi possivel remover a categoria."});
            }
        }
    }
}