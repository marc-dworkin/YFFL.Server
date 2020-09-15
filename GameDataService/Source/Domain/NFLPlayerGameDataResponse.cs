using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace YFFL.Server.GameDataService.Domain
{
    public class NFLPlayerGameDataResponse
    {
        [JsonProperty("results")]
        public ICollection<NFLPlayerGameData> Results { get; set; }

        [JsonProperty("errors")]
        public ICollection<Exception> Errors { get; set; }
    }
}
