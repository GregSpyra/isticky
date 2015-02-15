using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

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
		private FileAccess fileAccess;
		private byte[] streamHandle;
		#endregion

		#region Properties
		public Stream Stream
		{
			get
			{
				return this.stream;
			}
		}

		public string MetaDataID
		{
			get
			{
				return this.metaDataID;
			}
			set
			{
				this.metaDataID = value;
			}
		}
		#endregion

		#region Constructors

		public SingleStream(FileStreamer fileStreamer, Stream stream)
		{
			this.fileStreamer = fileStreamer;
			this.fileAccess = FileAccess.ReadWrite;
			this.UploadData(stream);
			this.stream = GetStream();
		}

		public SingleStream(FileStreamer fileStreamer, string metaDataID)
		{
			this.fileAccess = FileAccess.ReadWrite;
			this.fileStreamer = fileStreamer;
			this.metaDataID = metaDataID;
			this.stream = GetStream();
		}
		#endregion

		#region Public Methods
	
		#endregion

		#region Private Methods
		public void UploadData(Stream stream)
		{
			const string sqlTransactionQuery = @"
INSERT INTO
	MetaData
	(MetaDataFile)
OUTPUT
	INSERTED.MetaDataFile.PathName(),
	INSERTED.MetaDataID,
	GET_FILESTREAM_TRANSACTION_CONTEXT()
VALUES (0x)";
			try
			{
				if (this.fileStreamer.SqlConnection.State == System.Data.ConnectionState.Closed)
				{
					this.fileStreamer.SqlConnection.Open();
				}
				using (SqlCommand sqlCommand = new SqlCommand(sqlTransactionQuery, this.fileStreamer.SqlConnection))
				{
					this.sqlTransaction = this.fileStreamer.SqlConnection.BeginTransaction();
					sqlCommand.Transaction = this.sqlTransaction;
					sqlCommand.CommandText = sqlTransactionQuery;

					string sqlStreamFullPath;
					byte[] data;

					using (SqlDataReader sqlReader = sqlCommand.ExecuteReader())
					{
						sqlReader.Read();
						sqlStreamFullPath = sqlReader.GetString(0);
						this.metaDataID = sqlReader.GetGuid(1).ToString();
						data = sqlReader.GetSqlBytes(2).Buffer;
					}

					using (SqlFileStream sqlFileStream = new SqlFileStream(sqlStreamFullPath, data, FileAccess.Write))
					{
						stream.CopyTo(sqlFileStream);
					}

					sqlCommand.Transaction.Commit();
				}
			}
			catch
			{
			}
		}

		private SqlFileStream GetStream()
		{
			const string SQL_TRANS_QUERY = @"SELECT GET_FILESTREAM_TRANSACTION_CONTEXT()";
            //byte[] buffer;
            //UInt32 position = 0;

			string sqlQuery = String.Format(@"
SELECT TOP 1
	[MetaDataFile].PathName()
FROM
	[NEHST].[dbo].[MetaData]
WHERE
	[MetaDataID] = '{0}'", this.metaDataID);
            
			if( this.fileStreamer.SqlConnection.State == System.Data.ConnectionState.Closed)
			{
				this.fileStreamer.SqlConnection.Open();
			}
			using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, this.fileStreamer.SqlConnection))
			{
				//using (SqlTransaction sqlTransaction 
				this.sqlTransaction = this.fileStreamer.SqlConnection.BeginTransaction(this.metaDataID.Replace("-", String.Empty));
				sqlCommand.Transaction = this.sqlTransaction;

				string filePath = (string)sqlCommand.ExecuteScalar();
				//SetRemoteSecurityContext(filePath);

				sqlCommand.CommandText = SQL_TRANS_QUERY;

				this.streamHandle = (byte[])sqlCommand.ExecuteScalar();
				return new SqlFileStream(filePath, this.streamHandle, this.fileAccess);
			}
		}
		#endregion

		private void SetRemoteSecurityContext(string filePath)
		{
			string securityContext = Thread.CurrentPrincipal.Identity.Name;
			FileSystemAccessRule rule = new FileSystemAccessRule(securityContext, FileSystemRights.Write, AccessControlType.Allow); 

			PermissionSet permissionSet = new PermissionSet(PermissionState.Unrestricted);
			permissionSet.AddPermission(new FileIOPermission(FileIOPermissionAccess.Read, new string[] { filePath }));
			permissionSet.AddPermission(new FileIOPermission(FileIOPermissionAccess.Write | FileIOPermissionAccess.PathDiscovery, new string[] { filePath })); 
			permissionSet.Assert();

			DirectoryInfo dirInfo = new DirectoryInfo(Path.GetDirectoryName(filePath)); 


			bool what = false; 
			DirectorySecurity security = dirInfo.GetAccessControl(); 

			security.ModifyAccessRule(AccessControlModification.Add, rule, out what); 
			dirInfo.SetAccessControl(security); 
		}
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
						try { this.sqlTransaction.Commit(); } catch { ;}
						this.sqlTransaction.Dispose();
				}

				this.disposed = true;
			}
		}
		#endregion
	}
}