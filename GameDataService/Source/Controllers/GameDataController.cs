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
    }
}
