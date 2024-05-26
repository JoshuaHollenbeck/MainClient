using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient._ViewModel;
using MainClient.Utilities;

namespace MainClient.Services
{
    // This class provides methods for performing financial transactions.
    public class MoveMoneyService
    {
        // Method to insert an ACH withdrawal transaction.
        public void InsertAcctTransactionAchWithdrawalByAcctNum(
            string AchAccountNumber,
            string AchRepId,
            string AchBankAccountNumber,
            decimal? AchAmount,
            string AchBankRoutingNumber
        )
        {
            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[Acct_InsertAcctTransactionAchWithdrawalByAcctNum]";
            
            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command.
                    command.Parameters.AddWithValue("@AcctNum", AchAccountNumber);
                    command.Parameters.AddWithValue("@RepId", AchRepId);
                    command.Parameters.AddWithValue("@BankAcctNum", AchBankAccountNumber);
                    command.Parameters.AddWithValue("@TransAmt", AchAmount);
                    command.Parameters.AddWithValue("@BankRoutingNum", AchBankRoutingNumber);

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

        // Method to insert a cash deposit transaction.
        public void InsertAcctTransactionCashDepositByAcctNum(
            string CashAccountNumber,
            string CashRepId,
            decimal? CashAmount
        )
        {
            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[Acct_InsertAcctTransactionCashDepositByAcctNum]";
            
            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command.
                    command.Parameters.AddWithValue("@AcctNum", CashAccountNumber);
                    command.Parameters.AddWithValue("@RepId", CashRepId);
                    command.Parameters.AddWithValue("@TransAmt", CashAmount);

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

        // Method to insert a cash withdrawal transaction.
        public void InsertAcctTransactionCashWithdrawalByAcctNum(
            string CashAccountNumber,
            string CashRepId,
            decimal? CashAmount
        )
        {
            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[Acct_InsertAcctTransactionCashWithdrawalByAcctNum]";
            
            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command.
                    command.Parameters.AddWithValue("@AcctNum", CashAccountNumber);
                    command.Parameters.AddWithValue("@RepId", CashRepId);
                    command.Parameters.AddWithValue("@TransAmt", CashAmount);

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

        // This method creates a DataTable from a collection of MultiCheckDetail objects.
        private DataTable MultipleCheckDetailsTable(
            IEnumerable<MultiCheckDetail> multiCheckDetailsCollection
        )
        {
            // Creating a new DataTable with a specific name.
            DataTable multiCheckDetailsTable = new DataTable("DepositMultipleCheckSingleAccount");

            // Adding columns to the DataTable.
            multiCheckDetailsTable.Columns.Add("MultiCheckAmount", typeof(decimal));
            multiCheckDetailsTable.Columns.Add("MultiPayerName", typeof(string));
            multiCheckDetailsTable.Columns.Add("MultiPayToOrderOf", typeof(string));
            multiCheckDetailsTable.Columns.Add("MultiCheckAccountNumber", typeof(string));
            multiCheckDetailsTable.Columns.Add("MultiCheckNumber", typeof(string));
            multiCheckDetailsTable.Columns.Add("MultiCheckRoutingNumber", typeof(string));
            multiCheckDetailsTable.Columns.Add("MultiCheckDate", typeof(DateTime));
            multiCheckDetailsTable.Columns.Add("MultiCheckMemo", typeof(string));

            // Populating the DataTable with data from the collection of MultiCheckDetail objects.
            foreach (var multicheck in multiCheckDetailsCollection)
            {
                multiCheckDetailsTable.Rows.Add(
                    multicheck.MultiCheckAmount,
                    multicheck.MultiPayerName,
                    multicheck.MultiPayToOrderOf,
                    multicheck.MultiCheckAccountNumber,
                    multicheck.MultiCheckNumber,
                    multicheck.MultiCheckRoutingNumber,
                    multicheck.MultiCheckDate.HasValue
                        ? multicheck.MultiCheckDate.Value
                        : (object)DBNull.Value,
                    multicheck.MultiCheckMemo
                );
            }
            return multiCheckDetailsTable;
        }

        // Method to insert a multiple check deposit transaction.
        public void InsertAcctTransactionCheckDepositMultipleCheckByAcctNum(
            IEnumerable<MultiCheckDetail> multiCheckDetailsCollection,
            string MultiAccountNumber,
            string MultiRepId
        )
        {
            DataTable multiCheckDetailsTable = MultipleCheckDetailsTable(
                new ObservableCollection<MultiCheckDetail>(multiCheckDetailsCollection)
            );

            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure =
                "[dbo].[Acct_InsertAcctTransactionCheckDepositMultipleCheckByAcctNum]";
            
            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command.
                    command.Parameters.AddWithValue("@AcctNum", MultiAccountNumber);
                    command.Parameters.AddWithValue("@RepId", MultiRepId);
                    SqlParameter multiCheckDetailsParam = command.Parameters.AddWithValue(
                        "@MultiCheckDetails",
                        multiCheckDetailsTable
                    );
                    multiCheckDetailsParam.SqlDbType = SqlDbType.Structured;
                    multiCheckDetailsParam.TypeName = "DepositMultipleCheckSingleAccount";

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

        // Method to insert a single check deposit transaction.
        public void InsertAcctTransactionCheckDepositSingleCheckByAcctNum(
            string SingleDepositAccountNumber,
            string SingleDepositRepId,
            decimal? SingleDepositCheckAmount,
            string SingleDepositPayerName,
            string SingleDepositPayToOrderOf,
            string SingleDepositCheckAccountNumber,
            string SingleDepositCheckNumber,
            string SingleDepositCheckRoutingNumber,
            DateTime? SingleDepositCheckDate,
            string SingleDepositCheckMemo
        )
        {
            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure =
                "[dbo].[Acct_InsertAcctTransactionCheckDepositSingleCheckByAcctNum]";
            
            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@AcctNum", SingleDepositAccountNumber);
                    command.Parameters.AddWithValue("@RepId", SingleDepositRepId);
                    command.Parameters.AddWithValue("@DepAmt", SingleDepositCheckAmount);
                    command.Parameters.AddWithValue("@PayerName", SingleDepositPayerName);
                    command.Parameters.AddWithValue("@PayToOrderOf", SingleDepositPayToOrderOf);
                    command.Parameters.AddWithValue("@CheckAcctNum", SingleDepositCheckAccountNumber);
                    command.Parameters.AddWithValue("@CheckNum", SingleDepositCheckNumber);
                    command.Parameters.AddWithValue("@CheckRouting", SingleDepositCheckRoutingNumber);
                    command.Parameters.AddWithValue("@CheckDate", SingleDepositCheckDate);
                    command.Parameters.AddWithValue("@Memo", SingleDepositCheckMemo);

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

        // This method creates a DataTable from a collection of SplitCheckDetail objects.
        private DataTable SplitCheckDetailsTable(
            IEnumerable<SplitCheckDetail> splitCheckDetailsCollection
        )
        {
            // Creating a new DataTable with a specific name.
            DataTable splitCheckDetailsTable = new DataTable("DepositSingleCheckMultipleAccount");

            // Adding columns to the DataTable.
            splitCheckDetailsTable.Columns.Add("SplitAccountNumber", typeof(string));
            splitCheckDetailsTable.Columns.Add("SplitCheckAmount", typeof(decimal));

            // Populating the DataTable with data from the collection of SplitCheckDetail objects.
            foreach (var splitCheck in splitCheckDetailsCollection)
            {
                splitCheckDetailsTable.Rows.Add(
                    splitCheck.SplitAccountNumber,
                    splitCheck.SplitCheckAmount
                );
            }
            return splitCheckDetailsTable;
        }

        // Method to insert a split check deposit transaction.
        public void InsertAcctTransactionCheckDepositSingleCheckSplitByAcctNum(
            IEnumerable<SplitCheckDetail> splitCheckDetailsCollection,
            string SplitRepId,
            string SplitPayerName,
            string SplitPayToOrderOf,
            string SplitCheckAccountNumber,
            string SplitCheckNumber,
            string SplitCheckRouting,
            DateTime? SplitCheckDate,
            string SplitCheckMemo,
            decimal? TotalCheckAmount
        )
        {
            DataTable splitCheckDetailsTable = SplitCheckDetailsTable(
                new ObservableCollection<SplitCheckDetail>(splitCheckDetailsCollection)
            );

            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure =
                "[dbo].[Acct_InsertAcctTransactionCheckDepositSingleCheckSplitByAcctNum]";
            
            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command.
                    command.Parameters.AddWithValue("@RepId", SplitRepId);
                    command.Parameters.AddWithValue("@PayerName", SplitPayerName);
                    command.Parameters.AddWithValue("@PayToOrderOf", SplitPayToOrderOf);
                    command.Parameters.AddWithValue("@CheckAcctNum", SplitCheckAccountNumber);
                    command.Parameters.AddWithValue("@CheckNum", SplitCheckNumber);
                    command.Parameters.AddWithValue("@CheckRouting", SplitCheckRouting);
                    command.Parameters.AddWithValue("@CheckDate", SplitCheckDate);
                    command.Parameters.AddWithValue("@Memo", SplitCheckMemo);
                    command.Parameters.AddWithValue("@TotalCheckAmt", TotalCheckAmount);
                    SqlParameter checkDetailsParam = command.Parameters.AddWithValue(
                        "@SplitCheckDetails",
                        splitCheckDetailsTable
                    );
                    checkDetailsParam.SqlDbType = SqlDbType.Structured;
                    checkDetailsParam.TypeName = "DepositSingleCheckMultipleAccount";

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

        // Method to insert a check withdrawal transaction.
        public void InsertAcctTransactionCheckWithdrawalByAcctNum(
            string SingleWithdrawalAccountNumber,
            string SingleWithdrawalRepId,
            decimal? SingleWithdrawalCheckAmount,
            string SingleWithdrawalPayToOrderOf,
            string SingleWithdrawalCheckNumber,
            DateTime? SingleWithdrawalCheckDate,
            string SingleWithdrawalCheckMemo
        )
        {
            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[Acct_InsertAcctTransactionCheckWithdrawalByAcctNum]";
            
            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command.
                    command.Parameters.AddWithValue("@AcctNum", SingleWithdrawalAccountNumber);
                    command.Parameters.AddWithValue("@RepId", SingleWithdrawalRepId);
                    command.Parameters.AddWithValue("@WithrdawalAmt", SingleWithdrawalCheckAmount);
                    command.Parameters.AddWithValue("@PayToOrderOf", SingleWithdrawalPayToOrderOf);
                    command.Parameters.AddWithValue("@CheckNum", SingleWithdrawalCheckNumber);
                    command.Parameters.AddWithValue("@CheckDate", SingleWithdrawalCheckDate);
                    command.Parameters.AddWithValue("@Memo", SingleWithdrawalCheckMemo);

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

        // Method to insert a journal transaction.
        public void InsertAcctTransactionJournalByAcctNum(
            string JournalFromAccountNumber,
            string JournalToAccountNumber,
            decimal? JournalAmount,
            string JournalRepId
        )
        {
            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[Acct_InsertAcctTransactionJournalByAcctNum]";
            
            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command.
                    command.Parameters.AddWithValue("@AcctNum", JournalFromAccountNumber);
                    command.Parameters.AddWithValue("@JournalAcctNum", JournalToAccountNumber);
                    command.Parameters.AddWithValue("@JournalAmt", JournalAmount);
                    command.Parameters.AddWithValue("@RepId", JournalRepId);

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

        // Method to insert a wire withdrawal transaction.
        public void InsertAcctTransactionWireWithdrawalByAcctNum(
            string WireFromAccountNumber,
            string WireRepId,
            string WireToAccountNumber,
            decimal? WireAmount,
            string WireRoutingNumber,
            string WireCurrency
        )
        {
            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[Acct_InsertAcctTransactionWireWithdrawalByAcctNum]";
            
            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command.
                    command.Parameters.AddWithValue("@AcctNum", WireFromAccountNumber);
                    command.Parameters.AddWithValue("@RepId", WireRepId);
                    command.Parameters.AddWithValue("@WireAcctNum", WireToAccountNumber);
                    command.Parameters.AddWithValue("@TransAmt", WireAmount);
                    command.Parameters.AddWithValue("@WireRoutingNum", WireRoutingNumber);
                    command.Parameters.AddWithValue("@WireCurrency", WireCurrency);

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
