using Cell;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Display
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		void App_Startup(object sender, StartupEventArgs e)
		{
			MainWindow wnd = new MainWindow();
			wnd.Show();
			Task.Run(() =>
			{
				foreach (BoardState b in wnd.boards)
				{
					this.Dispatcher.Invoke((Action)(async () =>
					{
						wnd.currentBoard = b;
						wnd.DrawBoardState();
						await Task.Delay(1000);
					}));
				}
			});
		}
	}
}
