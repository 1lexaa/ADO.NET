using ADO.net.EFCore;
using ADO.net.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
namespace ADO.net
{
    /// <summary>
    /// Логика взаимодействия для EFWindow.xaml
    /// </summary>
    public partial class EFWindow : Window
    {
        private EFContext _ef_context;
        private ICollectionView _I_departments_list_view;
        private static readonly Random _random = new Random();

        public EFWindow()
        {
            InitializeComponent();

            _ef_context = new EFContext();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ef_context.Departments.Load();

            DepartmentsList.ItemsSource = _ef_context.Departments.Local.ToObservableCollection();
            _I_departments_list_view = CollectionViewSource.GetDefaultView(DepartmentsList.ItemsSource);
            _I_departments_list_view.Filter = obj => (obj as EFCore.Department)?.DeleteDt == null;

            UpdateMonitor();
            UpdateDailyStatistics();
        }

        private void UpdateMonitor()
        {
            TextBlock_MonitorBlock.Text = $"Departments: {_ef_context.Departments.Count()}";
            TextBlock_MonitorBlock.Text += $"\nProducts: {_ef_context.Products.Count()}";
            TextBlock_MonitorBlock.Text += $"\nManagers: {_ef_context.Managers.Count()}";
            TextBlock_MonitorBlock.Text += $"\nSales: {_ef_context.Sales.Count()}";
        }

