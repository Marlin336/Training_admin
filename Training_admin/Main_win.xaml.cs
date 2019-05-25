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
			string conn_str = "Server = 127.0.0.1; Port = 5432; User Id = " + login + "; Password = " + password + "; Database = Training;";
			conn = new NpgsqlConnection(conn_str);
			FillCustomerGrid();
			FillTrainerGrid();
			FillGroupGrid();
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
					CustomList item = new CustomList(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetDate(4).ToString(), (int)reader.GetDouble(5), reader.GetValue(6).ToString(), reader.GetInt32(7), reader.GetBoolean(8));
					dg_customer.Items.Add(item);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			conn.Close();
		}
		public void UpdateCustomGrid()
		{
			dg_customer.Items.Clear();
			FillCustomerGrid();
		}
		private void Mb_re_cust_Click(object sender, RoutedEventArgs e)
		{
			UpdateCustomGrid();
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
					int count;
					try { count = reader.GetInt32(7); } catch { count = 0; }
					TrainerList item = new TrainerList(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetDate(4).ToString(), reader.GetValue(5).ToString(), reader.GetString(6), count);
					dg_trainer.Items.Add(item);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			conn.Close();
		}
		public void UpdateTrainerGrid()
		{
			dg_trainer.Items.Clear();
			FillTrainerGrid();
		}
		private void Mb_re_trainer_Click(object sender, RoutedEventArgs e)
		{
			UpdateTrainerGrid();
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
					GroupList item = new GroupList(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetValue(3).ToString(), reader.GetValue(4).ToString(), reader.GetInt32(5), reader.GetInt32(6));
					dg_group.Items.Add(item);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			conn.Close();
		}
		public void UpdateGroupGrid()
		{
			dg_group.Items.Clear();
			FillGroupGrid();
		}
		private void Mb_re_group_Click(object sender, RoutedEventArgs e)
		{
			UpdateGroupGrid();
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
			Money_win add_money = new Money_win(true, this)
			{
				Title = "Начисление денег"
			};
			add_money.Show();
		}

		private void Mb_take_Click(object sender, RoutedEventArgs e)
		{
			Money_win take_money = new Money_win(false, this)
			{
				Title = "Снятие денег"
			};
			take_money.Show();
		}

		private void Mb_log_view_Click(object sender, RoutedEventArgs e)
		{
			Log_win log = new Log_win(this);
			log.Show();
		}

		private void B_profile_Click(object sender, RoutedEventArgs e)
		{
			NpgsqlCommand comm = new NpgsqlCommand("select sname||' '||fname||' '||pname, login from my_own_admin(" + user_id + ")", conn);
			NpgsqlDataReader reader;
			string name = null, login = null;
			try
			{
				conn.Open();
				reader = comm.ExecuteReader();
				for (int i = 0; reader.Read(); i++)
				{
					name = reader.GetString(0);
					login = reader.GetString(1);
				}
				Profile_win profile = new Profile_win(this, name, login);
				profile.Show();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			conn.Close();
		}

		private void Mb_add_trainer_Click(object sender, RoutedEventArgs e)
		{
			Newtrainer_win win = new Newtrainer_win(this);
			win.Show();
		}

		private void Dg_trainer_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{
			mb_del_trainer.IsEnabled = mb_trainer_groups.IsEnabled = dg_trainer.SelectedCells.Count != 0;
		}

		private void Mb_del_trainer_Click(object sender, RoutedEventArgs e)
		{
			if(MessageBox.Show("Вы уверенны что хотите удалить этого тренера из базы данных?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
			{
				TrainerList list = dg_trainer.SelectedItem as TrainerList;
				string sql = "drop role \"" + list.login + "\"";
				NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
				conn.Open();
				comm.ExecuteNonQuery();
				comm = new NpgsqlCommand("DELETE FROM public.trainer WHERE id = " + list.id + "; ", conn);
				comm.ExecuteNonQuery();
				conn.Close();
			}
		}

		private void Dg_customer_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{
			mb_money_op.IsEnabled = mb_cust_enter.IsEnabled = dg_customer.SelectedCells.Count != 0;
		}

		private void Mb_cust_enter_Click(object sender, RoutedEventArgs e)
		{
			CustomList sel_cust = dg_customer.SelectedItem as CustomList;
			string sql = "select customer_in("+ sel_cust.id + ")";
			NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
			try
			{
				conn.Open();
				bool enter = (bool)comm.ExecuteScalar();
				conn.Close();
				if (enter)//Клиент внутри
				{
					comm = new NpgsqlCommand("select cust_in_gr from log_view where customer_id = " + sel_cust.id + " and out_time is null", conn);
					conn.Open();
					int cust_id = (int)comm.ExecuteScalar();
					conn.Close();
					sql = "UPDATE public.log SET out_time = '" + DateTime.Now.ToLongTimeString() + "' WHERE id_customer_in_group = " + cust_id + " and out_time is null; ";
					comm = new NpgsqlCommand(sql, conn);
					conn.Open();
					comm.ExecuteNonQuery();
					conn.Close();
					UpdateCustomGrid();
				}
				else
				{
					View_win win = new View_win(this, typeof(CustomList), sel_cust.id)
					{
						Title = sel_cust.sname + " " + sel_cust.fname + " " + sel_cust.pname
					};
					win.Show();
				}
			}
			catch (NpgsqlException ex)
			{
				MessageBox.Show(ex.Message, "Ошибка на сервере", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
			}
			finally { conn.Close(); }
		}

		private void Mb_trainer_groups_Click(object sender, RoutedEventArgs e)
		{
			TrainerList sel_tr = dg_trainer.SelectedItem as TrainerList;
			View_win win = new View_win(this, typeof(TrainerList), sel_tr.id)
			{
				Title = sel_tr.sname + " " + sel_tr.fname + " " + sel_tr.pname
			};
			win.Show();
		}

		private void Mb_gr_subs_Click(object sender, RoutedEventArgs e)
		{
			GroupList sel_gr = dg_group.SelectedItem as GroupList;
			View_win win = new View_win(this, typeof(GroupList), sel_gr.id)
			{
				Title = "Код группы: " + sel_gr.id
			};
			win.Show();
		}
	}
}
