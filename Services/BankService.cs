using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Utilities;

namespace MainClient.Services
{
    // Service class for outisde bank operations.
    public class BankService
    {
        // Class representing bank routing information.
        public class BankRoutingInfo
        {
            public string BankName { get; set; }
            public string BankRoutingNumber { get; set; }
        }

        // Method to look up bank routing information.
        public static IEnumerable<BankRoutingInfo> LookUpBankRouting()
        {
            // List to store bank routing information.
            var bankLists = new List<BankRoutingInfo>();

            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[MainClient_LookUpBankRouting]";

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
                            var bankList = new BankRoutingInfo
                            {
                                BankName = reader["Bank Name"] as string,
                                BankRoutingNumber = reader["Bank Routing Num"] as string,
                            };
                            bankLists.Add(bankList);
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
            return bankLists;
        }

        // Class representing bank account type.
        public class BankType
        {
            public string BankAccountType { get; set; }
        }

        // Method to look up bank account types.
        public static IEnumerable<BankType> LookUpBankAcctType()
        {
            // List to store bank account types.
            var bankTypeLists = new List<BankType>();

            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[MainClient_LookUpBankAcctType]";

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
                            var bankTypeList = new BankType
                            {
                                BankAccountType = reader["Bank Type"] as string
                            };
                            bankTypeLists.Add(bankTypeList);
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
            return bankTypeLists;
        }
    }
}
