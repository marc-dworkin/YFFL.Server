using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YFFL.Server.GameDataService.Domain;
using YFFL.Server.GameDataService.Services;
using YFFL.Server.GameDataService.Vendor.Yahoo.GameData;

namespace YFFL.Server.GameDataService.Vendor.Yahoo.GameData
{
	public class YahooPlayerGameDataService : IPlayerGameDataService
	{
		public PlayerGameDataResponse GetPlayerGameData(int season, int week)
		{
			var scoreboard = GetScoreboard(season, week);
			var errors = new List<Exception>();

            var res = scoreboard.Service.Scoreboard.Games.AsParallel()
                .SelectMany(c =>
                {
                    try
                    {
                        return GetPlayerGameData(c.Value.NavigationLinks.BoxScore.Url);
                    }
                    catch (Exception x)
                    {
                        lock (errors)
                        {
                            errors.Add(x);
                        }
                        return Array.Empty<PlayerGameData>();
                    }
                })
                .ToArray();

			return new PlayerGameDataResponse
			{
				Results = res,
				Errors = errors
			};
		}

		private YahooScoreboard GetScoreboard(int season, int week)
		{
			var c = new RestClient("https://api-secure.sports.yahoo.com/v1/editorial/s/");
			var req = new RestRequest($"scoreboard?lang=en-US&region=US&tz=America%2FNew_York&ysp_redesign=1&ysp_platform=desktop&leagues=nfl&week={week}&season={season}&sched_states=2&v=2&ysp_enable_last_update=1&include_last_play=1");
			var res = c.Get(req);
			return JsonConvert.DeserializeObject<YahooScoreboard>(res.Content);
		}


		private ICollection<PlayerGameData> GetPlayerGameData(string boxScoreLink)
        {
			try
            {
				var data = GetBoxScore(boxScoreLink);
				var playerMap = data.Context.Dispatcher.Stores.PlayersStore.Players;
				//				var teamMap = data.Context.Dispatcher.Stores.ScoresStore.State.ScoreStrip.Teams;
				var teamMap = data.Context.Dispatcher.Stores.TeamsStore.Teams;
				return data.Context.Dispatcher.Stores.StatsStore.PlayerStats
					.Select(c => Map(c.Key, c.Value, playerMap, teamMap))
					.Where(c => c.Player.NFLPosition != NFLPosition.NA)
					.ToArray();
			}
			catch (Exception x)
            {
				throw new Exception($"Error retrieving Player Game Data for Game: {boxScoreLink}", x);
            }


		}

		public class StatTypeMap
        {
			public string YahooStatTypeId { get; set; }
			public PassingStat? Passing { get; set; }
			public RushingStat? Rushing { get; set; }
			public ReceivingStat? Receiving { get; set; }
			public KickingStat? Kicking { get; set; }

			public StatTypeMap(string yahooStatTypeId)
            {
				YahooStatTypeId = yahooStatTypeId;
            }
		}

		private PlayerGameData Map(string playerId, PlayerStats stats, Dictionary<string, Player> playerMap, Dictionary<string, Team> teamMap)
        {
			var player = playerMap[playerId];
			try
			{
				var position = GetNFLPosition(player.PrimaryPositionId);
				var team = teamMap[player.TeamId];

				var res = new PlayerGameData
				{
					Player = new NFLPlayer
					{
						DisplayName = player.DisplayName,
						FirstName = player.FirstName,
						LastName = player.LastName,
						NFLPosition = position,
						NFLTeamCode = team.Abbreviation,
						YahooId = player.PlayerId
					},
					Passing = new Dictionary<PassingStat, int>(),
					Receiving = new Dictionary<ReceivingStat, int>(),
					Rushing = new Dictionary<RushingStat, int>(),
					Kicking = new Dictionary<KickingStat, int>(),
					KickReturns = new Dictionary<KickReturnsStat, int>()
				};

				var returnTDStats = new[] { "nfl.stat_type.507", "nfl.stat_type.513" };

				var variationId = "nfl.stat_variation.2";
				foreach (var stat in stats[variationId])
				{
					decimal value;
					if (decimal.TryParse(stat.Value, out value))
					{
						var intValue = Convert.ToInt32(value);

						if (returnTDStats.Contains(stat.Key))
						{
							if (res.KickReturns.ContainsKey(KickReturnsStat.TD))
							{
								res.KickReturns[KickReturnsStat.TD] += intValue;
							}
							else
							{
								res.KickReturns[KickReturnsStat.TD] = intValue;

							}
						}
						else
						{
							var map = GetStatMap(stat.Key);
							if (map.Passing.HasValue)
							{
								res.Passing[map.Passing.Value] = intValue;
							}
							if (map.Rushing.HasValue)
							{
								res.Rushing[map.Rushing.Value] = intValue;
							}
							if (map.Receiving.HasValue)
							{
								res.Receiving[map.Receiving.Value] = intValue;
							}
							if (map.Kicking.HasValue)
							{
								res.Kicking[map.Kicking.Value] = intValue;
							}
							if (map.Kicking.HasValue)
							{
								res.Kicking[map.Kicking.Value] = intValue;
							}
						}
					}
				}

				return res;
			} catch (Exception x)
            {
				throw new Exception($"Error processing player: {player.DisplayName}", x);
            }
		}

