using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cell
{
	public interface IBot
	{
		List<Move> Do(string jsonBoard);
		void SetPlayer(string player);
	}
}
