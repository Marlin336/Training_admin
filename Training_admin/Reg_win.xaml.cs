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
	/// Логика взаимодействия для Reg_win.xaml
	/// </summary>
	public partial class Reg_win : Window
	{
		public Reg_win()
		{
			InitializeComponent();
			b_reg.Click += B_reg_Click;
		}

		public Reg_win(string fname, string sname, string pname, string login, string pass)
		{
			InitializeComponent();
			Title = "Редактирование";
			b_reg.Click += B_edit_Click;
			tb_name.Text = fname;
			tb_sname.Text = sname;
			tb_pname.Text = pname;
			tb_login.Text = login;
			tb_pass.Password = pass;
			tb_repass.Password = pass;
		}

		//  При регистрации нового профиля
		private void B_reg_Click(object sender, RoutedEventArgs e)
		{
			if (tb_pass.Password == tb_repass.Password)
			{
				string conn_param = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=0000;Database=Training;";
				string sql = "INSERT INTO public.admin(" +
	"first_name, second_name, parent_name, login, pass)" +
	"VALUES('" + tb_name.Text.Trim() + "', '" + tb_sname.Text.Trim() + "', '" + tb_pname.Text.Trim() + "', '" + tb_login.Text + "', '" + tb_pass.Password + "');";
				NpgsqlConnection conn = new NpgsqlConnection(conn_param);
				NpgsqlCommand comm;
				try
				{					
					if (tb_name.Text.Trim().Length == 0 || tb_sname.Text.Trim().Length == 0 || tb_pname.Text.Trim().Length == 0 || tb_login.Text.Length == 0 || tb_pass.Password.Length == 0)
					{
						throw new ArgumentNullException();
					}				
					conn.Open();
					comm = new NpgsqlCommand(sql, conn);
					comm.ExecuteNonQuery();
					comm = new NpgsqlCommand("CREATE USER \"" + tb_login.Text + "\" WITH LOGIN NOSUPERUSER NOCREATEDB NOCREATEROLE INHERIT NOREPLICATION CONNECTION LIMIT - 1 PASSWORD '" + tb_pass.Password + "';" +
					"GRANT \"Admin\" TO \"" + tb_login.Text + "\";", conn);
					comm.ExecuteNonQuery();
					conn.Close();
					Close();
				}
				catch (ArgumentNullException)
				{
					MessageBox.Show("Необходимые поля не заполнены", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
				}
				catch (NpgsqlException ex)
				{
					MessageBox.Show(ex.Message, "Ошибка на сервере", MessageBoxButton.OK, MessageBoxImage.Error);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				}

			}
			else
				MessageBox.Show("Пароли не совпадают", "Ошибка подтверждение пароля", MessageBoxButton.OK, MessageBoxImage.Warning);
		}

		// При редактировании профиля
		private void B_edit_Click(object sender, RoutedEventArgs e)
		{
			if (tb_pass.Password == tb_repass.Password)
			{
				// TODO
				// Попытка регистрации

				//Проблема неверно заполненных полей должна обрабатываться сервером
			}
			else
				MessageBox.Show("Пароли не совпадают", "Ошибка подтверждение пароля", MessageBoxButton.OK, MessageBoxImage.Warning);
		}

		private void Tb_pass_GotFocus(object sender, RoutedEventArgs e)
		{
			tb_repass.Clear();
		}
	}
}
