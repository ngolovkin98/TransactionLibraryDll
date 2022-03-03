using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;


namespace TransactionLibrary
{
    public class shopStorage
    {
       public static string connectionString;
         public SqlConnection sqlConnection = new SqlConnection(connectionString);


        public void Reserve(string productName, int orderSize, int userId)
        {
             
            if (sqlConnection.State == ConnectionState.Open)
                sqlConnection.Close();
            sqlConnection.Open();
            SqlTransaction transaction = sqlConnection.BeginTransaction();
            SqlCommand command = sqlConnection.CreateCommand();
            command.Transaction = transaction;
            try
            {
                command.CommandText = $"UPDATE Shop SET AmountOfItems=AmountOfItems-{orderSize} WHERE ProductName = '{productName}';";
                command.ExecuteNonQuery();
                command.CommandText = $"UPDATE UserInfo SET ItemsPurshared=ItemsPurshared+{orderSize} WHERE id = '{userId}';";
                command.ExecuteNonQuery();
                command.CommandText = $"INSERT INTO TransactionInfo (UserId,TransactionTime,ItemSize,ProductName) VALUES ('{userId}','{DateTime.Now.ToString()}','{orderSize}', '{productName}' )";
                command.ExecuteNonQuery();
                transaction.Commit();
                sqlConnection.Close();

            }
            catch (Exception)
            {
                transaction.Rollback();
                sqlConnection.Close();
            }

        }

    }
    

    
}

