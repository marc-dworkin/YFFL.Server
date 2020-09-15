using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YFFL.Server.GameDataService.Vendor.Yahoo.GameData
{
    public class YahooScoreboard
    {
        [JsonProperty("service")]
        public Service Service { get; set; }
    }


    public class Service
    {
        [JsonProperty("scoreboard")]
        public Scoreboard Scoreboard { get; set; }

    }

    public class Scoreboard
    {
        [JsonProperty("games")]
        public Dictionary<string, Game> Games { get; set; }
    }

    public class Game
    {

        [JsonProperty("gameid")]
        public string GameId { get; set; }
        [JsonProperty("global_gameid")]
        public string GlobalGameId { get; set; }
        [JsonProperty("navigation_links")]
        public NavigationLinks NavigationLinks { get; set; }
    }

    public class NavigationLinks
    {
        [JsonProperty("boxscore")]
        public Link BoxScore { get; set; }

    }

    public class Link
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    /*
                    "gameid": "nfl.g.20190905003",
                    "global_gameid": "nfl.g.2142040",
                    "start_time": "Fri, 06 Sep 2019 00:20:00 +0000",
                    "is_time_tba": false,
                    "season_phase_id": "season.phase.season",
                    "game_type": "Regular Season",
                    "winning_team_id": "nfl.t.9",
                    "is_rank_upset": null,
                    "is_spread_upset": true,
                    "outcome_type": "outcome.type.won",
                    "home_team_id": "nfl.t.3",
                    "away_team_id": "nfl.t.9",
                    "week_number": "1",
                    "navigation_links": {
                        "tickets": {
                            "url": "https:\/\/seatgeek.com\/chicago-bears-tickets\/?aid=14&date=2019-09-05"
                        },
                        "boxscore": {
                            "url": "\/nfl\/green-bay-packers-chicago-bears-20190905003\/"
                        },
                        "match_page": {
                            "url": "\/nfl\/green-bay-packers-chicago-bears-20190905003\/"
                        },
                        "recap": {
                            "url": "\/packers-d-aaron-rodgers-beat-bears-10-3-033539387--nfl.html"
                        },
                        "league_home": {
                            "url": "\/nfl\/"
                        },
                        "league_scores": {
                            "url": "\/nfl\/scoreboard\/"
                        }
                    },
                    "sportacular_url": "ysportacular:\/\/v2\/scores\/details?gameId=nfl.g.20190905003&sport=nfl",
                    "status_display_name": "Final",
                    "status_description": "Final",
                    "status_type": "status.type.final",
                    "total_away_points": "10",
                    "current_period_id": "4",
                    "total_home_points": "3",
    */
}

