using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Utilities;

namespace MainClient.Services
{
    public class PrinterService
    {
        // Properties to store printer details.
        public string PrinterAcctNum { get; set; }
        public string PrinterAcctType { get; set; }
        public string PrinterContactName { get; set; }
        public string PrinterContactAddress { get; set; }
        public string PrinterContactAddres2 { get; set; }
        public string PrinterCity { get; set; }
        public string PrinterState { get; set; }
        public string PrinterPostalCode { get; set; }

        // Method to retrieve account transactions by account number.
        public static IEnumerable<PrinterService> GetAcctTransactionPrintInfoByAcctNum(string acctNum)
        {
            // List to store account transaction objects.
            var printerTransactions = new List<PrinterService>();

            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Acct_GetAcctTransactionPrintInfoByAcctNum]";

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
                            var printerTransaction = new PrinterService
                            {
                                PrinterAcctNum = reader["Acct Num"] as string,
                                PrinterAcctType = reader["Acct Type"] as string,
                                PrinterContactName = reader["Contact Name"] as string,
                                PrinterContactAddress = reader["Contact Address"] as string,
                                PrinterContactAddres2 = reader["Contact Address 2"] as string,
                                PrinterCity = reader["City"] as string,
                                PrinterState = reader["State"] as string,
                                PrinterPostalCode = reader["Postal Code"] as string
                            };
                            printerTransactions.Add(printerTransaction);
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
            return printerTransactions;
        }
    }
}