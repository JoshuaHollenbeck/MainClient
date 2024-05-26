using System;
using System.Data.SqlClient;
using System.Data;
using System.Security;
using System.Windows;
using MainClient.Utilities;


namespace MainClient._Model
{
    // This class represents the model for the login window.
    public class LoginWindowModel
    {
        // Method to handle user login.
        public (bool Result, string RepId, bool Trading, bool MoveMoney, bool ViewOnly) Login(
            string username,
            SecureString password
        )
        {
            // Convert SecureString password to byte array
            byte[] passwordBytes = ConvertSecureStringToByteArray(password);

            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[MainClient_LoginEmployee]";

            bool trading = false;
            bool moveMoney = false;
            bool viewOnly = false;
            string repId = string.Empty;
            bool result = false;

            try
            {
                // Using statement for ensuring proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameter for account number.
                    command.Parameters.Add(new SqlParameter("@inputRepID", username));
                    command.Parameters.Add(
                        new SqlParameter(
                            "@inputPassword",
                            new System.Net.NetworkCredential(string.Empty, password).Password
                        )
                    );

                    // Define output parameters
                    SqlParameter messageOutput = new SqlParameter();
                    messageOutput.ParameterName = "@LoginMessage";
                    messageOutput.SqlDbType = System.Data.SqlDbType.VarChar;
                    messageOutput.Size = 50;
                    messageOutput.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(messageOutput);

                    SqlParameter repIdOutput = new SqlParameter(
                        "@RepId",
                        System.Data.SqlDbType.VarChar
                    );
                    repIdOutput.Size = 5;
                    repIdOutput.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(repIdOutput);
                    
                    SqlParameter tradingOutput = new SqlParameter(
                        "@Trading",
                        System.Data.SqlDbType.Bit
                    );
                    tradingOutput.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(tradingOutput);

                    SqlParameter moveMoneyOutput = new SqlParameter(
                        "@MoveMoney",
                        System.Data.SqlDbType.Bit
                    );
                    moveMoneyOutput.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(moveMoneyOutput);

                    SqlParameter viewOnlyOutput = new SqlParameter(
                        "@ViewOnly",
                        System.Data.SqlDbType.Bit
                    );
                    viewOnlyOutput.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(viewOnlyOutput);

                    connection.Open();
                    
                    // Executing the non-query command to insert the notes into the database.
                    command.ExecuteNonQuery();

                    // Retrieve output parameter values
                    string message = command.Parameters["@LoginMessage"].Value.ToString();
                    result = message == "Login successful";
                    repId = command.Parameters["@RepId"].Value.ToString();
                    trading = Convert.ToBoolean(command.Parameters["@trading"].Value);
                    moveMoney = Convert.ToBoolean(command.Parameters["@MoveMoney"].Value);
                    viewOnly = Convert.ToBoolean(command.Parameters["@ViewOnly"].Value);
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
                MessageBox.Show($"Username or Password is incorrect.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return (result, repId, trading, moveMoney, viewOnly);
        }

        // Method to convert SecureString to byte array.
        private byte[] ConvertSecureStringToByteArray(SecureString secureString)
        {
            byte[] passwordBytes = new byte[secureString.Length * sizeof(char)];
            IntPtr ptr = IntPtr.Zero;

            try
            {
                ptr = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(
                    secureString
                );
                System.Runtime.InteropServices.Marshal.Copy(
                    ptr,
                    passwordBytes,
                    0,
                    passwordBytes.Length
                );
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                {
                    System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(ptr);
                }
            }

            return passwordBytes;
        }
    }
}
