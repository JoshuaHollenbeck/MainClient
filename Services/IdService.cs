using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using MainClient.Utilities;

namespace MainClient.Services
{
    // Service class for managing ID operations.
    public class IdService
    {
        // Class representing ID info.
        public class IdInfo
        {
            public string IdName { get; set; }
        }

        // Method to look up ID info
        public static IEnumerable<IdInfo> LookUpIdType()
        {
            // List to store ID objects.
            var idTypeLists = new List<IdInfo>();

            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[MainClient_LookUpIdType]";

            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    connection.Open();

                    // Executing the stored procedure and reading the results.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var idTypeList = new IdInfo
                            {
                                IdName = reader["ID Type"] as string,
                            };
                            idTypeLists.Add(idTypeList);
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
            return idTypeLists;
        }
    }
}
