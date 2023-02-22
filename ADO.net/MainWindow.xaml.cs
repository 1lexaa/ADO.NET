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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ADO.net
public partial class MainWindow : Window
{
    private SqlConnection _connection;  // Connection object (The basis ADO).

    public MainWindow()
    {
        InitializeComponent();

        _connection = new SqlConnection() { ConnectionString = App._connection_string };  // Get connection string from app file.
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)  // When loading the window, open a connection to the server, 
    {                                                            // indicate the connection status, display data.
        try
        {
            _connection.Open();

            Label_StatusConnection.Content = "connected";
            Label_StatusConnection.Foreground = Brushes.Green;
        }
        catch (SqlException ex)
        {
            Label_StatusConnection.Content = "disconnected";
            Label_StatusConnection.Foreground = Brushes.Red;

            MessageBox.Show(ex.Message, "DB Error", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
        ShowMonitor();
        Showviews();
    }

    #region Query and return results.
    #region Query create.
    private void Click_CreateDepartments(object sender, RoutedEventArgs e)  // The method creates a table with departments.
    {
        String sql = File.ReadAllText("RequestsSQL/RequestNewDepartmentTable.sql");
        using SqlCommand cmd = new SqlCommand(sql, _connection);

        try
        {
            cmd.ExecuteNonQuery();
            MessageBox.Show("Create departments OK", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (SqlException ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }

        cmd.Dispose();  // Releasing a resource.
    }

    private void Click_CreateProducts(object sender, RoutedEventArgs e)  // The method creates a table with Products.
    {
        String sql = File.ReadAllText("RequestsSQL/RequestNewProductsTable.sql");
        using SqlCommand cmd = new SqlCommand(sql, _connection);

        try
        {
            cmd.ExecuteNonQuery();
            MessageBox.Show("Create products ok", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }

        cmd.Dispose();  // Releasing a resource.
    }

    private void Click_CreateManagers(object sender, RoutedEventArgs e)  // The method creates a table with Managers.
    {
        String sql = File.ReadAllText("RequestsSQL/RequestNewManagersTable.sql");
        using SqlCommand cmd = new SqlCommand(sql, _connection);

        try
        {
            cmd.ExecuteNonQuery();
            MessageBox.Show("Fill products ok", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }

        cmd.Dispose();  // Releasing a resource.
    }
    #endregion
    #region Query fill
    private void Click_FillDepartments(object sender, RoutedEventArgs e)  // The method implements filling the Department table with default values using queries.
    {
        String sql = File.ReadAllText("RequestsSQL/RequestImplementationDepartmentStandardData.sql");
        using SqlCommand cmd = new SqlCommand(sql, _connection);

        try
        {
            cmd.ExecuteNonQuery();
            MessageBox.Show("Fill department OK", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (SqlException ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }

        cmd.Dispose();  // Releasing a resource.
    }

    private void Click_FillProducts(object sender, RoutedEventArgs e)  // The method implements filling the Products table with default values using queries.
    {
        String sql = File.ReadAllText("RequestsSQL/RequestImplementationProductsStandardData.sql");
        using SqlCommand cmd = new SqlCommand(sql, _connection);

        try
        {
            cmd.ExecuteNonQuery();
            MessageBox.Show("Fill products ok", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }

        cmd.Dispose();  // Releasing a resource.
    }

    private void Click_FillManagers(object sender, RoutedEventArgs e)  // The method implements filling the Managers table with default values using queries.
    {
        String sql = File.ReadAllText("RequestsSQL/RequestImplementationManagersStandardData.sql");
        using SqlCommand cmd = new SqlCommand(sql, _connection);

        try
        {
            cmd.ExecuteNonQuery();
            MessageBox.Show("Fill products ok", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }

        cmd.Dispose();  // Releasing a resource.
    }
    #endregion
    #endregion
    #region Queries with a Single Scalar Result.
    private void ShowMonitor()  // The method displays all the data in the table.
    {
        ShowMonitorDepartments();
        ShowMonitorProducts();
        ShowMonitorManagers();
    }

    private void ShowMonitorDepartments()  // Get the number of departments for [Status-Monitor].
    {
        using SqlCommand cmd = new SqlCommand(File.ReadAllText("RequestsSQL/RequestCountDeportations.sql"), _connection);

        try
        {
            object res = cmd.ExecuteScalar();
            int cnt = Convert.ToInt32(res);

            Label_StatusDepartments.Content = cnt.ToString();
        }
        catch (SqlException ex)
        {
            MessageBox.Show(ex.Message, "SQL error", MessageBoxButton.OK, MessageBoxImage.Error);

            Label_StatusDepartments.Content = "----";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Casr error", MessageBoxButton.OK, MessageBoxImage.Error);

            Label_StatusDepartments.Content = "----";
        }
    }

    private void ShowMonitorProducts()  // Get the number of products for [Status-Monitor].
    {
        using SqlCommand cmd = new SqlCommand(File.ReadAllText("RequestsSQL/RequestCountDeportations.sql"), _connection);

        try
        {
            object res = cmd.ExecuteScalar();
            int cnt = Convert.ToInt32(res);

            Label_StatusProducts.Content = cnt.ToString();
        }
        catch (SqlException ex)
        {
            MessageBox.Show(ex.Message, "SQL error", MessageBoxButton.OK, MessageBoxImage.Error);

            Label_StatusProducts.Content = "----";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Casr error", MessageBoxButton.OK, MessageBoxImage.Error);

            Label_StatusProducts.Content = "----";
        }
    }

    private void ShowMonitorManagers()  // Get the number of managers for [Status-Monitor].
    {
        using SqlCommand cmd = new SqlCommand(File.ReadAllText("RequestsSQL/RequestCountManager.sql"), _connection);

        try
        {
            object res = cmd.ExecuteScalar();
            int cnt = Convert.ToInt32(res);

            Label_StatusManagers.Content = cnt.ToString();
        }
        catch (SqlException ex)
        {
            MessageBox.Show(ex.Message, "SQL error", MessageBoxButton.OK, MessageBoxImage.Error);

            Label_StatusManagers.Content = "----";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Casr error", MessageBoxButton.OK, MessageBoxImage.Error);

            Label_StatusManagers.Content = "----";
        }
    }
    #endregion
    #region Request from tabular results
    private void Showviews()  // The method shows the contents of the tables in the database and passes them to the text box.
    {
        ShowDepartmentsView();
        ShowProductsView();
        ShowManagersView();
    }

    private void ShowDepartmentsView()  // Drawing all departmental data.
    {
        using SqlCommand cmd = new SqlCommand(File.ReadAllText("RequestsSQL/DepartmentPullRequest.sql"), _connection);

        try
        {
            SqlDataReader reader = cmd.ExecuteReader();
            String str = String.Empty;

            while (reader.Read())
                str += $"{reader.GetString(0)}  |  {reader[1]}\n";
            reader.Close();

            TextBlock_Departments.Text = str;
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Output error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void ShowProductsView()  // Drawing all product data.
    {
        using SqlCommand cmd = new SqlCommand(File.ReadAllText("RequestsSQL/ProductPullRequest.sql"), _connection);

        try
        {
            SqlDataReader reader = cmd.ExecuteReader();
            String str = String.Empty;

            while (reader.Read())
                str += $"{reader.GetString(0)}  |  {reader[1]} \n";
            reader.Close();

            TextBlock_Products.Text = str;
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Output error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void ShowManagersView()  // Drawing all manager data.
    {
        using SqlCommand cmd = new SqlCommand(File.ReadAllText("RequestsSQL/ManagersPullRequest.sql"), _connection);

        try
        {
            SqlDataReader reader = cmd.ExecuteReader();
            String str = String.Empty;

            while (reader.Read())
                str += $"{reader.GetString(0)}  |  {reader[1]}  |  {reader[2]}\n";
            reader.Close();

            TextBlock_Managers.Text = str;
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Output error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    #endregion

    private void Window_Closed(object sender, EventArgs e)  // Closing the connection.
    {
        if (_connection?.State == System.Data.ConnectionState.Open)
            _connection.Close();
    }
}
}