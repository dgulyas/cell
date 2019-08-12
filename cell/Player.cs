namespace Cell
{
	public class Player
	{
		public string Name; //This is the unique id.

		public Player Clone()
		{
			return new Player{Name = Name};
		}

		public override string ToString()
		{
			return $"Name: {Name}";
		}

	}
}
