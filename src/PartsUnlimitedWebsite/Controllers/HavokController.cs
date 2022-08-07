using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PartsUnlimited.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PartsUnlimited.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HavokController : ControllerBase
    {

        private readonly IHostingEnvironment _env;



        public HavokController(IHostingEnvironment env)
        {
            _env = env;
        }
        [HttpGet]
        public ActionResult<Havok> GetHavok()
        {
            FileProcessor fp = new FileProcessor(_env);
            Havok item = JsonConvert.DeserializeObject<Havok> (fp.LoadJsonFromAppFolder("\\", "havok.json"));
            return item;
        }


        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHavokItem(long id, Havok item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            FileProcessor fp = new FileProcessor(_env);
            string json = JsonConvert.SerializeObject(item);
           await  fp.SaveAwaitableJsonToAppFolder("\\", "havok.json", json);

            return NoContent();
        } 
     
   
        
    }
}
