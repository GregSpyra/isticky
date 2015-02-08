using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Threading.Tasks;

namespace pep.AppHandler.CandyDelivery
{
	/// <summary>
	/// Class handing SqlFileStreams for documents
	/// </summary>
	public class SingleStream : IDisposable
    {
        #region Constants
        private const int packetSize = 10 * 1000 * 1024; //[MB]
        #endregion

        #region Variables
        private bool disposed = false;
		private string metaDataID;
		private FileStreamer fileStreamer;
		private Stream stream;
		SqlTransaction sqlTransaction;
		#endregion

		#region Properties
		public Stream Stream
		{
			get
			{
				return this.stream;
			}
		}
		#endregion

		#region Constructors
		public SingleStream(FileStreamer fileStreamer, string metaDataID)
		{
			this.fileStreamer = fileStreamer;
			this.metaDataID = metaDataID;
			this.stream = GetData();
		}
		#endregion

		#region Private Methods
		private SqlFileStream GetData()
		{
			const string sqlTransactionQuery = @"SELECT GET_FILESTREAM_TRANSACTION_CONTEXT()";
            //byte[] buffer;
            //UInt32 position = 0;

			string sqlQuery = String.Format(@"
SELECT TOP 1
	[MetaDataFile].PathName()
FROM
	[NEHST].[dbo].[MetaData]
WHERE
	[MetaDataID] = '{0}'", this.metaDataID);
            
			this.fileStreamer.SqlConnection.Open();
			using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, this.fileStreamer.SqlConnection))
			{
				string filePath = (string)sqlCommand.ExecuteScalar();

				//using (SqlTransaction sqlTransaction 
				this.sqlTransaction = this.fileStreamer.SqlConnection.BeginTransaction();
				sqlCommand.Transaction = sqlTransaction;
				sqlCommand.CommandText = sqlTransactionQuery;

				return new SqlFileStream(filePath, (byte[])sqlCommand.ExecuteScalar(), FileAccess.Read);
			}
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					if (this.stream != null)
						this.stream.Dispose();
					if (this.sqlTransaction != null)
						this.sqlTransaction.Dispose();
				}

				this.disposed = true;
			}
		}

		#endregion
	}
}
