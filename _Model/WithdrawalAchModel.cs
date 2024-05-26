using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Utilities;

namespace MainClient._Model
{
    // This class represents the model for handling ACH payments.
    public class WithdrawalAchModel
    {
        // Properties to store ACH list details.
        public string PaymentNickname { get; set; }
        public string PaymentBankName { get; set; }
        public string PaymentRouting { get; set; }
        public string PaymentAcctNum { get; set; }
        public string DisplayName => $"{PaymentNickname} - {PaymentAcctNum}";

        // Method to retrieve ACH list associated with an account.
        public static IEnumerable<WithdrawalAchModel> GetAcctAchListByAcctNum(string acctNum)
        {
            // List to store ACH list objects.
            var achPaymentLists = new List<WithdrawalAchModel>();

            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Acct_GetAcctAchListByAcctNum]";

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
                            var achPaymentList = new WithdrawalAchModel
                            {
                                PaymentNickname = reader["Nickname"] as string,
                                PaymentBankName = reader["Bank Name"] as string,
                                PaymentRouting = reader["Bank Routing"] as string,
                                PaymentAcctNum = reader["Bank Acct Num"] as string,
                            };
                            achPaymentLists.Add(achPaymentList);
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
            return achPaymentLists;
        }
    }
}