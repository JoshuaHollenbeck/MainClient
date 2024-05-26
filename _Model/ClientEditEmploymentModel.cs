using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Utilities;

namespace MainClient._Model
{
    public class ClientEditEmploymentModel
    {
        public string NewEmploymentStatus { get; set; }
        public string NewEmployerName { get; set; }
        public string NewOccupation { get; set; }

        // Method to edit client employment.
        public void EditClientEmployment(
            string accountNumber,
            string NewEmploymentStatus,
            string NewEmployerName,
            string NewOccupation
        )
        {
            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[Client_EditClientEmployment]";

            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command.
                    command.Parameters.AddWithValue("@AcctNum", accountNumber);
                    command.Parameters.Add("@EmploymentStatus", SqlDbType.VarChar).Value =
                        string.IsNullOrEmpty(NewEmploymentStatus)
                            ? (object)DBNull.Value
                            : NewEmploymentStatus;

                    command.Parameters.Add("@EmployerName", SqlDbType.VarChar).Value =
                        NewEmploymentStatus == "Unemployed" || string.IsNullOrEmpty(NewEmployerName)
                            ? (object)DBNull.Value
                            : NewEmployerName;

                    command.Parameters.Add("@Occupation", SqlDbType.VarChar).Value =
                        NewEmploymentStatus == "Unemployed" || string.IsNullOrEmpty(NewOccupation)
                            ? (object)DBNull.Value
                            : NewOccupation;

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

        // Properties for account overview information.
        public string EmploymentStatus { get; set; }
        public string EmployerName { get; set; }
        public string Occupation { get; set; }

        // Method to retrieve account contact by account number.
        public static IEnumerable<ClientEditEmploymentModel> GetClientEmploymentByAcctNum(
            string acctNum
        )
        {
            // List to store accounts objects.
            var clientEmployments = new List<ClientEditEmploymentModel>();

            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Client_GetClientEmploymentByAcctNum]";

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
                            var clientEmployment = new ClientEditEmploymentModel
                            {
                                EmploymentStatus = reader["Employment Status"] as string,
                                EmployerName = reader["Employer Name"] as string,
                                Occupation = reader["Occupation"] as string
                            };
                            clientEmployments.Add(clientEmployment);
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

            return clientEmployments;
        }
    }
}
