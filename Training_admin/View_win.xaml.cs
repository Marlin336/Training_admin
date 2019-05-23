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
		public View_win(Main_win super, int customer_id)
		{
			InitializeComponent();
			this.super = super;
			FillTable(customer_id);
		}

		private void FillTable(int c_id)
		{
			string sql = "select * from customer_subs(" + c_id + ")";
			NpgsqlCommand comm = new NpgsqlCommand(sql, super.conn);
			super.conn.Open();
			NpgsqlDataReader reader = comm.ExecuteReader();
			for (int i = 0; reader.Read(); i++)
			{
				GroupList item = new GroupList(reader.GetInt32(0), reader.GetString(1), reader.GetValue(2).ToString(), reader.GetValue(3).ToString(), reader.GetInt32(4), reader.GetInt32(5));
				dg.Items.Add(item);
			}
		}

		private void Dg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			string sql = "";
		}
	}
}
