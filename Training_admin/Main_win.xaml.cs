using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Training_admin
{
	/// <summary>
	/// Логика взаимодействия для Main_win.xaml
	/// </summary>
	public partial class Main_win : Window
	{
		private bool logout { set; get; } = false;
		public Login_win super { get; private set; }
		public Main_win(Login_win super)
		{
			InitializeComponent();
			this.super = super;
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (logout)
			{
				if (MessageBox.Show("Вы действительно хотите выйти?", "Выйти?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
				{
					super.tb_log.Clear();
					super.tb_pass.Clear();
					super.Show();
				}
				else
				{
					logout = false;
					e.Cancel = true;
				}
			}
			else
			{
				if (MessageBox.Show("Вы действительно хотите закрыть приложение?", "Закрыть приложение?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
					Application.Current.Shutdown();
				else
					e.Cancel = true;
			}
		}

		private void B_exit_Click(object sender, RoutedEventArgs e)
		{
			logout = true;
			Close();
		}

		private void Mb_log_Click(object sender, RoutedEventArgs e)
		{
			View_win log_win = new View_win()
			{
				Title = "Журнал посещений"
			};
			log_win.Show();
		}

		private void Mb_trans_Click(object sender, RoutedEventArgs e)
		{
			View_win trans_win = new View_win()
			{
				Title = "Журнал денежных транзакций"
			};
			trans_win.Show();
		}

		private void Mb_add_Click(object sender, RoutedEventArgs e)
		{
			Money_win add_money = new Money_win(true)
			{
				Title = "Начисление денег"
			};
			add_money.Show();
		}

		private void Mb_take_Click(object sender, RoutedEventArgs e)
		{
			Money_win take_money = new Money_win(false)
			{
				Title = "Снятие денег"
			};
			take_money.Show();
		}
	}
}
