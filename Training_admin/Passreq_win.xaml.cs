using Npgsql;
using System;
using System.Windows;

namespace Training_admin
{
	/// <summary>
	/// Логика взаимодействия для Passreq_win.xaml
	/// </summary>
	public partial class Passreq_win : Window
    {
		int user_id;
		Profile_win profile { get; }
		public Main_win super { get; }

        public Passreq_win(Main_win super, Profile_win profile ,int id)
        {
            InitializeComponent();
			this.profile = profile;
			this.super = super;
			user_id = id;
        }

		private void B_accept_Click(object sender, RoutedEventArgs e)
		{
			NpgsqlCommand comm = new NpgsqlCommand("select pass from my_own_admin(" + user_id + ")", super.conn);
			NpgsqlDataReader reader = null;
			try
			{
				super.conn.Open();
				string res = comm.ExecuteScalar().ToString();
				if (tb_pass.Password == res)
				{
					comm = new NpgsqlCommand("select * from my_own_admin(" + user_id + ")", super.conn);
					reader = comm.ExecuteReader();
					reader.Read();
					Reg_win edit = new Reg_win(profile, reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5));
					Close();
					edit.ShowDialog();
				}
				else
					MessageBox.Show("Неверный пароль", "Ошибка подтверждения", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			super.conn.Close();
		}
	}
}
