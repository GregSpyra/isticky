using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pep.AppHandler.CandyStore.OOXML
{
	/// <summary>
	/// Class handing low level OOXML document operations
	/// </summary>
	public class Document : IDisposable
	{
		#region Variables
		private bool disposed = false;
		private WordprocessingDocument data;
		#endregion

		#region Properties
		public WordprocessingDocument Data
		{
			get
			{
				return this.data;
			}
			set
			{
				this.data = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Creates an instance of a single OOXML document
		/// </summary>
		public Document()
		{
			//this.data = this.GetDocument();
		}
		#endregion

		#region Public Methods
		public WordprocessingDocument OpenDefault()
		{
			return this.data;
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Method retrieves OOXML document from SqlFileStream
		/// </summary>
		/// <returns></returns>
		//private WordprocessingDocument GetDocument()
		//{
			//using (CandyDelivery.SingleStream fileStreamer = new CandyDelivery.SingleStream())
			//	return WordprocessingDocument.Create(fileStreamer.FileStream, DocumentFormat.OpenXml.WordprocessingDocumentType.Document);
		//}
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
					if (this.data != null)
						this.data.Dispose();
				}

				this.disposed = true;
			}
		}
		#endregion
	}
}
