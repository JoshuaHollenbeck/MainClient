using System.Data;
using System.Net.NetworkInformation;
using System.Threading;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Data.SqlClient;
using MainClient.Utilities;

namespace MainClient._ViewModel
{
    class SearchWindowVM : ViewModelBase
    {
        private string _searchQuery;

        public SearchWindowVM(string searchText)
        {
            _searchQuery = searchText;
            LoadTypes();
        }

        private void LoadTypes()
        {
            
            if (_searchQuery.Length == 8 || _searchQuery.Length == 12)
                {
                    string acct_num_query = "SELECT acct_num FROM acct_info WHERE acct_num LIKE @searchText";
            
                    using (SqlConnection connection = new SqlConnection(Connection.connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(acct_num_query, connection))
                        {
                            command.Parameters.AddWithValue("@searchText", "%" + _searchQuery + "%");
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    // Handle the results here.
                                }
                            }
                        }
                        connection.Close();
                    }
                }
            else if (_searchQuery.Length == 9)
                {
                    string tax_id_query = "SELECT acct_num FROM acct_info WHERE acct_num LIKE @searchText";
            
                    using (SqlConnection connection = new SqlConnection(Connection.connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(tax_id_query, connection))
                        {
                            command.Parameters.AddWithValue("@searchText", "%" + _searchQuery + "%");
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    // Handle the results here.
                                }
                            }
                        }
                        connection.Close();
                    }
                }
            else
                {
                    MessageBox.Show("Account number or tax ID not found.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                }
        }
    }
}