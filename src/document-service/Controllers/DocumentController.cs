using Microsoft.AspNetCore.Mvc;

namespace DocumentServce.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentController : ControllerBase
{

    [HttpGet(Name = "GetDocument")]
    public void Get()
    {
        throw new NotImplementedException();
    }

    [HttpPost(Name = "CreateDocument")]
    public void Create()
    {
        throw new NotImplementedException();
    }

    [HttpPut(Name = "UpdateDocument")]
    public void Update()
    {
        throw new NotImplementedException();
    }

    [HttpDelete(Name = "DeleteDocument")]
    public void Delete()
    {
        throw new NotImplementedException();
    }

    [HttpGet(Name = "ListDocuments")]
    public void List()
    {
        throw new NotImplementedException();
    }
}
