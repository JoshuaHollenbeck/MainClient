using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Services;
using MainClient.Utilities;

namespace MainClient._Model
{
    public class AddAccountNewClientModel
    {
        public string AcctType { get; set; }
        public string InitialContactMethod { get; set; }
        public string AcctObjective { get; set; }
        public string AcctFunding { get; set; }
        public string AcctPurpose { get; set; }
        public string AcctActivity { get; set; }
        public bool? ContactSame { get; set; }
        public string ContactName { get; set; }
        public string ContactAddress { get; set; }
        public string ContactAddress2 { get; set; }
        public string ContactPostalCode { get; set; }
        
        // Method to insert a new account for the create client.
        public void AddAcctNewClient(
            long NewCustId,
            string RepId,
            string InitialContactMethod,
            string AcctType,
            string AcctObjective,
            string AcctFunding,
            string AcctPurpose,
            string AcctActivity,
            bool? ContactSame,
            string ContactName,
            string ContactAddress,
            string ContactAddress2,
            string ContactPostalCode
        )
        {
            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[Acct_AddAcctNewClient]";

            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command.
                    command.Parameters.AddWithValue("@CustId", NewCustId);
                    command.Parameters.AddWithValue("@RepId", RepId);
                    command.Parameters.AddWithValue("@InitialContactMethod", InitialContactMethod);
                    command.Parameters.AddWithValue("@AcctType", AcctType);
                    command.Parameters.AddWithValue("@AcctObjective", AcctObjective);
                    command.Parameters.AddWithValue("@AcctFunding", AcctFunding);
                    command.Parameters.AddWithValue("@AcctPurpose", AcctPurpose);
                    command.Parameters.AddWithValue("@AcctActivity", AcctActivity);
                    command.Parameters.AddWithValue("@ContactSame", ContactSame);
                    command.Parameters.AddWithValue("@ContactName", ContactName);
                    command.Parameters.AddWithValue("@ContactAddress", ContactAddress);
                    command.Parameters.AddWithValue("@ContactAddress2", ContactAddress2);
                    command.Parameters.AddWithValue("@ContactPostalCode", ContactPostalCode);

                    SqlParameter newAcctNum = new SqlParameter("@NewAcctNum", SqlDbType.VarChar)
                    {
                        Direction = ParameterDirection.Output,
                        Size = 14
                    };
                    command.Parameters.Add(newAcctNum);

                    // Opening connection and executing the command.
                    connection.Open();
                    command.ExecuteNonQuery();

                    string NewAcctNum = (string)newAcctNum.Value;

                    AccountNumService.Instance.SelectedAccountNumber = NewAcctNum;
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
