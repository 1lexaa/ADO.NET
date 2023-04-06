using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace ADO.net
{
    public partial class OrmWindow : Window
    {
        private SqlConnection _connection;  // Сonnection type.

        public ObservableCollection<Entity.Department> Departments { get; set; }  // Getting data from the department class.
        public ObservableCollection<Entity.Product> Product { get; set; }  // Getting data from the product class.
        public ObservableCollection<Entity.Manager> Manager { get; set; }  // Getting data from the manager class.
        public ObservableCollection<Entity.Sale> Sale { get; set; }  // Getting data from the sale class.

        public OrmWindow()
        {
            InitializeComponent();

            Departments = new ObservableCollection<Entity.Department>();
            Product = new ObservableCollection<Entity.Product>();
            Manager = new ObservableCollection<Entity.Manager>();
            Sale = new ObservableCollection<Entity.Sale>();
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
                #region Load Product.
                cmd.CommandText = "SELECT P.* FROM Products as P WHERE P.DeleteData IS NULL";
                reader = cmd.ExecuteReader();

                while (reader.Read())
                    Product.Add(new Entity.Product(reader));

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
                #region Load Sale.
                cmd.CommandText = "SELECT S.* FROM Sales as S";
                reader = cmd.ExecuteReader();

                while (reader.Read())
                    Sale.Add(new Entity.Sale(reader));

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

        #region Events click.
        private void MouseDoubleClick_ListView_Departments(object sender, MouseButtonEventArgs e)  // Clicking opens the data editor window in the department table.
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Entity.Department departments)
                {
                    CrudDepartment dialog = new CrudDepartment(departments);

                    if (dialog.ShowDialog() == true)
                    {
                        if (dialog.EditedDepartment is null)  // If delete.
                        {
                            Departments.Remove(departments);
                            MessageBox.Show($"Deleting {departments.Name}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                        }
                        else  // If save.
                        {
                            int index = Departments.IndexOf(departments);

                            Departments.Remove(departments);
                            Departments.Insert(index, departments);
                            MessageBox.Show($"Update {departments.Name}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
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
                            using SqlCommand cmd = new SqlCommand() { Connection = _connection };

                            cmd.CommandText = "UPDATE Products SET DeleteData = CURRENT_TIMESTAMP WHERE Id = @Id";
                            cmd.Parameters.AddWithValue("@Id", product.Id);

                            try
                            {
                                cmd.ExecuteNonQuery();
                                Product.Remove(product);
                                MessageBox.Show($"Deleting {product.Name}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            catch (Exception ex) { MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); }

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
            if (sender is ListViewItem item)
            {
                if (item.Content is Entity.Manager manager)
                {
                    CrudManager dialog = new CrudManager(manager) { Owner = this };

                    if (dialog.ShowDialog() == true)
                    {

                    }
                }
            }
        }

        private void MouseDoubleClick_ListView_Sale(object sender, MouseButtonEventArgs e)  // Clicking opens the data editor window in the Sale table.
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Entity.Sale sale)
                {
                    CrudSale dialog = new CrudSale(sale) { Owner = this };

                    if (dialog.ShowDialog() == true)
                    {
                        if (dialog.EditSale is null)  // If delete.
                        {
                            using SqlCommand cmd = new SqlCommand() { Connection = _connection };

                            cmd.CommandText = "UPDATE Sales SET DeleteDt = CURRENT_TIMESTAMP WHERE Id = @Id";
                            cmd.Parameters.AddWithValue("@Id", sale.Id);

                            try
                            {
                                cmd.ExecuteNonQuery();
                                Sale.Remove(sale);
                                MessageBox.Show($"Deleting {sale.IdProduct}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            catch (Exception ex) { MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); }

                        }
                        else  // If save.
                        {
                            int index = Sale.IndexOf(sale);

                            Sale.Remove(sale);
                            Sale.Insert(index, sale);
                            MessageBox.Show($"Update {sale.IdProduct}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
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


        private void Click_Button_AddSale(object sender, RoutedEventArgs e)  // The method allows you to add data to the Sales table.
        {
            CrudSale dialog = new CrudSale(null!) { Owner = this };

            if (dialog.ShowDialog() == true && dialog.EditSale is not null)
            {
                using SqlCommand cmd = new SqlCommand($"INSERT INTO Sales (Id, ProductId, ManagerId, Cnt, SaleDt) VALUES (@Id, @ProductId, @ManagerId, @Cnt, @SaleDt)", _connection);
                cmd.Parameters.AddWithValue("@Id", dialog.EditSale.Id);
                cmd.Parameters.AddWithValue("@ManagerId", dialog.EditSale.IdManager);
                cmd.Parameters.AddWithValue("@ProductId", dialog.EditSale.IdProduct);
                cmd.Parameters.AddWithValue("@Cnt", dialog.EditSale.Cnt);
                cmd.Parameters.AddWithValue("@SaleDt", dialog.EditSale.SaleDt);

                try
                {
                    cmd.ExecuteNonQuery();
                    Sale.Add(dialog.EditSale);
                    MessageBox.Show("Add true");
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }
        #endregion
    }
}