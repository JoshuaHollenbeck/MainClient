using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient._ViewModel;
using MainClient.Utilities;

namespace MainClient.Services
{
    // Service class for managing currency operations.
    public class CurrencyService
    {
        // Class representing currency type.
        public class CurrencyInfo
        {
            public string CurrencyAbbr { get; set; }
        }

        // Method to look up currency abbreviations.
        public static IEnumerable<CurrencyInfo> LookUpCurrencyAbbr()
        {
            // List to store currencies.
            var currencyLists = new List<CurrencyInfo>();

            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[MainClient_LookUpCurrencyAbbr]";

            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    // Executing the stored procedure and reading the results.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {   
                            var currencyList = new CurrencyInfo
                            {
                                CurrencyAbbr = reader["Currency Abbr"] as string
                            };
                            currencyLists.Add(currencyList);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log the exception, show a message, or handle it as necessary
                MessageBox.Show($"A SQL error occurred: {sqlEx.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // This catches non-SQL exceptions
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return currencyLists;
        }
    }
}
