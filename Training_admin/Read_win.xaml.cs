using System.Windows;

namespace Training_admin
{
	/// <summary>
	/// Логика взаимодействия для Read_win.xaml
	/// </summary>
	public partial class Read_win : Window
    {
        public Read_win(string text)
        {
            InitializeComponent();
			tb_read.Text = text;
        }
    }
}
