using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Utilities;

namespace MainClient.Services
{
    public class AddAccountService
    {
        public class InitialContactMethodInfo
        {
            public string InitialContact { get; set; }
        }

        // Method to look up bank account types.
        public static IEnumerable<InitialContactMethodInfo> LookUpInitialContactMethod()
        {
            // List to store bank account types.
            var initialContactMethodLists = new List<InitialContactMethodInfo>();

            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[MainClient_LookUpInitialContactMethod]";

            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    // Executing the stored procedure and reading the results.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var initialContactMethodList = new InitialContactMethodInfo
                            {
                                InitialContact = reader["Initial Contact Method"] as string
                            };
                            initialContactMethodLists.Add(initialContactMethodList);
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
            return initialContactMethodLists;
        }

        public class AcctTypeInfo
        {
            public string AcctType { get; set; }
        }

        // Method to look up bank account types.
        public static IEnumerable<AcctTypeInfo> LookUpAcctType()
        {
            // List to store bank account types.
            var acctTypeLists = new List<AcctTypeInfo>();

            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[MainClient_LookUpAcctType]";

            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    // Executing the stored procedure and reading the results.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var acctTypeList = new AcctTypeInfo
                            {
                                AcctType = reader["Account Type"] as string
                            };
                            acctTypeLists.Add(acctTypeList);
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
            return acctTypeLists;
        }

        public class AcctObjectiveInfo
        {
            public string AcctObjective { get; set; }
        }

        // Method to look up bank account types.
        public static IEnumerable<AcctObjectiveInfo> LookUpAcctObjective()
        {
            // List to store bank account types.
            var acctObjectiveLists = new List<AcctObjectiveInfo>();

            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[MainClient_LookUpAcctObjective]";

            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    // Executing the stored procedure and reading the results.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var acctObjectiveList = new AcctObjectiveInfo
                            {
                                AcctObjective = reader["Objective Type"] as string
                            };
                            acctObjectiveLists.Add(acctObjectiveList);
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
            return acctObjectiveLists;
        }

        public class AcctFundingInfo
        {
            public string AcctFunding { get; set; }
        }

        // Method to look up bank account types.
        public static IEnumerable<AcctFundingInfo> LookUpAcctFunding()
        {
            // List to store bank account types.
            var acctFundingLists = new List<AcctFundingInfo>();

            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[MainClient_LookUpAcctFunding]";

            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    // Executing the stored procedure and reading the results.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var acctFundingList = new AcctFundingInfo
                            {
                                AcctFunding = reader["Funding Type"] as string
                            };
                            acctFundingLists.Add(acctFundingList);
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
            return acctFundingLists;
        }
        
        public class AcctPurposeInfo
        {
            public string AcctPurpose { get; set; }
        }

        // Method to look up bank account types.
        public static IEnumerable<AcctPurposeInfo> LookUpAcctPurpose()
        {
            // List to store bank account types.
            var acctPurposeLists = new List<AcctPurposeInfo>();

            // Connection string to the database.
            string connectionString = Connection.connectionString;

            try
            {
                // Name of the stored procedure.
                string storedProcedure = "[dbo].[MainClient_LookUpAcctPurpose]";

                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    // Executing the stored procedure and reading the results.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var acctPurposeList = new AcctPurposeInfo
                            {
                                AcctPurpose = reader["Purpose Type"] as string
                            };
                            acctPurposeLists.Add(acctPurposeList);
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
            return acctPurposeLists;
        }

        public class AcctActivityInfo
        {
            public string AcctActivity { get; set; }
        }

        // Method to look up bank account types.
        public static IEnumerable<AcctActivityInfo> LookUpAcctActivity()
        {
            // List to store bank account types.
            var acctActivityLists = new List<AcctActivityInfo>();

            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[MainClient_LookUpAcctActivity]";

            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    // Executing the stored procedure and reading the results.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var acctActivityList = new AcctActivityInfo
                            {
                                AcctActivity = reader["Activity Type"] as string
                            };
                            acctActivityLists.Add(acctActivityList);
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
            return acctActivityLists;
        }

        public class RelationshipTypeInfo
        {
            public string RelationshipType { get; set; }
        }

        // Method to look up bank account types.
        public static IEnumerable<RelationshipTypeInfo> LookUpRelationshipType()
        {
            // List to store bank account types.
            var relationshipTypeLists = new List<RelationshipTypeInfo>();

            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[MainClient_LookUpRelationshipType]";

            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    // Executing the stored procedure and reading the results.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var relationshipTypeList = new RelationshipTypeInfo
                            {
                                RelationshipType = reader["Relationship Type"] as string
                            };
                            relationshipTypeLists.Add(relationshipTypeList);
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
            return relationshipTypeLists;
        }
        
        public class PoaRoleInfo
        {
            public string PoaRole { get; set; }
        }

        // Method to look up bank account types.
        public static IEnumerable<PoaRoleInfo> LookUpPoaRole()
        {
            // List to store bank account types.
            var poaRoleLists = new List<PoaRoleInfo>();

            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[MainClient_LookUpPoaRole]";

            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    // Executing the stored procedure and reading the results.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var poaRoleList = new PoaRoleInfo
                            {
                                PoaRole = reader["Poa Role"] as string
                            };
                            poaRoleLists.Add(poaRoleList);
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
            return poaRoleLists;
        }
    }
}