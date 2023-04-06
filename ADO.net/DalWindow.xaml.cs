using ADO.net.DAL;
using ADO.net.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для DalWindow.xaml
    /// </summary>
    public partial class DalWindow : Window
    {
        private SqlConnection _connection;
        private readonly DataContext _context;
        public ObservableCollection<Entity.Department> DepartmentsList { get; set; }
        public ObservableCollection<Entity.Manager> ManagersList { get; set; }


        public DalWindow()
        {
            InitializeComponent();

            _context = new DataContext();
            DepartmentsList = new ObservableCollection<Entity.Department>(_context.Departments.GetAll());
            ManagersList = new ObservableCollection<Entity.Manager>(_context.Managers.GetAll());
            _connection = new SqlConnection(App._connection_string);
            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) => _connection.Open();

        private void MouseDoubleClick_ListView_Departments(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Entity.Department department)
                {
                    CrudDepartment dialog = new CrudDepartment(department);

                    if (dialog.ShowDialog() == true)
                    {
                        if (dialog.EditedDepartment is null)
                        {
                            DepartmentsList.Remove(department);
                            MessageBox.Show("Delete: " + department.Name);
                        }
                        else
                        {
                            int index = DepartmentsList.IndexOf(department);

                            DepartmentsList.Remove(department);
                            DepartmentsList.Insert(index, department);
                            MessageBox.Show("Update: " + department.Name);
                        }
                    }
                }
            }
        }

        private void MouseDoubleClick_ListView_Manager(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_AddDepartment_Click(object sender, RoutedEventArgs e)
        {
            CrudDepartment dialog = new CrudDepartment(null!);

            if (dialog.ShowDialog() == true)
            {
                if (dialog.EditedDepartment is not null)
                {
                    using SqlCommand cmd = new SqlCommand($"INSERT INTO Departments(Id, Name) VALUES (@id, @name)", _connection);

                    cmd.Parameters.AddWithValue("@id", dialog.EditedDepartment.Id);
                    cmd.Parameters.AddWithValue("@name", dialog.EditedDepartment.Name);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        DepartmentsList.Add(dialog.EditedDepartment);
                        MessageBox.Show("Add new depatments", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); }
                }
            }
        }

        private void Button_AddManager_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Entity.Manager manager)
                    MessageBox.Show(manager.Name);
            }
        }

        private void Window_Closed(object sender, EventArgs e) => _connection.Close();
    }
}
