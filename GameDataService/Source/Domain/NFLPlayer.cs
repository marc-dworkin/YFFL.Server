using Newtonsoft.Json;
using System.Collections;
using YFFL.Server.GameDataService.Vendor.Yahoo.GameData;

namespace YFFL.Server.GameDataService.Domain
{
    public class NFLPlayer
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// TODO: make enum?
        /// </summary>
        [JsonProperty("nflTeamCode")]
        public string NFLTeamCode{ get; set; }

        [JsonProperty("yahooId")]
        public string YahooId { get; set; }


        [JsonProperty("nflPosition")] 
        public NFLPosition NFLPosition  { get; set; }
    }
}
