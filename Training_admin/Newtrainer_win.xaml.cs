﻿using Npgsql;
using System;
using System.Windows;

namespace Training_admin
{
	/// <summary>
	/// Логика взаимодействия для Newtrainer_win.xaml
	/// </summary>
	public partial class Newtrainer_win : Window
    {
		Main_win super { get; }

        public Newtrainer_win(Main_win super)
        {
            InitializeComponent();
			this.super = super;
        }

		private void B_reg_Click(object sender, RoutedEventArgs e)
		{
			if (tb_pass.Password == tb_repass.Password)
			{
				string fname = tb_name.Text.Trim();
				string sname = tb_sname.Text.Trim();
				string pname = tb_pname.Text.Trim();
				string birth = date_birth.Text;
				string mail = tb_mail.Text.Trim();
				string login = tb_login.Text;
				string pass = tb_pass.Password;
				try
				{
					if (fname.Length == 0 || sname.Length == 0 || pname.Length == 0 || birth.Length == 0 || login.Length == 0 || pass.Length == 0)
						throw new ArgumentNullException();
					NpgsqlCommand comm = new NpgsqlCommand("INSERT INTO public.trainer(first_name, second_name, parent_name, birthday, e_mail, login, pass)" +
		"VALUES('" + fname + "', '" + sname + "', '" + pname + "', '" + birth + "', '" + mail + "', '" + login + "', '" + pass + "'); ", super.conn);
					super.conn.Open();
					comm.ExecuteNonQuery();
					comm = new NpgsqlCommand("CREATE USER \"trainer_" + login + "\" WITH LOGIN NOSUPERUSER NOCREATEDB NOCREATEROLE INHERIT NOREPLICATION CONNECTION LIMIT - 1 PASSWORD '" + pass + "';" +
					"GRANT \"Trainer\" TO \"trainer_" + login + "\"; ", super.conn);
					comm.ExecuteNonQuery();
					Close();
					super.conn.Close();
					super.UpdateTrainerGrid();
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
				{ super.conn.Close(); }
			}
			else
				MessageBox.Show("Пароли не совпадают", "Ошибка подтверждение пароля", MessageBoxButton.OK, MessageBoxImage.Warning);
		}
	}
}