        private void DepartmentsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is EFCore.Department departments)
                {
                    CrudDepartment dialog = new CrudDepartment(new Entity.Department() { Id = departments.Id, Name = departments.Name });

                    if (dialog.ShowDialog() == true)
                    {
                        if (dialog.EditedDepartment is null)  // If delete.
                        {
                            departments.DeleteDt = DateTime.Now;
                            _ef_context.SaveChanges();
                            MessageBox.Show($"Deleting {departments.Name}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                        }
                        else  // If save.
                        {
                            _ef_context.Departments.Remove(departments);
                            _ef_context.SaveChanges();
                            _ef_context.Departments.Add(new EFCore.Department() { Id = dialog.EditedDepartment.Id, Name = dialog.EditedDepartment.Name });
                            MessageBox.Show($"Update {departments.Name}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
        }

        private void Click_Button_AddDepartment(object sender, RoutedEventArgs e)
        {
            CrudDepartment dialog = new CrudDepartment(null!);

            if (dialog.ShowDialog() == true)
            {
                _ef_context.Departments.Add(new EFCore.Department() { Id = dialog.EditedDepartment.Id, Name = dialog.EditedDepartment.Name });
                _ef_context.SaveChanges();

                TextBlock_MonitorBlock.Text = $"Departments: {_ef_context.Departments.Count()}";
            }
        }

        private bool HideDeletedDepartmentsFilter(object item)
        {
            if (item is EFCore.Department department)
                return department.DeleteDt is null;
            return false;
        }

        private void Checkbox_showDeletedDepartment_Checked(object sender, RoutedEventArgs e)
        {
            _I_departments_list_view.Filter = null;
            ((GridView)DepartmentsList.View).Columns[2].Width = Double.NaN;
        }

        private void Checkbox_showDeletedDepartment_Unchecked(object sender, RoutedEventArgs e)
        {
            _I_departments_list_view.Filter = HideDeletedDepartmentsFilter;
            ((GridView)DepartmentsList.View).Columns[2].Width = 0;
        }

        private void UpdateDailyStatistics()  // Sales statistics for today. 
        {
            DateTime now_date = new DateTime(2023, 3, 14);  // DateTime.Now.
            var day_sale = _ef_context.Sales.Where(s => s.SaleDt.Date == now_date.Date).ToList();

            if (day_sale.Count() != 0)
            {
                Lable_SalesCheck.Content = day_sale.Count().ToString();  // Total sales out.
                Lable_SalesCnt.Content = day_sale.Sum(s => s.Cnt);  // Total number of goods sold.
                Lable_StartMoment.Content = day_sale.Min(s => s.SaleDt.ToLongTimeString());  // Actual sales start time today.
                Lable_FinishMoment.Content = day_sale.Max(s => s.SaleDt.ToLongTimeString());  // Time of last sale.
                Lable_MaxCheckCnt.Content = day_sale.Max(s => s.Cnt);  // Maximum number of products in one check (for today).
                Lable_AvgCheckCnt.Content = Math.Round(day_sale.Average(s => s.Cnt), 2);  // "Average check" by quantity - the average value of the quantity.

                //
                //-----------------------------------------------------------------------------------------------------------------

                var query = _ef_context.Products.GroupJoin(_ef_context.Sales, p => p.Id, s => s.IdProduct, (p, sales) =>
                    new { Name = p.Name, Cnt = sales.Where(s => s.SaleDt.Date == now_date).Count(), Price = p.Price });
                //var query = _ef_context.Sales.Where(s => s.SaleDt.Date == now_date).GroupBy(s => s.IdProduct).ToList().Join(_ef_context.Products, 
                //    grp => grp.Key, p => p.Id, (grp, p) => new { Name = p.Name, Cnt = grp.Count()});

                Lable_BestProduct.Content = query.OrderByDescending(item => item.Cnt).First().Name;

                var bestManager = _ef_context.Managers.GroupJoin(_ef_context.Sales.Where(s => s.SaleDt.Date == now_date), m => m.Id, s => s.IdManager,
                            (m, ss) => new { Manager = m, Count = ss.Count() }).OrderByDescending(m => m.Count).First();

                Lable_BestProductCnt.Content = query.OrderByDescending(s => s.Cnt).First().Name;

                var best_sum = query.OrderByDescending(p => p.Cnt * p.Price).First();
                Label_BestProductSum.Content = best_sum.Name;
                Label_BestManager.Content = $"{bestManager.Manager.Surname}  {bestManager.Manager.Secname}  {bestManager.Manager.Surname}";

                foreach (var item in query)
                    TextBlock_LogBlock.Text += $"{item.Name}:  {item.Cnt}x\n";
            }
        }

        private void Button_AddSales_Click(object sender, RoutedEventArgs e)
        {
            int man_cnt = _ef_context.Managers.Count();
            int pro_cnt = _ef_context.Products.Count();
            double max_price = _ef_context.Products.Max(p => p.Price);

            for (int i = 0; i < 100; i++)
            {
                int rand_index = _random.Next(man_cnt);
                EFCore.Manager manager = _ef_context.Managers.Skip(rand_index).First();

                rand_index = _random.Next(pro_cnt);

                EFCore.Product product = _ef_context.Products.Skip(rand_index).First();
                DateTime moment = DateTime.Today.AddHours(8).AddSeconds(_random.Next(43200)).AddDays(-_random.Next(2));
                int cnt_limit = (int)(100 * (1 - product.Price / max_price) + 2);
                int cnt = _random.Next(1, cnt_limit);
                DateTime? delete_dt = _random.Next(50) == 0 ? moment.AddHours(_random.Next(1, 48)) : null;

                _ef_context.Sales.Add(new EFCore.Sale()
                {
                    Id = Guid.NewGuid(),
                    IdManager = manager.Id,
                    IdProduct = product.Id,
                    SaleDt = moment,
                    Cnt = cnt,
                    DeleteDt = delete_dt
                });
            }

            _ef_context.SaveChanges();
            UpdateMonitor();
            UpdateDailyStatistics();
        }

        private EFCore.Manager RandManager(int rand)
        {
            EFCore.Manager manager = _ef_context.Managers.Skip(rand).First();
            //MessageBox.Show($"{manager.Surname} {manager.Name} {manager.Secname}");

            return manager;
        }

        private EFCore.Product RandProduct(int rand)
        {
            EFCore.Product product = _ef_context.Products.Skip(rand).First();
            //MessageBox.Show($"{product.Name}  {product.Price}$");

            return product;
        }
    }
}
