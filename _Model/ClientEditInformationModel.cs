using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Utilities;


namespace MainClient._Model
{
    public class ClientEditInformationModel
    { 
        public string NewFirstName { get; set; }
        public string NewMiddleName { get; set; }
        public string NewLastName { get; set; }
        public string NewSuffix { get; set; }
        public DateTime? NewDateOfBirth { get; set; }
        public bool? NewVoiceAuth { get; set; }
        public bool? NewDoNotCall { get; set; }
        public bool? NewShareAffiliates { get; set; }
        public string NewIdType { get; set; }
        public string NewIdStateName { get; set; }
        public string NewIdNum { get; set; }
        public string NewIdExp { get; set; }
        public string NewMothersMaiden { get; set; }

        // Method to edit client information.
        public void EditClientInfo(
            string accountNumber,
            string NewFirstName,
            string NewMiddleName,
            string NewLastName,
            string NewSuffix,
            DateTime? NewDateOfBirth,
            bool? NewVoiceAuth,
            bool? NewDoNotCall,
            bool? NewShareAffiliates,
            string NewIdType,
            string NewIdStateName,
            string NewIdNum,
            string NewIdExp,
            string NewMothersMaiden
        )
        {
            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[Client_EditClientInfo]";

            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command.
                    command.Parameters.AddWithValue("@AcctNum", accountNumber);
                    command.Parameters.AddWithValue("@FirstName", NewFirstName);
                    command.Parameters.AddWithValue("@MiddleName", NewMiddleName);
                    command.Parameters.AddWithValue("@LastName", NewLastName);
                    command.Parameters.AddWithValue("@Suffix", NewSuffix);
                    command.Parameters.AddWithValue("@DateOfBirth", NewDateOfBirth);
                    command.Parameters.AddWithValue("@VoiceAuth", NewVoiceAuth);
                    command.Parameters.AddWithValue("@DoNotCall", NewDoNotCall);
                    command.Parameters.AddWithValue("@ShareAffiliates", NewShareAffiliates);
                    command.Parameters.AddWithValue("@IdType", NewIdType);
                    command.Parameters.AddWithValue("@IdStateName", NewIdStateName);
                    command.Parameters.AddWithValue("@IdNum", NewIdNum);
                    command.Parameters.AddWithValue("@IdExp", NewIdExp);
                    command.Parameters.AddWithValue("@MothersMaiden", NewMothersMaiden);

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
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? VoiceAuth { get; set; }
        public bool? DoNotCall { get; set; }
        public bool? ShareAffiliates { get; set; }
        public string IdType { get; set; }
        public string IdStateName { get; set; }
        public string IdNum { get; set; }
        public string IdExp { get; set; }
        public string MothersMaiden { get; set; }
        public string CountryName { get; set; }
        
        // Method to retrieve account contact by account number.
        public static IEnumerable<ClientEditInformationModel> GetClientInfoByAcctNum(string acctNum)
        {
            // List to store accounts objects.
            var clientInfos = new List<ClientEditInformationModel>();

            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Client_GetClientInfoByAcctNum]";

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
                            var clientInfo = new ClientEditInformationModel
                            {
                                FirstName = reader["First Name"] as string,
                                MiddleName = reader["Middle Name"] as string,
                                LastName = reader["Last Name"] as string,
                                Suffix = reader["Suffix"] as string,
                                DateOfBirth = reader["Date of Birth"] as DateTime?,
                                VoiceAuth = reader["Voice Auth"] as bool?,
                                DoNotCall = reader["DNC"] as bool?,
                                ShareAffiliates = reader["Share Affiliates"] as bool?,
                                IdType = reader["ID Type"] as string,
                                IdStateName = reader["ID State"] as string,
                                IdNum = reader["ID Number"] as string,
                                IdExp = reader["ID Expiration"] as string,
                                MothersMaiden = reader["Mothers Maiden"] as string,
                                CountryName = reader["Country Name"] as string
                            };
                            clientInfos.Add(clientInfo);
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
            return clientInfos;
        }
    }
}