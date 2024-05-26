using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Utilities;


namespace MainClient._Model
{
    // This class represents the model for account notes.
    public class ViewNotesModel
    {
        // Properties to store note details.
        public string NoteAccountNumber { get; set; }
        public string Note { get; set; }
        public DateTime? NoteCreated { get; set; }
        public string NoteRepID { get; set; }

        // Method to retrieve account notes by account number.
        public static IEnumerable<ViewNotesModel> GetAcctNotesByAcctNum(string acctNum)
        {
            // List to store account note objects.
            var acctNotes = new List<ViewNotesModel>();

            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Acct_GetAcctNotesByAcctNum]";

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
                            var acctNote = new ViewNotesModel
                            {
                                NoteAccountNumber = reader["Acct Num"] as string,
                                Note = reader["Note"] as string,
                                NoteCreated = reader["Date"] as DateTime?,
                                NoteRepID = reader["Rep ID"] as string,
                            };
                            acctNotes.Add(acctNote);
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
            return acctNotes;
        }
    }
}
