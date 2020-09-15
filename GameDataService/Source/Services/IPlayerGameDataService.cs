using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YFFL.Server.GameDataService.Services
{
    public interface IPlayerGameDataService
    {
        PlayerGameDataResponse GetPlayerGameData(int season, int week);
    }
}
