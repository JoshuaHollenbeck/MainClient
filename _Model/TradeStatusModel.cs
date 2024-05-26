using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Utilities;


namespace MainClient._Model
{
    // This class represents the model for account transactions.
    public class TradeStatusModel
    {
        // Properties to store transaction details.
        public DateTime? TradeDate { get; set; }
        public string TradeAction { get; set; }
        public decimal? TradeAmount { get; set; }
        public string TradeSymbol { get; set; }
        public string TradeName { get; set; }
        public decimal? TradeQuantity { get; set; }
        public decimal? TradePrice { get; set; }
        public string TradeStatus { get; set; }
        public decimal? TradeFees { get; set; }
        public string TradeCurrency { get; set; }
        public string TradeRepID { get; set; }

        // Method to retrieve account trades by account number.
        public static IEnumerable<TradeStatusModel> GetAcctTransactionsTradeByAcctNum(string acctNum)
        {
            // List to store account transaction objects.
            var acctTrades = new List<TradeStatusModel>();

            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Acct_GetAcctTransactionsTradeByAcctNum]";

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
                            var acctTrade = new TradeStatusModel
                            {
                                TradeDate = reader["Transaction Date"] as DateTime?,
                                TradeAction = reader["Action"] as string,
                                TradeAmount = reader["Amount"] as decimal?,
                                TradeSymbol = reader["Symbol"] as string,
                                TradeName = reader["Stock Name"] as string,
                                TradeQuantity = reader["Trade Quantity"] as decimal?,
                                TradePrice = reader["Trade Price"] as decimal?,
                                TradeStatus = reader["Trade Status"] as string,
                                TradeFees = reader["Trade Fees"] as decimal?,
                                TradeCurrency = reader["Trade Currency"] as string,
                                TradeRepID = reader["Rep ID"] as string,
                            };
                            acctTrades.Add(acctTrade);
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
            return acctTrades;
        }
    }
}
