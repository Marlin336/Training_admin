using Npgsql;
using System;
using System.Windows;

namespace Training_admin
{
	/// <summary>
	/// Логика взаимодействия для Money_win.xaml
	/// </summary>
	public partial class Money_win : Window
	{
		Main_win super { get; }
		CustomList list { get; }
		private bool adding { get; }//true - добавление на счёт, false - снятие со счёта

		public Money_win(bool adding, Main_win super)
		{
			InitializeComponent();
			this.adding = adding;
			this.super = super;
			list = super.dg_customer.SelectedItem as CustomList;
			try
			{
				NpgsqlCommand comm = new NpgsqlCommand("select sname||' '||fname||' '||pname from my_own_admin(" + super.user_id + ")", super.conn);
				super.conn.Open();
				string admin = comm.ExecuteScalar().ToString();
				comm = new NpgsqlCommand("select sname||' '||fname||' '||pname from customer_view where id = " + list.id + "", super.conn);
				string cust = comm.ExecuteScalar().ToString();
				super.conn.Close();
				l_admin.Content += admin;
				l_cust.Content += cust;
			}
			catch (NpgsqlException ex)
			{
				MessageBox.Show(ex.Message, "Ошибка на сервере", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
			}
			finally
			{
				super.conn.Close();
			}
		}

		private void B_accept_Click(object sender, RoutedEventArgs e)
		{
			int add_money = adding ? (int)num_money.Value : -(int)num_money.Value;
			string sql = "CALL public.trans_money(" + list.id + ", " + super.user_id + ", " + add_money + ", '" + tb_descript.Text + "')";
			NpgsqlCommand comm = new NpgsqlCommand(sql, super.conn);
			try
			{
				super.conn.Open();
				comm.ExecuteNonQuery();
				Close();
			}
			catch (NpgsqlException ex)
			{
				if (ex.ErrorCode == -2147467259)
					MessageBox.Show("Счёт пользователя не может быть отрицательным", "Ошибка на сервере", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
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
