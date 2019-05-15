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
	/// Логика взаимодействия для Money_win.xaml
	/// </summary>
	public partial class Money_win : Window
	{
		private bool adding { get; }//true - добавление на счёт, false - снятие со счёта
		public Money_win(bool adding)
		{
			InitializeComponent();
			this.adding = adding;
		}
	}
}
