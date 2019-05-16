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
        public Passreq_win()
        {
            InitializeComponent();
        }

		private void B_accept_Click(object sender, RoutedEventArgs e)
		{
			if (tb_pass.Password == "0000")
			{
				Reg_win edit = new Reg_win(true);
				Close();
				edit.ShowDialog();
			}
			else
				MessageBox.Show("Неверный пароль", "Ошибка подтверждения", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}
