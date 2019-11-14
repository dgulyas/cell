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
					wnd.currentBoard = b;
					this.Dispatcher.Invoke((Action)(() =>
					{
						wnd.InvalidateVisual();
						wnd.DrawBoardState();
						Thread.Sleep(200);
					}));
				}
			});
		}
	}
}
