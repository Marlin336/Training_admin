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
			FillCustomerGrid();
		}

		private void FillCustomerGrid()
		{
			NpgsqlCommand comm = new NpgsqlCommand("select * from customer_view_admin", conn);
			NpgsqlDataReader reader;
			try
			{
				conn.Open();
				reader = comm.ExecuteReader();
				for (int i = 0; reader.Read(); i++)
				{
					CustomList item = new CustomList(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetDate(4).ToString(), reader.GetValue(5).ToString(), reader.GetInt32(6), reader.GetString(7), i%2==0);
					dg_customer.Items.Add(item);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				throw;
			}
			conn.Close();
		}
		private void Mb_re_cust_Click(object sender, RoutedEventArgs e)
		{
			dg_customer.Items.Clear();
			FillCustomerGrid();
		}
		private void Tab_cust_GotFocus(object sender, RoutedEventArgs e)
		{
			dg_customer.Items.Clear();
			FillCustomerGrid();
		}

		private void FillTrainerGrid()
		{
			NpgsqlCommand comm = new NpgsqlCommand("select * from trainer_view_admin", conn);
			NpgsqlDataReader reader;
			try
			{
				conn.Open();
				reader = comm.ExecuteReader();
				for (int i = 0; reader.Read(); i++)
				{
					TrainerList item = new TrainerList(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetDate(4).ToString(), reader.GetValue(5).ToString(), reader.GetString(6), reader.GetInt32(7));
					dg_trainer.Items.Add(item);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				throw;
			}
			conn.Close();
		}
		private void Mb_re_trainer_Click(object sender, RoutedEventArgs e)
		{
			dg_trainer.Items.Clear();
			FillTrainerGrid();
		}
		private void Tab_trainer_GotFocus(object sender, RoutedEventArgs e)
		{
			dg_trainer.Items.Clear();
			FillTrainerGrid();
		}

		private void FillGroupGrid()
		{
			NpgsqlCommand comm = new NpgsqlCommand("select * from group_view_admin", conn);
			NpgsqlDataReader reader;
			try
			{
				conn.Open();
				reader = comm.ExecuteReader();
				for (int i = 0; reader.Read(); i++)
				{
					GroupList item = new GroupList(reader.GetInt32(0), reader.GetString(1), reader.GetValue(2).ToString(), reader.GetValue(3).ToString(), reader.GetInt32(4), reader.GetInt32(5));
					dg_group.Items.Add(item);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				throw;
			}
			conn.Close();
		}
		private void Tab_group_GotFocus(object sender, RoutedEventArgs e)
		{
			dg_group.Items.Clear();
			FillGroupGrid();
		}
		private void Mb_re_group_Click(object sender, RoutedEventArgs e)
		{
			dg_group.Items.Clear();
			FillGroupGrid();
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
