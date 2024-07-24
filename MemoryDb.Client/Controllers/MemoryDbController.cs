using Microsoft.AspNetCore.Mvc;

namespace MemoryDb.Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MemoryDbController : ControllerBase
    {
        [HttpGet(Name = "GetClient")]
        public string Get()
        {
            var client = new Core.Client();
            return client.Set("Hello World");
        }
    }
}
