using ADO.net.Entity;
using System;

using System.Data.SqlClient;
using System.Windows;

namespace ADO.net
{
    /// <summary>
    /// Логика взаимодействия для CrudProduct.xaml
    /// </summary>
    public partial class CrudProduct : Window
    {
        private SqlConnection _connection;

        public Entity.Product EditProduct { get; private set; }

        public CrudProduct(Entity.Product product)
        {
            InitializeComponent();

            _connection = new SqlConnection() { ConnectionString = App._connection_string };
            this.EditProduct = product;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _connection.Open();

            if (EditProduct is null)
            {
                EditProduct = new Product() { Id = Guid.NewGuid() };
                DeleteButton.IsEnabled = false;
            }
            else
            {
                TextBox_Name.Text = EditProduct.Name;
                DeleteButton.IsEnabled = true;
                TextBox_Price.Text = EditProduct.Price.ToString();
            }

            TextBox_Id.Text = EditProduct.Id.ToString();
        }

        private void Click_Button_Save(object sender, RoutedEventArgs e)
        {
            EditProduct.Name = TextBox_Name.Text;
            EditProduct.Price = Convert.ToDouble(TextBox_Price.Text);
            this.DialogResult = true;
        }

        private void Click_Button_Delete(object sender, RoutedEventArgs e)
        {
            EditProduct = null!;
            this.DialogResult = true;
        }

        private void Click_Button_Cancel(object sender, RoutedEventArgs e) => this.DialogResult = false;

        private void Window_Closed(object sender, EventArgs e) => _connection.Close();
    }
}