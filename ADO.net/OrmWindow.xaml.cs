using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
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
    public ObservableCollection<Entity.Department> Departments { get; set; }  // Getting data from the department class.
    public ObservableCollection<Entity.Product> Product { get; set; }  // Getting data from the product class.
    public ObservableCollection<Entity.Manager> Manager { get; set; }  // Getting data from the manager class.

    public partial class OrmWindow : Window
    {
        public OrmWindow()
        {
            InitializeComponent();
            Departments = new ObservableCollection<Entity.Department>();
            Manager = new ObservableCollection<Entity.Manager>();
            Product = new ObservableCollection<Entity.Product>();
            DataContext = this;  // {Binding Departments}
            _connection = new SqlConnection(App._connection_string);
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)  // When loading a window, we pull data from the table.
        {
            try
            {
                _connection.Open();

                SqlCommand cmd = new SqlCommand() { Connection = _connection };

                #region Load Department.
                cmd.CommandText = "SELECT Id, Name FROM Departments";

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                    Departments.Add(new Entity.Department() { Id = reader.GetGuid(0), Name = reader.GetString(1) });

                reader.Close();
                #endregion
                #region Load Manager.
                cmd.CommandText = "SELECT M.Id, M.Surname, M.Name, M.Secname, M.Id_main_Dep, M.Id_sec_dep, M.Id_chief FROM Managers as M";
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Manager.Add(new Entity.Manager()
                    {
                        Id = reader.GetGuid(0),
                        Surname = reader.GetString(1),
                        Name = reader.GetString(2),
                        Secname = reader.GetString(3),
                        IdMainDep = reader.GetGuid(4),
                        IdSecDep = reader.GetValue(5) == DBNull.Value ? null : reader.GetGuid(5),
                        IdChief = reader.IsDBNull(6) ? null : reader.GetGuid(6)
                    });
                }

                reader.Close();
                #endregion
                #region Load Product.
                cmd.CommandText = "SELECT P.Id, P.Name, P.Price FROM Products as P";
                reader = cmd.ExecuteReader();

                while (reader.Read())
                    Product.Add(new Entity.Product() { Id = reader.GetGuid(0), Name = reader.GetString(1), Price = reader.GetDouble(2) });

                reader.Close();
                #endregion

                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Window will be cloded", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
        private void MouseDoubleClick_ListView_Products(object sender, MouseButtonEventArgs e)  // Clicking opens the data editor window in the product table.
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Entity.Product product)
                {
                    CrudProduct dialog = new CrudProduct(product);

                    if (dialog.ShowDialog() == true)
                    {
                        if (dialog.EditProduct is null)  // If delete.
                        {
                            Product.Remove(product);
                            MessageBox.Show($"Deleting {product.Name}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                        }
                        else  // If save.
                        {
                            int index = Product.IndexOf(product);

                            Product.Remove(product);
                            Product.Insert(index, product);
                            MessageBox.Show($"Update {product.Name}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
        }

        private void MouseDoubleClick_ListView_Manager(object sender, MouseButtonEventArgs e)  // Clicking opens the data editor window in the manager table.
        {

        }
#endregion
        #region Events add.
        private void Click_Button_AddDepartment(object sender, RoutedEventArgs e)  // The method allows you to add data to the Departments table.
        {
            CrudDepartment dialog = new CrudDepartment(null!);

            if (dialog.ShowDialog() == true)
            {
                if (dialog.EditedDepartment is not null)
                {
                    String sql = $"INSERT INTO Departments(Id, Name) VALUES (@id, @name)";
                    using SqlCommand cmd = new SqlCommand(sql, _connection);

                    cmd.Parameters.AddWithValue("@id", dialog.EditedDepartment.Id);
                    cmd.Parameters.AddWithValue("@name", dialog.EditedDepartment.Name);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        Departments.Add(dialog.EditedDepartment);
                        MessageBox.Show("Add", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); }
                }
            }
        }

        private void Click_Button_AddProduct(object sender, RoutedEventArgs e)  // The method allows you to add data to the Products table.
        {
            CrudProduct dialog = new CrudProduct(null!);

            if (dialog.ShowDialog() == true)
            {
                if (dialog.EditProduct is not null)
                {
                    String sql = $"INSERT INTO Products(Id, Name, Price) VALUES (@id, @name, @price)";
                    using SqlCommand cmd = new SqlCommand(sql, _connection);

                    cmd.Parameters.AddWithValue("@id", dialog.EditProduct.Id);
                    cmd.Parameters.AddWithValue("@name", dialog.EditProduct.Name);
                    cmd.Parameters.AddWithValue("@price", dialog.EditProduct.Price);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        Product.Add(dialog.EditProduct);
                        MessageBox.Show("Add", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); }
                }
            }
        }

        private void Click_Button_AddManager(object sender, RoutedEventArgs e)
        {

        }
        #endregion
    }
}
