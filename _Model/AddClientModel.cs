using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Services;
using MainClient.Utilities;


namespace MainClient._Model
{
    public class AddClientModel
    {
        // Method to insert a new client.
        public void AddClient(
            string CustTaxId,
            string FirstName,
            string MiddleName,
            string LastName,
            string Suffix,
            DateTime? DateOfBirth,
            string CustEmail,
            string CustPhoneHome,
            string CustPhoneBusiness,
            string CustAddress,
            string CustAddress2,
            string CustPostal,
            string EmploymentStatus,
            string EmployerName,
            string Occupation,
            string IdType,
            string IdStateName,
            string IdNum,
            string IdExp,
            string MothersMaiden,
            string RepId
        )
        {
            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[Client_AddClient]";
            
            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command.
                    command.Parameters.AddWithValue("@CustTaxId", CustTaxId);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@MiddleName", MiddleName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@Suffix", Suffix);
                    command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    command.Parameters.AddWithValue("@CustEmail", CustEmail);
                    command.Parameters.AddWithValue("@CustPhoneHome", CustPhoneHome);
                    command.Parameters.AddWithValue("@CustPhoneBusiness", CustPhoneBusiness);
                    command.Parameters.AddWithValue("@CustAddress", CustAddress);
                    command.Parameters.AddWithValue("@CustAddress2", CustAddress2);
                    command.Parameters.AddWithValue("@CustPostal", CustPostal);
                    command.Parameters.AddWithValue("@EmploymentStatus", EmploymentStatus);
                    command.Parameters.AddWithValue("@EmployerName", EmployerName);
                    command.Parameters.AddWithValue("@Occupation", Occupation);
                    command.Parameters.AddWithValue("@IdType", IdType);
                    command.Parameters.AddWithValue("@IdStateName", IdStateName);
                    command.Parameters.AddWithValue("@IdNum", IdNum);
                    command.Parameters.AddWithValue("@IdExp", IdExp);
                    command.Parameters.AddWithValue("@MothersMaiden", MothersMaiden);
                    command.Parameters.AddWithValue("@RepId", RepId);

                    SqlParameter custId = new SqlParameter("@CustId", SqlDbType.BigInt)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(custId);

                    // Opening connection and executing the command.
                    connection.Open();
                    command.ExecuteNonQuery();

                    long NewCustId = (long)custId.Value;

                    ClientIdService.Instance.SelectedNewCustId = NewCustId;
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
