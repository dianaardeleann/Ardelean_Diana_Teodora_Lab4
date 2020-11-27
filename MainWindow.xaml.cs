using AutoLotModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
//using AutoLotModel;

namespace Ardelean_Diana_Teodora_Lab4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        AutoLotEntitiesModel ctx = new AutoLotEntitiesModel();
        CollectionViewSource customerViewSource = null;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // customerViewSource.Source = [generic data source]
            //using AutoLotModel;
            this.customerViewSource =
((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            this.customerViewSource.Source = ctx.Customers.Local;
            ctx.Customers.Load();
            
        }
    
    private void btnNew_Click(object sender, RoutedEventArgs e)
    {
        action = ActionState.New;
        btnCancel.IsEnabled = true;
        btnSave.IsEnabled = true;
        btnEdit.IsEnabled = false;
        btnDelete.IsEnabled = false;
        btnPrev.IsEnabled = false;
        btnNext.IsEnabled = false;
        BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
        BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
        BindingOperations.ClearBinding(custIdTextBox, TextBox.TextProperty);
        BindingOperations.ClearBinding(contract_valueTextBox,
       TextBox.TextProperty);
        BindingOperations.ClearBinding(contract_DateDatePicker,
       DatePicker.SelectedDateProperty);
        Keyboard.Focus(contract_DateDatePicker);

    }

    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
        action = ActionState.Edit;
        btnCancel.IsEnabled = true;
        btnSave.IsEnabled = true;
        btnNew.IsEnabled = false;
        btnDelete.IsEnabled = false;
        customerDataGrid.IsEnabled = false;
        btnPrev.IsEnabled = false;
        btnNext.IsEnabled = false;
    }

    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {
        action = ActionState.Delete;
        btnCancel.IsEnabled = true;
        btnSave.IsEnabled = true;
        btnNew.IsEnabled = false;
        btnEdit.IsEnabled = false;
        customerDataGrid.IsEnabled = false;
        btnPrev.IsEnabled = false;
        btnNext.IsEnabled = false;
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
        Customer customer = null;
        if (action == ActionState.New)
        {
            try
            {
                //instantiem Customer entity
                customer = new Customer()
                {
                    FirstName = firstNameTextBox.Text.Trim(),
                    LastName = lastNameTextBox.Text.Trim(),
                    Contract_value = Decimal.Parse(contract_valueTextBox.Text.Trim()),
                    Contract_Date = contract_DateDatePicker.SelectedDate
                };
                //adaugam entitatea nou creata in context
                ctx.Customers.Add(customer);
                customerViewSource.View.Refresh();
                //salvam modificarile
                ctx.SaveChanges();
            }
            //using System.Data;
            catch (DataException ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        else
if (action == ActionState.Edit)
        {
            try
            {
                customer = (Customer)customerDataGrid.SelectedItem;
                customer.FirstName = firstNameTextBox.Text.Trim();
                customer.LastName = lastNameTextBox.Text.Trim();
                customer.Contract_value = Decimal.Parse(contract_valueTextBox.Text.Trim());
                customer.Contract_Date = contract_DateDatePicker.SelectedDate;
                
                //salvam modificarile
                ctx.SaveChanges();
            }
            catch (DataException ex)
            {
                MessageBox.Show(ex.Message);
            }
            customerViewSource.View.Refresh();
            // pozitionarea pe item-ul curent
            customerViewSource.View.MoveCurrentTo(customer);
        }
        else if (action == ActionState.Delete)
        {
            try
            {
                customer = (Customer)customerDataGrid.SelectedItem;
                ctx.Customers.Remove(customer);
                ctx.SaveChanges();
            }
            catch (DataException ex)
            {
                MessageBox.Show(ex.Message);
            }
            customerViewSource.View.Refresh();

        }
        btnNew.IsEnabled = true;
        btnEdit.IsEnabled = true;
        btnDelete.IsEnabled = true;
        btnSave.IsEnabled = false;
        btnCancel.IsEnabled = false;
        btnNext.IsEnabled = true;
        btnPrev.IsEnabled = true;
        customerDataGrid.IsEnabled = true;

    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        action = ActionState.Nothing;
        btnNew.IsEnabled = true;
        btnEdit.IsEnabled = true;
        btnDelete.IsEnabled = true;
        btnSave.IsEnabled = false;
        btnCancel.IsEnabled = false;
        btnNext.IsEnabled = true;
        btnPrev.IsEnabled = true;
        customerDataGrid.IsEnabled = true;
    }

    private void btnPrev_Click(object sender, RoutedEventArgs e)
    {
        customerViewSource.View.MoveCurrentToPrevious();
    }

    private void btnNext_Click(object sender, RoutedEventArgs e)
    {
        customerViewSource.View.MoveCurrentToNext();

    }
}

}
