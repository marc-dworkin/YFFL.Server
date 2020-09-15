using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YFFL.Server.GameDataService.Vendor.Yahoo.GameData
{
    public class YahooBoxScoreData
    {
        [JsonProperty("context")]
        public Context Context { get; set; }
    }

    public class Context
    {
        [JsonProperty("dispatcher")]
        public Dispatcher Dispatcher { get; set; }
    }

    public class Dispatcher
    {
        [JsonProperty("stores")]
        public Stores Stores { get; set; }
    }

    public class Stores
    {
        [JsonProperty("statsStore")]
        public StatsStore StatsStore { get; set; }

        [JsonProperty("playersStore")]
        public PlayersStore PlayersStore { get; set; }

        [JsonProperty("scoresStore")]
        public ScoresStore ScoresStore { get; set; }

        [JsonProperty("teamsStore")]
        public TeamsStore TeamsStore { get; set; }
        
    }

    public class StatsStore
    {
        [JsonProperty("statTypes")]
        public Dictionary<string, StatType> StatTypes { get; set; }

        [JsonProperty("statCategories")]
        public Dictionary<string, StatCategory> StatCategories { get; set; }

        [JsonProperty("statVariations")]
        public Dictionary<string, StatVariation> StatVariations { get; set; }

        [JsonProperty("playerStats")]
        public Dictionary<string, PlayerStats> PlayerStats { get; set; }
    }
    public class StatType
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("short_name")]
        public string ShortName { get; set; }
    }


    public class StatCategory
    {
        [JsonProperty("name")]

        public string Name { get; set; }
        [JsonProperty("sort")]
        public string Sort { get; set; }
        [JsonProperty("stats")]
        public string[] Stats { get; set; }
    }

    public class StatVariation
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class PlayerStats : Dictionary<string, Dictionary<string, string>>
    {
        public string this[string variationName, string statName]
        {
            get
            {
                return this[variationName][statName];
            }
        }
    }

    public class PlayersStore
    {
        [JsonProperty("players")]
        public Dictionary<string, Player> Players { get; set; }

        [JsonProperty("positions")]
        public Dictionary<string, Position> Positions { get; set; }
    }

    public class Player
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("primary_position_id")]
        public string PrimaryPositionId { get; set; }

        [JsonProperty("team_id")]
        public string TeamId { get; set; }

        [JsonProperty("player_id")]
        public string PlayerId { get; set; }

    }
    public class Position
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("abbr")]
        public string Abbreviation { get; set; }
        [JsonProperty("collective_name")]
        public string CollectivrName { get; set; }
    }

    public class ScoresStore
    {
        [JsonProperty("state")]
        public ScoresStoreState State { get; set; }
    }

    public class ScoresStoreState
    {
        [JsonProperty("scoreStrip")]
        public ScoreStrip ScoreStrip { get; set; }
    }

    public class ScoreStrip
    {
        [JsonProperty("teams")]
        public Dictionary<string, Team> Teams { get; set; }
    }

    /*
                       "team_id": "nfl.t.33",
                  "display_name": "Baltimore",
                  "first_name": "Baltimore",
                  "last_name": "Ravens",
                  "full_name": "Baltimore Ravens",
                  "abbr": "BAL",
                  "division_id": "4",
                  "division": "North",
                  "division_abbr": "",
                  "subdivision_id": null,
                  "subdivision": null,
                  "conference_id": "1",
                  "conference_abbr": "AFC",
                  "conference_seed": "2",
                  "conference": "American",
                  "seatgeek_id": "2061",
                  "teamHomeLink": "\u002Fnfl\u002Fteams\u002Fbaltimore\u002F",
                  "teamScheduleLink": "\u002Fnfl\u002Fteams\u002Fbaltimore\u002Fschedule",
                  "conference_position": null,
                  "group_position": "1",
                  "playoff_seed": null

     */
    public class Team
    {
        [JsonProperty("team_id")]
        public string TeamId { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("abbr")]
        public string Abbreviation { get; set; }

    }

    public class GamesStore
    {
        //records
    }

    public class TeamsStore 
    {
        [JsonProperty("teams")]
         public Dictionary<string, Team> Teams { get; set; }
    }
}
