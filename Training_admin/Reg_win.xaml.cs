using Npgsql;
using System;
using System.Windows;

namespace Training_admin
{
	/// <summary>
	/// Логика взаимодействия для Reg_win.xaml
	/// </summary>
	public partial class Reg_win : Window
	{
		Profile_win profile = null;
		int user_id;
		string fname = null;
		string sname = null;
		string pname = null;
		string login = null;
		string pass = null;

		public Reg_win()
		{
			InitializeComponent();
			b_reg.Click += B_reg_Click;
		}
		public Reg_win(Profile_win profile, int id, string fname, string sname, string pname, string login, string pass)
		{
			InitializeComponent();
			Title = "Редактирование";
			b_reg.Click += B_edit_Click;
			this.profile = profile;
			user_id = id;
			tb_name.Text = this.fname = fname;
			tb_sname.Text = this.sname = sname;
			tb_pname.Text = this.pname = pname;
			tb_login.Text = this.login = login;
			tb_pass.Password = this.pass = pass;
			tb_repass.Password = pass;
		}
		#region events
		//  При регистрации нового профиля
		private void B_reg_Click(object sender, RoutedEventArgs e)
		{
			if (tb_pass.Password == tb_repass.Password)
			{
				string conn_param = "Server=127.0.0.1;Port=5432;User Id=Training_login;Password=0000;Database=Training;";
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
					comm = new NpgsqlCommand("CREATE USER \"admin_" + tb_login.Text + "\" WITH LOGIN NOSUPERUSER NOCREATEDB CREATEROLE INHERIT NOREPLICATION CONNECTION LIMIT - 1 PASSWORD '" + tb_pass.Password + "';" +
					"GRANT \"Admin\" TO \"admin_" + tb_login.Text + "\";", conn);
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
				finally
				{ conn.Close(); }
			}
			else
				MessageBox.Show("Пароли не совпадают", "Ошибка подтверждение пароля", MessageBoxButton.OK, MessageBoxImage.Warning);
		}
		// При редактировании профиля
		private void B_edit_Click(object sender, RoutedEventArgs e)
		{
			if (tb_pass.Password == tb_repass.Password)
			{
				string conn_param = "Server=127.0.0.1;Port=5432;User Id=Training_login;Password=0000;Database=Training;";
				string sql = "UPDATE admin SET first_name='" + tb_name.Text + "', second_name='" + tb_sname.Text + "', parent_name='" + tb_pname.Text + "', login='" + tb_login.Text + "', pass='" + tb_pass.Password + "' WHERE id =" + user_id + "";
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
					if (login != tb_login.Text)
					{
						comm.CommandText = "ALTER USER \"admin_" + login + "\" RENAME TO \"admin_" + tb_login.Text + "\";";
						comm.ExecuteNonQuery();
					}
					comm.CommandText = "ALTER USER \"admin_"+tb_login.Text+"\" PASSWORD '"+tb_pass.Password+"'; ";
					comm.ExecuteNonQuery();
					conn.Close();
					profile.l_name.Content = tb_sname.Text + " " + tb_name.Text + " " + tb_pname.Text;
					profile.l_login.Content = "Логин: " + tb_login.Text;
					Close();
				}
				catch (ArgumentNullException)
				{
					MessageBox.Show("Необходимые поля не заполнены", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
				}
			}
			else
				MessageBox.Show("Пароли не совпадают", "Ошибка подтверждение пароля", MessageBoxButton.OK, MessageBoxImage.Warning);
		}
		private void Tb_pass_GotFocus(object sender, RoutedEventArgs e)
		{
			tb_repass.Clear();
		}
		#endregion
	}
}
