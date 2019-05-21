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
			NpgsqlCommand comm = new NpgsqlCommand("select sname||' '||fname||' '||pname from my_own_admin(" + super.user_id + ")", super.conn);
			super.conn.Open();
			string admin = comm.ExecuteScalar().ToString();
			comm = new NpgsqlCommand("select sname||' '||fname||' '||pname from customer_view_admin where id = " + list.id + "", super.conn);
			string cust = comm.ExecuteScalar().ToString();
			super.conn.Close();
			l_admin.Content += admin;
			l_cust.Content += cust;
		}

		private void B_accept_Click(object sender, RoutedEventArgs e)
		{
			int add_money = adding ? (int)num_money.Value : -(int)num_money.Value;
			string sql = "CALL public.transfer_money(" + list.id + "," + add_money + ")";
			NpgsqlCommand comm = new NpgsqlCommand(sql, super.conn);
			try
			{
				super.conn.Open();
				comm.ExecuteNonQuery();
				sql = "INSERT INTO public.transact_log(customer_id, admin_id, addition, date, \"time\", description) VALUES(" + list.id + ", " + super.user_id + ", " + add_money + ", '" + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToLongTimeString() + "', '" + tb_descript.Text + "'); ";
				comm = new NpgsqlCommand(sql, super.conn);
				comm.ExecuteNonQuery();
				Close();
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
				super.UpdateCustomGrid();
			}
		}
	}
}
