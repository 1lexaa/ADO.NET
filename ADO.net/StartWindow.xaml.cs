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

namespace ADO.net
{
    /// <summary>
    /// Логика взаимодействия для StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void Click_Button_Basics(object sender, RoutedEventArgs e)  // The button shows the main window, the window displays all the data of the database tables.
        {
            this.Hide();
            new MainWindow().ShowDialog();
            this.Show();
        }

        private void Click_Button_Orm(object sender, RoutedEventArgs e)  // The button opens a window for changing data.
        {
            this.Hide();
            new OrmWindow().ShowDialog();
            this.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new DalWindow().ShowDialog();
            this.Show();
        }

        private void Button_Click_EF(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new EFWindow().ShowDialog();
            this.Show();
        }
    }
}