using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pep.AppHandler.CandyDelivery
{
	public class FileStreamer : IDisposable
	{
		#region Variables
		private bool disposed = false;
		private string sqlConnectionString;
		private SqlConnection sqlConnection;
		#endregion

		#region Properties
		public SqlConnection SqlConnection
		{
			get
			{
				return this.sqlConnection;
			}
		}
		#endregion

		#region Constructors
		public FileStreamer(string sqlConnectionString)
		{
			this.sqlConnectionString = sqlConnectionString;
			this.sqlConnection = new SqlConnection(sqlConnectionString);
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
					if (this.sqlConnection != null)
						this.sqlConnection.Dispose();
				}

				this.disposed = true;
			}
		}

		#endregion
	}
}
