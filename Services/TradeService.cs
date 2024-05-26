using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Utilities;


namespace MainClient.Services
{
    // Service class for managing trade operations.
    public class TradeService
    {
        // Class representing stock info.
        public class TradeInfo
        {
            public string StockName { get; set; }
            public string StockAbbr { get; set; }
            public decimal? LocalPrice { get; set; }
            public decimal? UsdPrice { get; set; }
            public decimal? StockFee { get; set; }
            public string StockExchangeName { get; set; }
            public string StockExchangeAbbr { get; set; }
            public string StockCurrency { get; set; }
            public string DisplayStockInfo => $"{StockExchangeAbbr}: {StockAbbr}";
        }

        // Method to look up stock info.
        public static IEnumerable<TradeInfo> LookUpStockInfo()
        {
            // List to store stock objects.
            var stockLists = new List<TradeInfo>();

            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[MainClient_LookUpStockInfo]";

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
                            var stockList = new TradeInfo
                            {
                                StockName = reader["Stock Name"] as string,
                                StockAbbr = reader["Stock Ticker"] as string,
                                UsdPrice = reader["USD Price"] as decimal?,
                                LocalPrice = reader["Local Price"] as decimal?,
                                StockFee = reader["Fee Amt"] as decimal?,
                                StockExchangeName = reader["Exchange Name"] as string,
                                StockExchangeAbbr = reader["Exchange Abbr"] as string,
                                StockCurrency = reader["Currency Abbr"] as string
                            };
                            stockLists.Add(stockList);
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
            return stockLists;
        }

        // Method to insert a trade buy transaction.
        public void InsertAcctTransactionTradeBuyByAcctNum(
            string BuyAccountNumber,
            string BuyRepId,
            decimal? BidPrice,
            decimal? BuyTradeQuantity,
            string BuyStockTicker
        )
        {
            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[Acct_InsertAcctTransactionTradeBuyByAcctNum]";

            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command.
                    command.Parameters.AddWithValue("@AcctNum", BuyAccountNumber);
                    command.Parameters.AddWithValue("@RepId", BuyRepId);
                    command.Parameters.AddWithValue("@BidPrice", BidPrice);
                    command.Parameters.AddWithValue("@TradeQuantity", BuyTradeQuantity);
                    command.Parameters.AddWithValue("@StockTicker", BuyStockTicker);

                    // Opening connection and executing the command.
                    connection.Open();
                    command.ExecuteNonQuery();
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
        }

        // Method to insert a trade sell transaction.
        public void InsertAcctTransactionTradeSellByAcctNum(
            string SellAccountNumber,
            string SellRepId,
            decimal? SellPrice,
            decimal? SellTradeQuantity,
            string SellStockTicker
            
        )
        {
            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[Acct_InsertAcctTransactionTradeSellByAcctNum]";

            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command.
                    command.Parameters.AddWithValue("@AcctNum", SellAccountNumber);
                    command.Parameters.AddWithValue("@RepId", SellRepId);
                    command.Parameters.AddWithValue("@SellPrice", SellPrice);
                    command.Parameters.AddWithValue("@TradeQuantity", SellTradeQuantity);
                    command.Parameters.AddWithValue("@StockTicker", SellStockTicker);

                    // Opening connection and executing the command.
                    connection.Open();
                    command.ExecuteNonQuery();
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
        }
    }
}
