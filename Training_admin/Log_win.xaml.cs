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
    /// Логика взаимодействия для Log_win.xaml
    /// </summary>
    public partial class Log_win : Window
    {
		public Main_win super { get; }
        public Log_win(Main_win super)
        {
            InitializeComponent();
			this.super = super;
			string sql = "SELECT * FROM public.transact_log_view_admin";
			NpgsqlCommand comm = new NpgsqlCommand(sql, super.conn);
			try
			{
				super.conn.Open();
				NpgsqlDataReader reader = comm.ExecuteReader();
				for (int i = 0; reader.Read(); i++)
				{
					var a0 = reader.GetInt32(0);
					var a1 = reader.GetString(1);
					var a2 = reader.GetString(2);
					var a3 = reader.GetInt32(3);
					var a4 = reader.GetDate(4).ToString();
					var a5 = reader.GetValue(5).ToString();
					TransactList item = new TransactList(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetDate(4).ToString(), reader.GetValue(5).ToString());
					dg_transact.Items.Add(item);
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
		public Log_win(string search, Main_win super)
		{
			InitializeComponent();
			this.tb_search.Text = search;
			this.super = super;
		}

		private void Tb_search_GotFocus(object sender, RoutedEventArgs e)
		{
			if(tb_search.Foreground == Brushes.DarkGray)
			{
				tb_search.Foreground = Brushes.Black;
				tb_search.Clear();
			}
		}

		private void Tb_search_LostFocus(object sender, RoutedEventArgs e)
		{
			if(tb_search.Text == "")
			{
				tb_search.Foreground = Brushes.DarkGray;
				tb_search.Text = "Поиск...";
			}
		}
	}
}