		private YahooBoxScoreData GetBoxScore(string boxScoreLink)
        {
            var c = new RestClient("https://sports.yahoo.com");
            var req = new RestRequest(boxScoreLink);
            var res = c.Get(req);
            var regex = new Regex(@"^.+root.App.main = (\{.+?)\;\W+\(this\).+$", RegexOptions.Compiled | RegexOptions.Singleline);
            var m = regex.Match(res.Content);
            var json = m.Groups[1].Value;
            return JsonConvert.DeserializeObject<YahooBoxScoreData>(json);

        }

		/*
		 * 	var statMap = data.Context.Dispatcher.Stores.StatsStore.StatTypes.ToDictionary(st => st.Key, st => st.Value.ShortName);
		 * 	var statCatMap = data.Context.Dispatcher.Stores.StatsStore.StatCategories.SelectMany(sc => sc.Value.Stats.Select(c => new { Cat = sc.Value.Name, Id = c, Name = statMap[c] }));
		 * 	statCatMap.Dump();
		 * 
         * 	Passing	nfl.stat_type.3	FL
	Passing	nfl.stat_type.102	Comp
	Passing	nfl.stat_type.103	Att
	Passing	nfl.stat_type.104	Pct
	Passing	nfl.stat_type.105	Yds
	Passing	nfl.stat_type.106	Y/A
	Passing	nfl.stat_type.108	TD
	Passing	nfl.stat_type.109	Int
	Passing	nfl.stat_type.111	Sack
	Passing	nfl.stat_type.112	YdsL
	Passing	nfl.stat_type.113	QBRat
		*/
		private string GetYahooStatId(PassingStat stat)
        {
			switch (stat)
            {
				case PassingStat.COMP:
					return "nfl.stat_type.102";
				case PassingStat.ATT:
					return "nfl.stat_type.103";
				case PassingStat.YDS:
					return "nfl.stat_type.105";
				case PassingStat.TD:
					return "nfl.stat_type.108";
			}
			throw new NotSupportedException($"PassingStat.{stat}");
		}

		/*
	Rushing	nfl.stat_type.3	FL
	Rushing	nfl.stat_type.202	Rush
	Rushing	nfl.stat_type.203	Yds
	Rushing	nfl.stat_type.205	Avg
	Rushing	nfl.stat_type.206	Long
	Rushing	nfl.stat_type.207	TD
		*/
		private string GetYahooStatId(RushingStat stat)
		{
			switch (stat)
			{
				case RushingStat.CAR:
					return "nfl.stat_type.202";
				case RushingStat.YDS:
					return "nfl.stat_type.203";
				case RushingStat.TD:
					return "nfl.stat_type.207";
			}
			throw new NotSupportedException($"RushingStat.{stat}");
		}

		/*
	Receiving	nfl.stat_type.3	FL
	Receiving	nfl.stat_type.302	Rec
	Receiving	nfl.stat_type.303	Yds
	Receiving	nfl.stat_type.305	Avg
	Receiving	nfl.stat_type.306	Long
	Receiving	nfl.stat_type.309	TD
	Receiving	nfl.stat_type.310	Tgt
		*/
		private string GetYahooStatId(ReceivingStat stat)
		{
			switch (stat)
			{
				case ReceivingStat.TGT:
					return "nfl.stat_type.310";
				case ReceivingStat.REC:
					return "nfl.stat_type.302";
				case ReceivingStat.YDS:
					return "nfl.stat_type.303";
				case ReceivingStat.TD:
					return "nfl.stat_type.309";
			}
			throw new NotSupportedException($"ReceivingStat.{stat}");
		}

