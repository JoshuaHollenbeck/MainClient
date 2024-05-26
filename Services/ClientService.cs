using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Utilities;

namespace MainClient.Services
{
    public class ClientService
    {
        public class JournalAcctInfo
        {
            public string AcctNum { get; set; }
        }

        // Method to look up state info.
        public static IEnumerable<JournalAcctInfo> GetClientAcctJournalListByAcctNum(string accountNumber)
        {
            // List to store state objects.
            var acctJournalLists = new List<JournalAcctInfo>();

            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[Client_GetClientAcctJournalListByAcctNum]";

            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@AcctNum", SqlDbType.NVarChar) { Value = accountNumber });

                    connection.Open();

                    // Executing the stored procedure and reading the results.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var acctJournalList = new JournalAcctInfo
                            {
                                AcctNum = reader["Accts"] as string
                            };
                            acctJournalLists.Add(acctJournalList);
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
            return acctJournalLists;
        }
    }
}