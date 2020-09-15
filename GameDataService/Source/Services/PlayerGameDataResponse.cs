using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using YFFL.Server.GameDataService.Domain;

namespace YFFL.Server.GameDataService.Services
{
    public class PlayerGameDataResponse
    {
        [JsonProperty("results")]
        public ICollection<PlayerGameData> Results { get; set; }

        [JsonProperty("errors")]
        public ICollection<Exception> Errors { get; set; }
    }
}
