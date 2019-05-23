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
	/// Логика взаимодействия для View_win.xaml
	/// </summary>
	public partial class View_win : Window
	{
		Main_win super { get; }
		public View_win(Main_win super, int customer_id)
		{
			InitializeComponent();
			this.super = super;
		}

		private void FillTable(int c_id)
		{

		}
	}
}
