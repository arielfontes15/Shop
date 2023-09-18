using Microsoft.AspNetCore.Mvc;

[Route("categories")]
public class CategoryController :  ControllerBase {

    [HttpGet]
    [Route("")]
    public string GetCategory() {
        return "GET";
    }
    [HttpGet]
    [Route("{id:int}")]
    public string GetByIdCategory(int id) {
        return "GET BY ID";
    }

    [HttpPost]
    [Route("")]
    public string PostCategory() {
        return "POST";
    }

    [HttpPut]
    [Route("")]
    public string PutCategory() {
        return "PUT";
    }

    [HttpDelete]
    [Route("")]
    public string DeleteCategory() {
        return "DELETE";
    }
}