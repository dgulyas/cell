using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Cell
{
	public class Cell : IGame
	{
		private readonly Dictionary<string, IBot> m_playerNameToBot = new Dictionary<string, IBot>();

		private List<Fort> Forts;
		private List<GuyGroup> TravelingGGs = new List<GuyGroup>();

		private int m_turnNumber = 0;

		//returns the name of the winning bot. Null if it's a tie.
		public string PlayGame(string jsonForts, Dictionary<string, IBot> players, StringBuilder gameState)
		{
			var turnStates = new List<BoardState>();

			Forts = JsonConvert.DeserializeObject<List<Fort>>(jsonForts);
			ValidateForts();
			SetBotMapping(players);

			var gameTied = false;
			var winner = GetTheWinner();
			while (winner == null)
			{
				m_turnNumber++;

				if (m_turnNumber > 5000)
				{
					gameTied = true;
					break;
				}

				var jsonBoardState = MakeJsonBoardState();

				//deserializing it means we get a copy that won't change
				turnStates.Add(JsonConvert.DeserializeObject<BoardState>(jsonBoardState));

				var moves = GetMovesFromBots(jsonBoardState);
				ExecuteMoves(moves);
				Tick();
				winner = GetTheWinner();
			}

			gameState.Append(JsonConvert.SerializeObject(turnStates));

			return gameTied ? null : winner;
		}

		private void ValidateForts()
		{
			//The IDs of all the forts must be unique.
			//We let the map creator set the IDs because
			//it's nicer when forts have IDs that make sense.
			var fortIDs = new HashSet<int>();
			foreach (var fort in Forts)
			{
				if (fortIDs.Contains(fort.ID))
				{
					throw new Exception("2 forts have the same ID");
				}

				if (fort.BirthSpeed < 0)
				{
					throw new Exception($"Fort '{fort.ID}' birthspeed can't be negative.");
				}

				fortIDs.Add(fort.ID);
			}
		}

		//This creates a 2 way mapping between player names and the bots.
		private void SetBotMapping(Dictionary<string, IBot> players)
		{
			foreach (var playerKey in players.Keys)
			{
				m_playerNameToBot.Add(playerKey, players[playerKey]);
			}
		}

		private string GetTheWinner()
		{
			var remainingPlayers = GetPlayerList();
			return remainingPlayers.Count == 1 ? remainingPlayers.First() : null;
		}

		private List<string> GetPlayerList()
		{
			//A player is still alive as long as it controls a fort or has a GuyGroup that
			//hasn't arrived yet.
			var ggControllers = TravelingGGs.Select(gg => gg.GroupOwner);
			var fortControllers = Forts.Select(f => f.FortOwner).Where(fo => fo != null);
			return ggControllers.Union(fortControllers).Distinct().ToList();
		}

		private string MakeJsonBoardState()
		{
			var bs = new BoardState { TurnNumber = m_turnNumber, Forts = Forts, GuyGroups = TravelingGGs };
			return JsonConvert.SerializeObject(bs);
		}

		private Dictionary<string, List<Move>> GetMovesFromBots(string jsonBoardState)
		{
			var botMoves = new Dictionary<string, List<Move>>();

			//For each remaining player we ask that player's bot what moves it wants to make.
			var remainingPlayers = GetPlayerList();
			foreach (var player in remainingPlayers)
			{
				var moves = m_playerNameToBot[player].Do(jsonBoardState);
				botMoves.Add(player, moves);
			}

			return botMoves;
		}

		private void ExecuteMoves(Dictionary<string, List<Move>> playerMoves)
		{
			foreach (var player in playerMoves.Keys)
			{
				foreach (var move in playerMoves[player])
				{
					var numGuysToMove = move.NumGuys;
					if (numGuysToMove < 1)
					{
						continue;
					}

					var sourceFort = move.Source;
					var destFort = move.Destination;
					if (sourceFort == null || destFort == null || sourceFort.FortOwner != player || sourceFort.NumDefendingGuys < numGuysToMove)
					{
						continue;
					}

					sourceFort.NumDefendingGuys -= numGuysToMove;
					var distanceBetweenForts = GetDistanceBetween(sourceFort.Location, destFort.Location);
					var gg = new GuyGroup{DestinationFortID = destFort.ID, GroupOwner = player, NumGuys = numGuysToMove, TicksTillFinished = (int)distanceBetweenForts};
					TravelingGGs.Add(gg);
				}
			}
		}

		private Fort GetFortByID(int id)
		{
			var forts = Forts.Where(f => f.ID == id).ToList();
			if (forts.Count == 1)
			{
				return forts[0];
			}

			return null;
		}

		private double GetDistanceBetween(Point source, Point dest)
		{
			double dX = source.X - dest.X;
			double dY = source.Y - dest.Y;
			return Math.Sqrt(dX * dX + dY * dY);
		}

		private void Tick()
		{
			foreach (var fort in Forts)
			{
				fort.NumDefendingGuys += fort.BirthSpeed;
			}

			foreach (var gg in TravelingGGs)
			{
				gg.TicksTillFinished--;
			}

			foreach (var gg in TravelingGGs)
			{
				if (gg.TicksTillFinished < 1)
				{
					EnterFort(gg);
				}
			}

			TravelingGGs.RemoveAll(gg => gg.NumGuys == 0);
		}

		private void EnterFort(GuyGroup gg)
		{
			var destFort = GetFortByID(gg.DestinationFortID);
			if (gg.GroupOwner == destFort.FortOwner)
			{
				destFort.NumDefendingGuys += gg.NumGuys;
				gg.NumGuys = 0;
			}
			else
			{
				destFort.NumDefendingGuys -= gg.NumGuys;
				if (destFort.NumDefendingGuys < 0)
				{
					destFort.FortOwner = gg.GroupOwner;
					destFort.NumDefendingGuys *= -1;
					gg.NumGuys = 0;
				}
			}
		}

	}

	public class BoardState
	{
		public int TurnNumber;
		public List<Fort> Forts;
		public List<GuyGroup> GuyGroups;

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.AppendLine($"Turn:{TurnNumber}");

			sb.AppendLine("Forts:");
			foreach (var fort in Forts)
			{
				sb.AppendLine(fort.ToString());
			}

			sb.AppendLine("GuyGroups:");
			foreach (var gg in GuyGroups)
			{
				sb.AppendLine(gg.ToString());
			}

			return sb.ToString();
		}
	}

	public class Fort
	{
		public int ID;
		public Point Location;
		public int BirthSpeed;
		public int NumDefendingGuys;
		public string FortOwner;
		public int DefensiveBonus;
		public GuyType BirthingType;

		public override string ToString()
		{
			return $"ID:{ID} BS:{BirthSpeed} Loc:{Location} Owner:{FortOwner} Guys:{NumDefendingGuys}";
		}
	}

	public class GuyGroup
	{
		public string GroupOwner;
		public int NumGuys;
		public int TicksTillFinished;
		public int DestinationFortID;

		public override string ToString()
		{
			return $"Owner:{GroupOwner} Guys:{NumGuys} Dest:{DestinationFortID} TicksLeft{TicksTillFinished}";
		}
	}

	public class Move
	{
		public Fort Source;
		public Fort Destination;
		public int NumGuys;
	}

	public class Point
	{
		public int X;
		public int Y;

		public override string ToString()
		{
			return $"{X},{Y}";
		}
	}
}
