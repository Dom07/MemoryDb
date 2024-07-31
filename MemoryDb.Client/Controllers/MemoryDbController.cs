using Microsoft.AspNetCore.Mvc;

namespace MemoryDb.Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MemoryDbController : ControllerBase
    {
        [HttpGet(Name="GetValue")]
        public async Task<string> Get()
        {
            var client = new Core.Client();
            await client.Set("1", "First string going in.", "-1");
            return "OK";
        }
    }
}
