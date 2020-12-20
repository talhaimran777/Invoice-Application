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
        public MainWindow()
        {
            InitializeComponent();
            // Get Invoice Data from the database

            SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\Visual Studio Apps\Shopping Cart App\Shopping Cart App\ShoppingCart.mdf;Integrated Security=True");
            SqlCommand command = new SqlCommand
               (
                   "SELECT CustomerName " +
                   "FROM Invoices", sqlCon);

            try
            {
                sqlCon.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    MessageBox.Show(reader["CustomerName"].ToString());
                }

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
}
