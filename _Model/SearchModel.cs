using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using MainClient.Utilities;

namespace MainClient._Model
{
    // This class represents the model for search functionality.
    public class SearchModel
    {
        // Nested class to represent search results.
        public class SearchResult
        {
            public string AccountNumber { get; set; }
            public string FirstLastName { get; set; }
            public string CustomerID { get; set; }
            public string AccountAddress { get; set; }
            public string AccountCity { get; set; }
            public string AccountState { get; set; }
            public string AccountZip { get; set; }
            public string JointCustomerId { get; set; }
            public byte? AccountType { get; set; }  
        }

        // Method to perform search.
        public List<SearchResult> AcctSearch(
            string custSecondaryId,
            string custTaxId,
            string acctNum
        )
        {
            List<SearchResult> searchResults = new List<SearchResult>();
            
            // Retrieving connection string from the utility class.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Acct_Search]";

            try
            {
                // Using statement for ensuring proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {   
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Adding parameter for account number.
                    command.Parameters.Add(new SqlParameter("@CustSecondaryID", custSecondaryId));
                    command.Parameters.Add(new SqlParameter("@CustTaxID", custTaxId));
                    command.Parameters.Add(new SqlParameter("@AcctNum", acctNum));

                    connection.Open();
                    
                    // Executing the stored procedure and reading the results.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SearchResult result = new SearchResult
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
                            searchResults.Add(result);
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

            return searchResults;
        }
    }
}