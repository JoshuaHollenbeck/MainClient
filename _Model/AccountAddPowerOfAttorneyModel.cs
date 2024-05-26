using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient._ViewModel;
using MainClient.Utilities;

namespace MainClient._Model
{
    public class AccountAddPowerOfAttorneyModel
    {
        // This method creates a DataTable from a collection of MultiPowerOfAttorneyDetail objects.
        private DataTable MultiplePowerOfAttorneyDetailsTable(
            IEnumerable<MultiPowerOfAttorneyDetail> multiPowerOfAttorneyDetailsCollection
        )
        {
            // Creating a new DataTable with a specific name.
            DataTable multiPowerOfAttorneyDetailsTable = new DataTable(
                "InsertPowersOfAttorneyIntoAccount"
            );

            // Adding columns to the DataTable.
            multiPowerOfAttorneyDetailsTable.Columns.Add("PoaFirstName", typeof(string));
            multiPowerOfAttorneyDetailsTable.Columns.Add("PoaLastName", typeof(string));
            multiPowerOfAttorneyDetailsTable.Columns.Add("PoaTaxId", typeof(string));
            multiPowerOfAttorneyDetailsTable.Columns.Add("PoaRole", typeof(string));

            // Populating the DataTable with data from the collection of MultiPowerOfAttorneyDetail objects.
            foreach (var multipoa in multiPowerOfAttorneyDetailsCollection)
            {
                multiPowerOfAttorneyDetailsTable.Rows.Add(
                    multipoa.MultiPoaFirstName,
                    multipoa.MultiPoaLastName,
                    multipoa.MultiPoaTaxId,
                    multipoa.MultiPoaRole
                );
            }
            return multiPowerOfAttorneyDetailsTable;
        }

        // Method to insert a multiple check deposit transaction.
        public void AddPowerOfAttorney(
            IEnumerable<MultiPowerOfAttorneyDetail> multiPowerOfAttorneyDetailsCollection,
            string MultiAccountNumber
        )
        {
            DataTable multiPowerOfAttorneyDetailsTable = MultiplePowerOfAttorneyDetailsTable(
                new ObservableCollection<MultiPowerOfAttorneyDetail>(multiPowerOfAttorneyDetailsCollection)
            );

            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[Acct_AddPowerOfAttorney]";

            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command.
                    command.Parameters.AddWithValue("@AcctNum", MultiAccountNumber);
                    SqlParameter multiPowerOfAttorneyDetailsParam = command.Parameters.AddWithValue(
                        "@PowerOfAttorneyDetails",
                        multiPowerOfAttorneyDetailsTable
                    );
                    multiPowerOfAttorneyDetailsParam.SqlDbType = SqlDbType.Structured;
                    multiPowerOfAttorneyDetailsParam.TypeName = "InsertPowersOfAttorneyIntoAccount";

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
