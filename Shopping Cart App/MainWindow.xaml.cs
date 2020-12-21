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
using System.Data.SqlClient;

namespace Shopping_Cart_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Declaring an invoices list to hold all the invoices
        public List<Invoice> invoices = new List<Invoice>();

        // Declaring an invoice items list to hold all the invoices
        public List<InvoiceItem> invoiceItems = new List<InvoiceItem>();


        public void onSelection(object sender, RoutedEventArgs e) {
            if (InvoiceList.SelectedItem != null) {
                InvoiceIDBox.Text = (InvoiceList.SelectedItem as Invoice).InvoiceID.ToString();
                InvoiceDateBox.Text = (InvoiceList.SelectedItem as Invoice).InvoiceDate.ToString();
                CustomerNameBox.Text = (InvoiceList.SelectedItem as Invoice).CustomerName.ToString();
                CustomerEmailBox.Text = (InvoiceList.SelectedItem as Invoice).CustomerEmail.ToString();
                CustomerAddressBox.Text = (InvoiceList.SelectedItem as Invoice).CustomerAddress.ToString();

           

                if ((InvoiceList.SelectedItem as Invoice).Shipped.ToString() == "True")
                {
                    McCheckBox.IsChecked = true;
                }
                else {
                    McCheckBox.IsChecked = false;
                }


                // Now getting invoice items from the invoice table 
                // based on the InvoiceID in invoice items
                SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\Visual Studio Apps\Shopping Cart App\Shopping Cart App\ShoppingCart.mdf;Integrated Security=True");
                SqlCommand command = new SqlCommand
                   (
                       "SELECT ItemID, InvoiceID, ItemName, ItemDescription, ItemPrice, ItemQuantity " +
                       "FROM InvoiceItems where InvoiceID='" + InvoiceIDBox.Text+"'", sqlCon);


                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        //MessageBox.Show(reader["CustomerName"].ToString() + reader["CustomerEmail"].ToString());
                        //MessageBox.Show(reader["ItemID"].ToString() + reader["ItemDescription"].ToString());
                        
                        invoiceItems.Add(new InvoiceItem
                        {
                            ItemID = reader["ItemID"].ToString(),
                            InvoiceID = reader["InvoiceID"].ToString(),
                            ItemName = reader["ItemName"].ToString(),
                            ItemDescription = reader["ItemDescription"].ToString(),
                            ItemPrice = reader["ItemPrice"].ToString(),
                            ItemQuantity = reader["ItemQuantity"].ToString(),
                        });
                    }

                    ItemsList.ItemsSource = invoiceItems;
                    reader.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlCon.Close();
                }




            }
                
                
        }

        private void InsertDataToDB(object sender, RoutedEventArgs e)
        {
            bool validated = true;

            if (!ValidateInput(validated))
            {
                MessageBox.Show("Cannot leave any of the fields empty!");
            }
            else {
                // Get data from the text boxes and write a query 
                // to insert the data in the database.
                SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\Visual Studio Apps\Shopping Cart App\Shopping Cart App\ShoppingCart.mdf;Integrated Security=True");
                string shipped;
                if (McCheckBox.IsChecked == true) {
                    shipped = "True";
                }
                else {
                    shipped = "False";
                }

                string query = "insert into Invoices (InvoiceID, InvoiceDate, Shipped, CustomerName, CustomerAddress, CustomerEmail) values ('"+InvoiceIDBox.Text+"','"+InvoiceDateBox.Text+ "','" + shipped + "','" + CustomerNameBox.Text + "','" + CustomerAddressBox.Text + "','" + CustomerEmailBox.Text + "')";
                SqlCommand command = new SqlCommand(query, sqlCon);


                try {
                    sqlCon.Open();
                    command.ExecuteNonQuery();
                    UpdateListOfInvoices();
                }
                catch (SqlException ex) {
                    MessageBox.Show(ex.Message);

                }
                finally {
                    sqlCon.Close();
                }

            };
        }

        public bool ValidateInput(bool validated)
        {
            if (InvoiceIDBox.Text == "")
            {
                validated = false;
            }

            if (CustomerNameBox.Text == "") {
                validated = false;
            }

            if (CustomerEmailBox.Text == "")
            {
                validated = false;
            }

            if (CustomerAddressBox.Text == "")
            {
                validated = false;
            }

            if (InvoiceDateBox.Text == "")
            {
                validated = false;
            }

            return validated;

        }

        public void UpdateListOfInvoices() {

            // Clearing the list of invoices
            invoices.Clear();

            SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\Visual Studio Apps\Shopping Cart App\Shopping Cart App\ShoppingCart.mdf;Integrated Security=True");
            SqlCommand command = new SqlCommand
               (
                   "SELECT InvoiceID, InvoiceDate, Shipped, CustomerName, CustomerAddress, CustomerEmail " +
                   "FROM Invoices", sqlCon);


            try
            {
                sqlCon.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    //MessageBox.Show(reader["CustomerName"].ToString() + reader["CustomerEmail"].ToString());
                    invoices.Add(new Invoice
                    {
                        InvoiceID = reader["InvoiceID"].ToString(),
                        InvoiceDate = reader["InvoiceDate"].ToString(),
                        Shipped = reader["Shipped"].ToString(),
                        CustomerName = reader["CustomerName"].ToString(),
                        CustomerAddress = reader["CustomerAddress"].ToString(),
                        CustomerEmail = reader["CustomerEmail"].ToString(),
                    });
                }

                List<string> myList = new List<string>() { };
                InvoiceList.ItemsSource = myList;
                InvoiceList.ItemsSource = invoices;

                reader.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            // Get Invoice Data from the database
            SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\Visual Studio Apps\Shopping Cart App\Shopping Cart App\ShoppingCart.mdf;Integrated Security=True");
            SqlCommand command = new SqlCommand
               (
                   "SELECT InvoiceID, InvoiceDate, Shipped, CustomerName, CustomerAddress, CustomerEmail " +
                   "FROM Invoices", sqlCon);


            try
            {
                sqlCon.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    //MessageBox.Show(reader["CustomerName"].ToString() + reader["CustomerEmail"].ToString());
                    invoices.Add(new Invoice
                    {
                        InvoiceID = reader["InvoiceID"].ToString(),
                        InvoiceDate = reader["InvoiceDate"].ToString(),
                        Shipped = reader["Shipped"].ToString(),
                        CustomerName = reader["CustomerName"].ToString(),
                        CustomerAddress = reader["CustomerAddress"].ToString(),
                        CustomerEmail = reader["CustomerEmail"].ToString(),
                    });
                }

                InvoiceList.ItemsSource = invoices;
                reader.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }

            //SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\Visual Studio Apps\Shopping Cart App\Shopping Cart App\ShoppingCart.mdf;Integrated Security=True");
            //SqlCommand command = new SqlCommand
            //   (
            //       "SELECT InvoiceID, InvoiceDate, Shipped, CustomerName, CustomerAddress, CustomerEmail " +
            //       "FROM Invoices", sqlCon);

            //try
            //{
            //    sqlCon.Open();
            //    SqlDataReader reader = command.ExecuteReader();

            //    while (reader.Read())
            //    {
            //        //MessageBox.Show(reader["CustomerName"].ToString() + reader["CustomerEmail"].ToString());
            //        invoices.Add(new Invoice {
            //            InvoiceID = reader["InvoiceID"].ToString(),
            //            InvoiceDate = reader["InvoiceDate"].ToString(),
            //            Shipped = reader["Shipped"].ToString(),
            //            CustomerName = reader["CustomerName"].ToString(),
            //            CustomerAddress = reader["CustomerAddress"].ToString(),
            //            CustomerEmail = reader["CustomerEmail"].ToString(),
            //        });
            //    }

            //    InvoiceList.ItemsSource = invoices;
            //    reader.Close();
            //}
            //catch (SqlException ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //finally
            //{
            //    sqlCon.Close();
            //}
        }

        private void DeleteInvoiceRecord(object sender, RoutedEventArgs e)
        {
            // Delete a row of invoice table

            if (InvoiceIDBox.Text == "")
            {
                MessageBox.Show("Select an invoice from the list in order to delete!");
            }
            else {
                SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\Visual Studio Apps\Shopping Cart App\Shopping Cart App\ShoppingCart.mdf;Integrated Security=True");
                string query = "delete from invoices where InvoiceID='"+ InvoiceIDBox.Text+"'";
                SqlCommand command = new SqlCommand(query, sqlCon);


                try
                {
                    sqlCon.Open();
                    command.ExecuteNonQuery();
                    UpdateListOfInvoices();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally {
                    sqlCon.Close();
                }
            }
            


        }
    }

    public class Invoice {
        public string InvoiceID { get; set;}
        public string InvoiceDate { get; set; }
        public string Shipped { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }

        public string CustomerAddress { get; set; }
    }


    public class InvoiceItem
    {
        public string ItemID { get; set; }
        public string InvoiceID { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string ItemPrice { get; set; }
        public string ItemQuantity { get; set; }
        public string Price { get; set; }
    }
}
