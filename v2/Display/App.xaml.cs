using Cell;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

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
			foreach (BoardState b in wnd.boards)
			{
				this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
				{
					wnd.currentBoard = b;
					wnd.DrawBoardState();
					Task.Delay(800).Wait();
				}));
			}

		}
	}
}
