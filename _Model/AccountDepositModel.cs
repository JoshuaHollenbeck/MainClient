using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MainClient.Utilities;
using System.Data;

namespace MainClient._Model
{
    public class AccountDepositModel
    {
        public static void InsertAcctDepositCheckByAcctNum(string acctNum, string depositAmt, string repId)
        {
            string connectionString = Connection.connectionString;

            string storedProcedure = "[dbo].[Acct_InsertAcctDepositCheckByAcctNum]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(storedProcedure, connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                
                command.Parameters.Add(new SqlParameter("@AcctNum", acctNum));
                command.Parameters.Add(new SqlParameter("@DepAmt", depositAmt));
                command.Parameters.Add(new SqlParameter("@RepId", repId));

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void InsertAcctDepositSplitCheckByAcctNum(string acctNum, string splitAcctNum, string totalDepositAmt, string depositAmt, string splitDepositAmt, string repId)
        {
            string connectionString = Connection.connectionString;

            string storedProcedure = "[dbo].[Acct_InsertAcctDepositSplitCheckByAcctNum]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(storedProcedure, connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                
                command.Parameters.Add(new SqlParameter("@AcctNum", acctNum));
                command.Parameters.Add(new SqlParameter("@AcctNum", splitAcctNum));
                command.Parameters.Add(new SqlParameter("@DepAmt", totalDepositAmt));
                command.Parameters.Add(new SqlParameter("@DepAmt", depositAmt));
                command.Parameters.Add(new SqlParameter("@DepAmt", splitDepositAmt));
                command.Parameters.Add(new SqlParameter("@RepId", repId));

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void InsertAcctDepositMultiCheckByAcctNum(string acctNum, List<Tuple<string, decimal>> accountDeposits, decimal totalDepositAmt, string repId)
        {
            string connectionString = Connection.connectionString;
            string storedProcedure = "[dbo].[Acct_InsertAcctDepositMultiCheckByAcctNum]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(storedProcedure, connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;

                // Create a DataTable to hold the deposits
                DataTable depositTable = new DataTable();
                depositTable.Columns.Add("AccountNumber", typeof(string));
                depositTable.Columns.Add("DepositAmount", typeof(decimal));

                // Populate the DataTable with account number and deposit amount pairs
                foreach (var accountDeposit in accountDeposits)
                {
                    depositTable.Rows.Add(accountDeposit.Item1, accountDeposit.Item2);
                }

                // Add the TVP as a parameter
                SqlParameter tvpParam = command.Parameters.AddWithValue("@Deposits", depositTable);
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "dbo.AccountDepositType";

                command.Parameters.Add(new SqlParameter("@AcctNum", acctNum));
                command.Parameters.Add(new SqlParameter("@TotalDepAmt", totalDepositAmt));
                command.Parameters.Add(new SqlParameter("@RepId", repId));

                connection.Open();
                command.ExecuteNonQuery();
            }
        }   
    }
}