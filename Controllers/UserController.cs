using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Services;

namespace Shop.Controllers {
    [Route("v1/users")]
    public class UserController : Controller {

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<User>>> GetAllUsers([FromServices] DataContext context){
            return await context.Users.AsNoTracking().ToListAsync();
        }

        [HttpPost]
        [Route("")]
        // [AllowAnonymous]
        [Authorize(Roles = "manager")] // Caso não haja nennhum usuario no banco nao vai dar pra fazer o post
        public async Task<ActionResult<List<User>>> PostUser(
            [FromBody]User model,
            [FromServices] DataContext context
        ) {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Users.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch
            {
                return BadRequest(new { message = "Não foi possivel criar um usuario."});
            }

        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Autenticate(
            [FromBody]User model,
            [FromServices] DataContext context
        ) {
            var user = await context.Users.AsNoTracking().Where(u => u.Username == model.Username && u.Password == model.Password).FirstOrDefaultAsync();
            if(user == null)
                return NotFound(new {message =  "Usuário ou senha invalidos"});
            var token = TokenService.GenerateToken(user);
            return new {
                user = user,
                token = token
            };

        }

        [HttpPut]
        [Route("{id:int}")]
        // [AllowAnonymous]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> PutUser([FromServices] DataContext context, int id , [FromBody] User model){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(id != model.Id)
                return BadRequest(new { message = "Usuário não encontrado"});
            try
            {
                context.Users.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Não foi possivel atualizar o usuario."});
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> DeleteUser(int id, [FromServices] DataContext context) {
            var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

            if(user == null)
                return BadRequest(new { message = "Não foi possivel remover o usuário"});

            try
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return user;
            }
            catch (System.Exception)
            {                
                return BadRequest(new { message = "Não foi possivel remover o usuario."});
            }
        }
    }
}