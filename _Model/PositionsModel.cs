using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Utilities;

namespace MainClient._Model
{
    // This class represents the model for handling account positions.
    public class PositionsModel
    {
        // Property to store the account balance.
        public decimal? AccountBalance { get; set; }

        // Method to retrieve account balance by account number.
        public static IEnumerable<PositionsModel> GetAcctBalanceByAcctNum(string acctNum)
        {
            // List to store account balances objects.
            var acctBalances = new List<PositionsModel>();

            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Acct_GetAcctBalanceByAcctNum]";

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
                            var acctBalance = new PositionsModel
                            {
                                AccountBalance = reader["Acct Balance"] as decimal?
                            };
                            acctBalances.Add(acctBalance);
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
            return acctBalances;
        }
        
        // Properties to store position details.
        public string PositionsStockTicker { get; set; }
        public string PositionsStockName { get; set; }
        public string PositionsExchange { get; set; }
        public decimal? PositionsQuantity { get; set; }
        public decimal? PositionsAverageCost { get; set; }
        public decimal? PositionsCurrentPrice { get; set; }
        public decimal? PositionsUsdPrice { get; set; }
        public decimal? PositionsCurrentValue { get; set; }
        public string PositionsCurrency { get; set; }

        // Method to retrieve account holdings by account number.
        public static IEnumerable<PositionsModel> GetAcctHoldingsByAcctNum(string acctNum)
        {
            // List to store account holding objects.
            var acctHoldings = new List<PositionsModel>();

            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Acct_GetAcctHoldingsByAcctNum]";

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
                            var acctHolding = new PositionsModel
                            {
                                PositionsStockTicker = reader["Symbol"] as string,
                                PositionsStockName = reader["Name"] as string,
                                PositionsExchange = reader["Exchange"] as string,
                                PositionsQuantity = reader["Quantity"] as decimal?,
                                PositionsAverageCost = reader["Dollar Cost Average"] as decimal?,
                                PositionsCurrentPrice = reader["Current Stock Price"] as decimal?,
                                PositionsUsdPrice = reader["USD Price"] as decimal?,
                                PositionsCurrentValue = reader["Current Value"] as decimal?,
                                PositionsCurrency = reader["Currency Abbr"] as string
                            };
                            acctHoldings.Add(acctHolding);
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
            return acctHoldings;
        }
    }
}
