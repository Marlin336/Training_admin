using Npgsql;
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

namespace Training_admin
{
    /// <summary>
    /// Логика взаимодействия для Passreq_win.xaml
    /// </summary>
    public partial class Passreq_win : Window
    {
		int user_id;
		public Main_win super { get; }
        public Passreq_win(Main_win super, int id)
        {
            InitializeComponent();
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
					Reg_win edit = new Reg_win(reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5));
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
