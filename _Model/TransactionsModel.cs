using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Utilities;

namespace MainClient._Model
{
    // This class represents the model for account transactions.
    public class TransactionsModel
    {
        // Properties to store transaction details.
        public DateTime? TransactionsDate { get; set; }
        public string TransactionsAction { get; set; }
        public string TransactionsActionLong { get; set; }
        public string TransactionsTradeSymbol { get; set; }
        public decimal? TransactionsTradeQuantity { get; set; }
        public decimal? TransactionsAmount { get; set; }
        public decimal? TransactionsTradeFees { get; set; }
        public decimal? TransactionsTradePrice { get; set; }
        public string TransactionsTradeStatus { get; set; }
        public decimal? TransactionsPreBalance { get; set; }
        public decimal? TransactionsPostBalance { get; set; }
        public string TransactionsRepID { get; set; }
        public string TransactionBankRouting { get; set; }
        public string TransactionBankAcct { get; set; }

        // Method to retrieve account transactions by account number.
        public static IEnumerable<TransactionsModel> GetAcctTransactionsByAcctNum(string acctNum)
        {
            // List to store account transaction objects.
            var acctTransactions = new List<TransactionsModel>();

            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Acct_GetAcctTransactionsByAcctNum]";

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
                            var acctTransaction = new TransactionsModel
                            {
                                TransactionsDate = reader["Transaction Date"] as DateTime?,
                                TransactionsAction = reader["Action"] as string,
                                TransactionsActionLong = reader["Action Long"] as string,
                                TransactionsAmount = reader["Amount"] as decimal?,
                                TransactionsTradeSymbol = reader["Symbol"] as string,
                                TransactionsTradeQuantity = reader["Trade Quantity"] as decimal?,
                                TransactionsTradePrice = reader["Trade Price"] as decimal?,
                                TransactionsTradeStatus = reader["Trade Status"] as string,
                                TransactionsTradeFees = reader["Trade/Wire Fees"] as decimal?,
                                TransactionBankRouting = reader["ACH/Wire Routing Num"] as string,
                                TransactionBankAcct = reader["ACH/Wire Acct Num"] as string,
                                TransactionsPreBalance = reader["Pre Balance"] as decimal?,
                                TransactionsPostBalance = reader["Post Balance"] as decimal?,
                                TransactionsRepID = reader["Rep ID"] as string,
                            };
                            acctTransactions.Add(acctTransaction);
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
            return acctTransactions;
        }

        
    }
}
