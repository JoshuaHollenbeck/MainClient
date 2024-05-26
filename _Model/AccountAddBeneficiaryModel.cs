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
    public class AccountAddBeneficiaryModel
    {
        // This method creates a DataTable from a collection of MultiBeneficiaryDetail objects.
        private DataTable MultipleBeneficiaryDetailsTable(
            IEnumerable<MultiBeneficiaryDetail> multiBeneficiaryDetailsCollection
        )
        {
            // Creating a new DataTable with a specific name.
            DataTable multiBeneficiaryDetailsTable = new DataTable(
                "InsertBeneficiariesIntoAccount"
            );

            // Adding columns to the DataTable.
            multiBeneficiaryDetailsTable.Columns.Add("BeneFirstName", typeof(string));
            multiBeneficiaryDetailsTable.Columns.Add("BeneLastName", typeof(string));
            multiBeneficiaryDetailsTable.Columns.Add("BeneTaxId", typeof(string));
            multiBeneficiaryDetailsTable.Columns.Add("RelationshipType", typeof(string));
            multiBeneficiaryDetailsTable.Columns.Add("BenePortion", typeof(decimal));

            // Populating the DataTable with data from the collection of MultiBeneficiaryDetail objects.
            foreach (var multibene in multiBeneficiaryDetailsCollection)
            {
                multiBeneficiaryDetailsTable.Rows.Add(
                    multibene.MultiBeneFirstName,
                    multibene.MultiBeneLastName,
                    multibene.MultiBeneTaxId,
                    multibene.MultiRelationshipType,
                    multibene.MultiBenePortion
                );
            }
            return multiBeneficiaryDetailsTable;
        }

        // Method to insert a multiple check deposit transaction.
        public void AddBeneficiary(
            IEnumerable<MultiBeneficiaryDetail> multiBeneficiaryDetailsCollection,
            string MultiAccountNumber
        )
        {
            DataTable multiBeneficiaryDetailsTable = MultipleBeneficiaryDetailsTable(
                new ObservableCollection<MultiBeneficiaryDetail>(multiBeneficiaryDetailsCollection)
            );

            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[Acct_AddBeneficiary]";

            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command.
                    command.Parameters.AddWithValue("@AcctNum", MultiAccountNumber);
                    SqlParameter multiBeneficiaryDetailsParam = command.Parameters.AddWithValue(
                        "@BeneficiaryDetails",
                        multiBeneficiaryDetailsTable
                    );
                    multiBeneficiaryDetailsParam.SqlDbType = SqlDbType.Structured;
                    multiBeneficiaryDetailsParam.TypeName = "InsertBeneficiariesIntoAccount";

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
