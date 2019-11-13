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
		private const int MAX_TURNS = 5000;

		//returns the name of the winning bot. Null if it's a tie.
		public string PlayGame(string jsonForts, Dictionary<string, IBot> players, StringBuilder gameState)
		{
			var turnStates = new List<BoardState>();

			Forts = JsonConvert.DeserializeObject<List<Fort>>(jsonForts);
			ValidateForts();
			ValidateStartingArmies();
			SetBotMapping(players);

			var gameTied = false;
			var winner = GetTheWinner();
			while (winner == null)
			{
				m_turnNumber++;
				if (m_turnNumber > MAX_TURNS)
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

		private void ValidateStartingArmies()
		{
			foreach (var fort in Forts)
			{
				// TODO: add cost and max number guys rules here
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

					var sourceFort = GetFortByID(move.SourceFortID);
					var destFort = GetFortByID(move.DestinationFortID);
					var toRemove = sourceFort.DefendingGuys.Where(g => g.Type == move.GuyType).Take(numGuysToMove).ToList();

					if (sourceFort == null || destFort == null || sourceFort.FortOwner != player || toRemove.Count < numGuysToMove)
					{
						continue;
					}

					// only remove the amount requested, not all
					var toMove = new List<Guy>();
					for (var i = 0; i < numGuysToMove; i++)
					{
						sourceFort.DefendingGuys.Remove(toRemove[i]);
						toMove.Add(toRemove[i]);
					}

					// TODO: divide groups by speed?
					var distanceBetweenForts = GetDistanceBetween(sourceFort.Location, destFort.Location);
					var gg = new GuyGroup{DestinationFort = destFort, GroupOwner = player, Guys = toMove, TicksTillFinished = (int)distanceBetweenForts};
					TravelingGGs.Add(gg);
				}
			}
		}

		private Fort GetFortByID(int id)
		{
			return Forts.Where(f => f.ID == id).FirstOrDefault();
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
				for (var i = 0; i < fort.BirthSpeed; i++)
				{
					fort.DefendingGuys.Add(GuyFactory.CreateGuy(fort.BirthingType));
				}
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

			TravelingGGs.RemoveAll(gg => gg.Guys.Count == 0);
		}

		private void EnterFort(GuyGroup gg)
		{
			var destFort = gg.DestinationFort;

			// if fort is empty
			if (destFort.FortOwner == null)
			{
				destFort.FortOwner = gg.GroupOwner;
				destFort.DefendingGuys = gg.Guys;
				gg.Guys = new List<Guy>();
			}
			// if player is reinforcing
			else if (gg.GroupOwner == destFort.FortOwner)
			{
				destFort.DefendingGuys.Concat(gg.Guys);
				gg.Guys = new List<Guy>();
			}
			// if player is attacking
			else
			{
				// each team stabs each other at the same time, and then deaths are calculated
				while (gg.Guys.Count > 0 && destFort.DefendingGuys.Count > 0)
				{
					var totalAttackerDmg = gg.Guys.Sum(g => g.Strength);
					var totalDefenderDmg = destFort.DefendingGuys.Sum(g => g.Strength);

					var numAttackers = gg.Guys.Count;
					var numDefenders = destFort.DefendingGuys.Count;

					// apply damage to defenders
					for (var i=0; i < totalAttackerDmg; i++)
					{
						var defender = destFort.DefendingGuys[i % numDefenders];
						if (defender.Health > 0)
						{
							defender.Health--;
						}
					}

					// apply damage to attackers
					for (var i = 0; i < totalDefenderDmg; i++)
					{
						var attacker = gg.Guys[i % numAttackers];
						if (attacker.Health > 0)
						{
							attacker.Health--;
						}
					}

					// remove dead guys from lists
					gg.Guys.RemoveAll(g => g.Health <= 0);
					destFort.DefendingGuys.RemoveAll(g => g.Health <= 0);
				}

				// defender wins
				if (gg.Guys.Count == 0)
				{
					// do nothing
				}
				// attacker wins
				if (destFort.DefendingGuys.Count == 0)
				{
					destFort.FortOwner = gg.GroupOwner;
					destFort.DefendingGuys = gg.Guys;
					gg.Guys = new List<Guy>();
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
		public List<Guy> DefendingGuys;
		public string FortOwner;
		public int DefensiveBonus;
		public GuyType BirthingType;

		public override string ToString()
		{
			return $"ID:{ID} BS:{BirthSpeed} Loc:{Location} Owner:{FortOwner} Guys:{DefendingGuys.Count}";
		}
	}

	public class GuyGroup
	{
		public string GroupOwner;
		public List<Guy> Guys;
		public int TicksTillFinished;
		public Fort DestinationFort;

		public override string ToString()
		{
			return $"Owner:{GroupOwner} Guys:{Guys.Count} Dest:{DestinationFort.ID} TicksLeft:{TicksTillFinished}";
		}
	}

	public class Move
	{
		public int SourceFortID;
		public int DestinationFortID;
		public int NumGuys;
		public GuyType GuyType;
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
