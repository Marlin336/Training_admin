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
	/// Логика взаимодействия для View_win.xaml
	/// </summary>
	public partial class View_win : Window
	{
		Main_win super { get; }
		int id { get; }
		public View_win(Main_win super, Type type, int id)
		{
			InitializeComponent();
			this.super = super;
			this.id = id;
			if (type == typeof(CustomList))
			{
				dg_customer.Visibility = Visibility.Visible;
				FillTableCustomer();
			}
			else if(type == typeof(TrainerList))
			{
				dg_trainer.Visibility = Visibility.Visible;
				FillTableTrainer();
			}
			else if (type == typeof(GroupList))
			{
				dg_group.Visibility = Visibility.Visible;
				FillTableGroup();
			}
		}

		private void FillTableCustomer()
		{
			string sql = "select * from customer_subs(" + id + ")";
			NpgsqlCommand comm = new NpgsqlCommand(sql, super.conn);
			try
			{
				super.conn.Open();
				NpgsqlDataReader reader = comm.ExecuteReader();
				for (int i = 0; reader.Read(); i++)
				{
					GroupList item = new GroupList(reader.GetInt32(0), reader.GetString(1), reader.GetValue(2).ToString(), reader.GetValue(3).ToString(), reader.GetInt32(4), reader.GetInt32(5));
					dg_customer.Items.Add(item);
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
			finally { super.conn.Close(); }
		}

		private void FillTableTrainer()
		{
			string sql = "select * from group_view_admin where trainer_id = " + id + "";
			NpgsqlCommand comm = new NpgsqlCommand(sql, super.conn);
			try
			{
				super.conn.Open();
				NpgsqlDataReader reader = comm.ExecuteReader();
				for (int i = 0; reader.Read(); i++)
				{
					GroupList item = new GroupList(reader.GetInt32(0), reader.GetString(2), reader.GetValue(3).ToString(), reader.GetValue(4).ToString(), reader.GetInt32(5), reader.GetInt32(6));
					dg_trainer.Items.Add(item);
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
			finally { super.conn.Close(); }
		}

		private void FillTableGroup()
		{
			string sql = "select cva.sname, cva.fname, cva.pname, cva.birthday, cva.age, cva.mail, cva.deposit, cva.customer_in "+
				"from customer_view_admin as cva, \"customer-customer_group\" as ccg where cva.id = ccg.id_customer and ccg.id_group = " + id + "";
			NpgsqlCommand comm = new NpgsqlCommand(sql, super.conn);
			try
			{
				super.conn.Open();
				NpgsqlDataReader reader = comm.ExecuteReader();
				for (int i = 0; reader.Read(); i++)
				{
					CustomList item = new CustomList(0, reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetDate(3).ToString(), (int)reader.GetDouble(4), reader.GetValue(5).ToString(), reader.GetInt32(6), reader.GetBoolean(7));
					dg_group.Items.Add(item);
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
			finally { super.conn.Close(); }
		}

		private void Dg_customer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			GroupList group = dg_customer.SelectedItem as GroupList;
			string sql = "CALL public.reg_entrance(" + id + ", " + group.id + ", " + super.user_id + ", " + group.cost + ", 'Оплата посещения занятия')";
			NpgsqlCommand comm = new NpgsqlCommand(sql, super.conn);
			try
			{
				super.conn.Open();
				comm.ExecuteNonQuery();
				super.conn.Close();
				Close();
			}
			catch (NpgsqlException ex)
			{
				if (ex.ErrorCode == -2147467259)
					MessageBox.Show("Недостаточно денег на счету", "Ошибка на сервере", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
				else
					MessageBox.Show(ex.Message, "Ошибка на сервере", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
			}
			finally
			{
				super.conn.Close();
				super.UpdateCustomGrid();
			}
		}
	}
}
