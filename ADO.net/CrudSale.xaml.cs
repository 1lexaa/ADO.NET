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
    public partial class CrudSale : Window
    {
        public Entity.Sale? EditSale;

        public CrudSale(Entity.Sale edit_sale)
        {
            InitializeComponent();
            EditSale = edit_sale;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = Owner;

            if (EditSale is null)
            {
                EditSale = new Entity.Sale();
                Button_Cancel.IsEnabled = false;
            }
            else
            {
                if (Owner is OrmWindow owner)
                {
                    ComboBox_Product.SelectedItem = owner.Product.FirstOrDefault(p => p.Id == EditSale.IdProduct);
                    ComboBox_Manager.SelectedItem = owner.Manager.FirstOrDefault(m => m.Id == EditSale.IdManager);
                }
                else
                {
                    MessageBox.Show("Owner is not OrmWindow");
                    Close();
                }
            }
            TextBox_Id.Text = EditSale.Id.ToString();
            TextBox_SaleDt.Text = EditSale.SaleDt.ToString();
            TextBox_Count.Text = EditSale.Cnt.ToString();
        }

        private void Click_Button_Save(object sender, RoutedEventArgs e)
        {
            if (EditSale is null)
                return;
            if (TextBox_Count.Text.Equals(String.Empty))
            {
                MessageBox.Show("Not count is null");
                TextBox_Count.Focus();
                return;
            }

            int cnt;
            try { cnt = Convert.ToInt32(TextBox_Count.Text); }
            catch (Exception)
            {
                MessageBox.Show("Count Error");
                TextBox_Count.Focus();
                return;
            }

            if (ComboBox_Product.SelectedItem is null)
            {
                MessageBox.Show("Choos product");
                ComboBox_Product.Focus();
                return;
            }
            if (ComboBox_Manager.SelectedItem is null)
            {
                MessageBox.Show("Choos Manager");
                ComboBox_Manager.Focus();
                return;
            }

            EditSale.Cnt = cnt;
            if (ComboBox_Product.SelectedItem is Entity.Product product)
                this.EditSale.IdProduct = product.Id;
            if (ComboBox_Manager.SelectedItem is Entity.Manager manager)
                this.EditSale.IdManager = manager.Id;
            this.DialogResult = true;
        }

        private void Click_Button_Delete(object sender, RoutedEventArgs e)
        {
            EditSale = null!;
            this.DialogResult = true;
        }

        private void Click_Button_Cancel(object sender, RoutedEventArgs e) => this.DialogResult = false;
    }
}