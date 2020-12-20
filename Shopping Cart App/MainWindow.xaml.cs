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
                    invoices.Add(new Invoice {
                        InvoiceID = reader["InvoiceID"].ToString(),
                        InvoiceDate = reader["InvoiceDate"].ToString(),
                        Shipped = reader["Shipped"].ToString(),
                        CustomerName = reader["CustomerName"].ToString(),
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
        }
    }

    public class Invoice {
        public string InvoiceID { get; set;}
        public string InvoiceDate { get; set; }
        public string Shipped { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
    }
}