		private StatTypeMap GetStatMap(string yahooStatId)
		{
			switch (yahooStatId)
			{
				case "nfl.stat_type.102":
					return new StatTypeMap(yahooStatId) { Passing = PassingStat.COMP };
				case "nfl.stat_type.103":
					return new StatTypeMap(yahooStatId) { Passing = PassingStat.ATT };
				case "nfl.stat_type.105":
					return new StatTypeMap(yahooStatId) { Passing = PassingStat.YDS };
				case "nfl.stat_type.108":
					return new StatTypeMap(yahooStatId) { Passing = PassingStat.TD };

				case "nfl.stat_type.202":
					return new StatTypeMap(yahooStatId) { Rushing = RushingStat.CAR };
				case "nfl.stat_type.203":
					return new StatTypeMap(yahooStatId) { Rushing = RushingStat.YDS };
				case "nfl.stat_type.207":
					return new StatTypeMap(yahooStatId) { Rushing = RushingStat.TD };

				case "nfl.stat_type.310":
					return new StatTypeMap(yahooStatId) { Receiving = ReceivingStat.TGT };
				case "nfl.stat_type.302":
					return new StatTypeMap(yahooStatId) { Receiving = ReceivingStat.REC };
				case "nfl.stat_type.303":
					return new StatTypeMap(yahooStatId) { Receiving = ReceivingStat.YDS };
				case "nfl.stat_type.309":
					return new StatTypeMap(yahooStatId) { Receiving = ReceivingStat.TD };

				case "nfl.stat_type.407":
					return new StatTypeMap(yahooStatId) { Kicking = KickingStat.FGM };
				case "nfl.stat_type.408":
					return new StatTypeMap(yahooStatId) { Kicking = KickingStat.FGA };
				case "nfl.stat_type.412":
					return new StatTypeMap(yahooStatId) { Kicking = KickingStat.XPA };
				case "nfl.stat_type.411":
					return new StatTypeMap(yahooStatId) { Kicking = KickingStat.XPM };

			}
			return new StatTypeMap(yahooStatId) ;

			//			throw new NotSupportedException(yahooStatId);
			//TODO: log?
		}

		/*
	Kicking	nfl.stat_type.407	FGM
	Kicking	nfl.stat_type.408	FGA
	Kicking	nfl.stat_type.409	Pct
	Kicking	nfl.stat_type.410	Long
	Kicking	nfl.stat_type.411	XPM
	Kicking	nfl.stat_type.412	XPA
	Kicking	nfl.stat_type.413	Pts
		*/
		private string GetYahooStatId(KickingStat stat)
		{
			switch (stat)
			{
				case KickingStat.FGM:
					return "nfl.stat_type.407";
				case KickingStat.FGA:
					return "nfl.stat_type.408";
				case KickingStat.XPA:
					return "nfl.stat_type.412";
				case KickingStat.XPM:
					return "nfl.stat_type.411";
			}
			throw new NotSupportedException($"ReceivingStat.{stat}");
		}


		/*
	Returns	nfl.stat_type.502	KR
	Returns	nfl.stat_type.503	Yds
	Returns	nfl.stat_type.505	Avg
	Returns	nfl.stat_type.506	Long
	Returns	nfl.stat_type.507	TD
	Returns	nfl.stat_type.508	PR
	Returns	nfl.stat_type.509	Yds
	Returns	nfl.stat_type.511	Avg
	Returns	nfl.stat_type.512	Long
	Returns	nfl.stat_type.513	TD
		*/
		private string GetYahooStatId(KickReturnsStat stat)
		{
			throw new NotImplementedException();
		}


		/*
			
		data.Context.Dispatcher.Stores.PlayersStore.Positions.ToDictionary(c => c.Value.Abbreviation, c => c.Key).Dump();

		 	TE	nfl.pos.7
	QB	nfl.pos.8
	RB	nfl.pos.9
	KR	nfl.pos.92
	PR	nfl.pos.94
	K	nfl.pos.22
	P	nfl.pos.23
	WR	nfl.pos.1
		 */

		private NFLPosition GetNFLPosition(string yahooPositionId)
        {
			switch (yahooPositionId)
            {
				case "nfl.pos.7":
					return NFLPosition.TE;
				case "nfl.pos.8":
					return NFLPosition.QB;
				case "nfl.pos.9":
					return NFLPosition.RB;
				case "nfl.pos.22":
					return NFLPosition.PK;
				case "nfl.pos.1":
					return NFLPosition.WR;
				default:
					return NFLPosition.NA;
			}
		}
	}
}
