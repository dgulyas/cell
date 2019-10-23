using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cell
{
	public interface IBot
	{
		List<Tuple<int, int, int>> Do(string jsonBoard);
	}
}
