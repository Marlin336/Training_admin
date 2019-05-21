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
using Npgsql;

namespace Training_admin
{
	class CustomList
	{
		public int id { get; set; }
		public string fname { get; set; }
		public string sname { get; set; }
		public string pname { get; set; }
		public string birthday { get; set; }
		public string mail { get; set; }
		public string login { get; set; }
		public int deposit { get; set; }
		public CustomList(int id, string sname, string fname, string pname, string birthday, string mail, int dep, string login)
		{
			this.id = id;
			this.fname = fname;
			this.sname = sname;
			this.pname = pname;
			this.birthday = birthday;
			this.mail = mail;
			this.login = login;
			deposit = dep;
		}
	}

	/// <summary>
	/// Логика взаимодействия для Main_win.xaml
	/// </summary>
	public partial class Main_win : Window
	{
		public int user_id { get; }
		private bool logout { set; get; } = false;
		public Login_win super { get; private set; }
		public NpgsqlConnection conn;
		public Main_win(Login_win super, int id, string login, string password)
		{
			InitializeComponent();
			this.super = super;
			user_id = id;
			string str = "Server = 127.0.0.1; Port = 5432; User Id = " + login + "; Password = " + password + "; Database = Training;";
			conn = new NpgsqlConnection(str);
			NpgsqlCommand comm = new NpgsqlCommand("select * from customer_view_admin", conn);
			NpgsqlDataReader reader;
			try
			{
				conn.Open();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				throw;
			}
			reader = comm.ExecuteReader();
			for (int i = 0; reader.Read(); i++)
			{
				CustomList item = new CustomList(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetDate(4).ToString(), reader.GetValue(5).ToString(), reader.GetInt32(6), reader.GetString(7));
				dg_customer.Items.Add(item);
			}
			conn.Close();
		}

		private void FillCustomerGrid()
		{

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

		private void Mb_log_view_Click(object sender, RoutedEventArgs e)
		{
			Log_win log = new Log_win();
			log.Show();
		}

		private void B_profile_Click(object sender, RoutedEventArgs e)
		{
			Profile_win profile = new Profile_win();
			profile.Show();
		}

		private void Mb_add_trainer_Click(object sender, RoutedEventArgs e)
		{
			Newtrainer_win win = new Newtrainer_win();
			win.Show();
		}
	}
}
