using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppHandler.CandyDelivery
{
	public class FileStreamer
	{
		public static SqlFileStream GetData()
		{
			const string connectionString = @"Persist Security Info=False;Integrated Security=SSPI;database=NEHST;server=WIN-TK7VVQF2IT3;Connect Timeout=120";
			const string sqlQuery = @"
SELECT TOP 1
	[MetaDataFile].PathName()
FROM
	[NEHST].[dbo].[MetaData]";
			const string sqlTransactionQuery = @"SELECT GET_FILESTREAM_TRANSACTION_CONTEXT()";


			//using (TransactionScope transactionScope = new TransactionScope())
			//{
			using (SqlConnection sqlConnection = new SqlConnection(connectionString))
			{
				sqlConnection.Open();

				using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection))
				{
					string filePath = (string)sqlCommand.ExecuteScalar();

					using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
					{
						sqlCommand.Transaction = sqlTransaction;
						sqlCommand.CommandText = sqlTransactionQuery;
						return new SqlFileStream(filePath, (byte[])sqlCommand.ExecuteScalar(), FileAccess.ReadWrite);
					}
				}

			}
		}
	}
}
