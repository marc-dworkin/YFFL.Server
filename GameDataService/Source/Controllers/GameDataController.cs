using Microsoft.AspNetCore.Mvc;
using YFFL.Server.GameDataService.Domain;
using YFFL.Server.GameDataService.Services;
using YFFL.Server.GameDataService.Vendor.Yahoo.GameData;

namespace YFFL.Server.GameDataService.Controllers
{
    [Route("api/[controller]")]
    public class GameDataController : ControllerBase
    {
        private readonly IPlayerGameDataService _service;

        public GameDataController(IPlayerGameDataService service)
        {
            _service = service;
        }

        // GET api/values
        [HttpGet]
        public PlayerGameDataResponse Get(int season, int week)
        {
            return _service.GetPlayerGameData(season, week);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
