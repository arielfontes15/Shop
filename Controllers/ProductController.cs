using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("products")]
    public class ProductController : ControllerBase 
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Product>>> GetProduct([FromServices] DataContext context) 
        {
            var products = await context.Products.Include(p => p.Category).AsNoTracking().ToListAsync();
            return products;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Product>>> GetByIdProduct(int id, [FromServices] DataContext context) 
        {
            try
            {
                var product = await context.Products.Include(p => p.Category).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                return Ok(product);
            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Não foi possivel encontrar esta produto."});
            }

        }

        [HttpGet]
        [Route("categories/{id:int}")]//products/categories/1
        public async Task<ActionResult<List<Product>>> GetProductByCategory([FromServices] DataContext context, int id) 
        {
            try
            {            
                var product = await context.Products.Include(p => p.Category).AsNoTracking().Where(p => p.CategoryId == id).ToListAsync();
                return Ok(product);
            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Não foi possivel encontrar esta produto."});
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<List<Product>>> PostProduct(
            [FromBody]Product model,
            [FromServices] DataContext context
        ) {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Products.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch
            {
                return BadRequest(new { message = "Não foi possivel criar o produto."});
            }

        }
    }
}