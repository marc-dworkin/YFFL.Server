using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YFFL.Server.GameDataService.Domain;
using YFFL.Server.GameDataService.Vendor.Yahoo.GameData;

namespace YFFL.Server.GameDataService.Controllers
{
    [Route("api/[controller]")]
    public class NFLPlayerGameDataController : ControllerBase
    {
        private readonly YahooGameDataService _service;
        public NFLPlayerGameDataController()
        {
            //TODO: IoC
            _service = new YahooGameDataService();
        }

        // GET api/values
        [HttpGet]
        public NFLPlayerGameDataResponse Get(int season, int week)
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


    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
