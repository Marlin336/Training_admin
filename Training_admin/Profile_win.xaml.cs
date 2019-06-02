using System.Windows;

namespace Training_admin
{
	/// <summary>
	/// Логика взаимодействия для Profile_win.xaml
	/// </summary>
	public partial class Profile_win : Window
    {
		public Main_win super { get; }

        public Profile_win(Main_win super, string name, string login)
        {
            InitializeComponent();
			this.super = super;
			l_name.Content = name;
			l_login.Content = "Логин: " + login;
        }

		private void B_edit_Click(object sender, RoutedEventArgs e)
		{
			Passreq_win req = new Passreq_win(super, this ,super.user_id);
			req.Show();
		}
	}
}
