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
    /// Логика взаимодействия для Log_win.xaml
    /// </summary>
    public partial class Log_win : Window
    {
        public Log_win()
        {
            InitializeComponent();
        }
		public Log_win(string search)
		{
			InitializeComponent();
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
