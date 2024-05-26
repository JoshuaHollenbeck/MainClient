using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Utilities;

namespace MainClient._Model
{
    // Model class for representing access history.
    public class AccessHistoryModel
    {
        // Properties representing access history attributes.
        public string HistoryAccountNumber { get; set; }
        public string HistoryRepID { get; set; }
        public DateTime? HistoryDate { get; set; }

        // Method to retrieve client access history by account number.
        public static IEnumerable<AccessHistoryModel> GetClientAccessHistoryByAcctNum(string acctNum)
        {
            // List to store access history objects.
            var accessHistorys = new List<AccessHistoryModel>();

            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Client_GetClientAccessHistoryByAcctNum]";

            try
            {
                // Using statement for ensuring proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameter for account number.
                    command.Parameters.Add(new SqlParameter("@AcctNum", acctNum));

                    connection.Open();

                    // Executing the stored procedure and reading the results.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var accessHistory = new AccessHistoryModel
                            {
                                HistoryAccountNumber = reader["Acct Number"] as string,
                                HistoryRepID = reader["Rep ID"] as string,
                                HistoryDate = reader["Date"] as DateTime?,
                            };
                            accessHistorys.Add(accessHistory);
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

            return accessHistorys;
        }
    }
}
