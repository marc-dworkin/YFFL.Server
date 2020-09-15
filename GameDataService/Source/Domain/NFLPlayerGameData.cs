using Newtonsoft.Json;
using System.Collections.Generic;

namespace YFFL.Server.GameDataService.Domain
{
    //TODO: Fumbles
    public class NFLPlayerGameData
    {
        [JsonProperty("player")]
        public NFLPlayer Player { get; set; }

        [JsonProperty("passing")]
        public Dictionary<PassingStat, int> Passing { get; set; }
        [JsonProperty("rushing")]
        public Dictionary<RushingStat, int> Rushing { get; set; }

        [JsonProperty("receiving")]
        public Dictionary<ReceivingStat, int> Receiving { get; set; }
        [JsonProperty("kicking")]
        public Dictionary<KickingStat, int> Kicking { get; set; }

        [JsonProperty("kickReturns")]
        public Dictionary<KickReturnsStat, int> KickReturns { get; set; }
    }
}
