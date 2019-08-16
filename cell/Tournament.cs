using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Cell.Bots;

namespace Cell
{
	class Tournament
	{
		public List<Map> maps = new List<Map>();
		public List<Type> bots = new List<Type>(); //the names of the bot classes that should be in the tournament
		private Dictionary<string, ConstructorInfo> m_botConstructors = new Dictionary<string, ConstructorInfo>();

		public void Test()
		{
			PopulateBotConstructors();

			foreach (var map in maps)
			{

			}


		}

		public void PopulateBotConstructors()
		{
			var ibotType = typeof(IBot);
			var botTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
				.Where(t => ibotType.IsAssignableFrom(t) && !t.IsInterface);

			foreach (var type in botTypes)
			{
				m_botConstructors.Add(type.Name, type.GetConstructor(new Type[] { }));
			}
		}
	}
}
