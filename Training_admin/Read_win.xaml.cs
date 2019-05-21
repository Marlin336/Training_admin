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
