using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Utilities;

namespace MainClient._Model
{
    // This class represents the model for adding notes to an account.
    public class AddNotesModel
    {
        // Method to insert account notes by account number.
        public static void InsertAcctNotesByAcctNum(string acctNum, string noteText, string repId)
        {
            // Retrieving connection string from the utility class.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Acct_InsertAcctNotesByAcctNum]";

            try
            {
                // Using statement for ensuring proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameter for account number.
                    command.Parameters.Add(new SqlParameter("@AcctNum", acctNum));
                    command.Parameters.Add(new SqlParameter("@Note", noteText));
                    command.Parameters.Add(new SqlParameter("@RepId", repId));

                    connection.Open();
                    
                    // Executing the non-query command to insert the notes into the database.
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
