using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using MainClient.Utilities;

namespace MainClient._Model
{
    // This class represents the model for advanced search functionality.
    public class AdvancedSearchModel
    {
        // Nested class to represent search results.
        public class AdvancedSearchResult
        {
            public string AccountNumber { get; set; }
            public string FirstLastName { get; set; }
            public string CustomerID { get; set; }
            public string JointCustomerId { get; set; }
            public string AccountAddress { get; set; }
            public string AccountCity { get; set; }
            public string AccountState { get; set; }
            public string AccountZip { get; set; }
            public byte? AccountType { get; set; }            
        }

        // Method to perform advanced search.
        public List<AdvancedSearchResult> AcctAdvancedSearch(
            string lastName,
            string firstName,
            string middleIntial,
            string custSecondaryId,
            string custPhone,
            string custZip,
            string custTaxId,
            string acctNum,
            string custEmail
        )
        {
            List<AdvancedSearchResult> advancedSearchResults = new List<AdvancedSearchResult>();
            
            // Retrieving connection string from the utility class.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Acct_AdvancedSearch]";

            try
            {
                // Using statement for ensuring proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {   
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Adding parameter for account number.
                    command.Parameters.Add(new SqlParameter("@LastName", lastName));
                    command.Parameters.Add(new SqlParameter("@FirstName", firstName));
                    command.Parameters.Add(new SqlParameter("@MiddleInitial", middleIntial));
                    command.Parameters.Add(new SqlParameter("@CustSecondaryID", custSecondaryId));
                    command.Parameters.Add(new SqlParameter("@CustPhone", custPhone));
                    command.Parameters.Add(new SqlParameter("@CustZip", custZip));
                    command.Parameters.Add(new SqlParameter("@CustTaxID", custTaxId));
                    command.Parameters.Add(new SqlParameter("@AcctNum", acctNum));
                    command.Parameters.Add(new SqlParameter("@CustEmail", custEmail));

                    connection.Open();
                    
                    // Executing the stored procedure and reading the results.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AdvancedSearchResult result = new AdvancedSearchResult
                            {
                                AccountNumber = reader["Acct Num"] as string,
                                FirstLastName = reader["First & Last Name"] as string,
                                CustomerID = reader["Cust Id"] as string,
                                JointCustomerId = reader["Joint Cust Id"] as string,
                                AccountAddress = reader["Address"] as string,
                                AccountCity = reader["City"] as string,
                                AccountState = reader["State"] as string,
                                AccountZip = reader["Zip"] as string,
                                AccountType = reader["Acct Type"] as byte?
                            };
                            advancedSearchResults.Add(result);
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
            return advancedSearchResults;
        }
        
        // Method to insert account access history by account number.
        public static void InsertAcctAccessHistoryByAcctNum(string acctNum, string repId)
        {
            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;
            
            // Stored procedure name.
            string storedProcedure = "[dbo].[Acct_InsertAcctAccessHistoryByAcctNum]";

            try
            {
                // Using statement for ensuring proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Adding parameter for account number.
                    command.Parameters.Add(new SqlParameter("@AcctNum", acctNum));
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