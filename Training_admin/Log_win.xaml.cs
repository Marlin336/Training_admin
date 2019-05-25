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
			FillTransTable();
			FillLogTable();
		}

		private void FillTransTable()
		{
			string sql = "SELECT * FROM public.transact_log_view_admin";
			NpgsqlCommand comm = new NpgsqlCommand(sql, super.conn);
			try
			{
				super.conn.Open();
				NpgsqlDataReader reader = comm.ExecuteReader();
				for (int i = 0; reader.Read(); i++)
				{
					var par0 = reader.GetInt32(0);
					var par1 = reader.GetString(1);
					var par2 = reader.GetString(2);
					var par3 = reader.GetInt32(3);
					var par4 = reader.GetDate(4).ToString();
					var par5 = reader.GetValue(5).ToString();
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
		private void FillLogTable()
		{
			string sql = "SELECT id, group_id, date, in_time, out_time, admin, customer, trainer, transact_id FROM public.log_view";
			NpgsqlCommand comm = new NpgsqlCommand(sql, super.conn);
			try
			{
				super.conn.Open();
				NpgsqlDataReader reader = comm.ExecuteReader();
				for (int i = 0; reader.Read(); i++)
				{
					var par0 = reader.GetInt32(0);
					var par1 = reader.GetInt32(1);
					var par2 = reader.GetDate(2).ToString();
					var par3 = reader.GetValue(3).ToString();
					var par4 = reader.GetValue(4).ToString();
					var par5 = reader.GetString(5);
					var par6 = reader.GetString(6);
					var par7 = reader.GetString(7);
					var par8 = reader.GetInt32(8);
					LogList item = new LogList(par0, par1, par2, par3, par4, par5, par6, par7, par8);
					dg_log.Items.Add(item);
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
		public void UpdateLog()
		{
			dg_log.Items.Clear();
			FillLogTable();
		}
		public void UpdateTransact()
		{
			dg_transact.Items.Clear();
			FillTransTable();
		}

		private void Dg_transact_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			TransactList item = dg_transact.SelectedItem as TransactList;
			string sql = "select description from transact_log where id = " + item.id + "";
			NpgsqlCommand comm = new NpgsqlCommand(sql, super.conn);
			super.conn.Open();
			var a = comm.ExecuteScalar().ToString();
			Read_win read = new Read_win(comm.ExecuteScalar().ToString());
			read.Show();
			super.conn.Close();
		}

		private void Mb_re_log_Click(object sender, RoutedEventArgs e)
		{
			UpdateLog();
		}

		private void Mb_re_transact_log_Click(object sender, RoutedEventArgs e)
		{
			UpdateTransact();
		}

		private void Dg_log_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			Dispatcher.BeginInvoke((Action)(() => tab_panel.SelectedItem = tab_transact));
			LogList selected = dg_log.SelectedItem as LogList;
			foreach (TransactList item in dg_transact.Items)
			{
				if (item.id == selected.transact_id)
				{
					dg_log.SelectedItem = item;
					dg_log.ScrollIntoView(item);
					break;
				}
			}
		}
	}
}
