using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using pep.AppHandler.CandyDelivery;

namespace pep.AppHandler.CandyStore.OOXML
{
	/// <summary>
	/// Class responsible for handling OOXML master document generation
	/// </summary>
	public class XMaster : IDisposable
	{
		#region Variables
		private bool disposed = false;
		private string name;
		private WordprocessingDocument data;
        private MemoryStream memoryStream;
		#endregion

		#region Constructors
		public XMaster()
		{
			this.name = Guid.NewGuid().ToString();
            this.memoryStream = new MemoryStream();
			this.data = WordprocessingDocument.Create(this.memoryStream, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true);
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Builds interpreted XACML master document wrapper
		/// </summary>
		/// <param name="documents">Documents file streams</param>
		public void Wrap(List<SingleStream> documents)
		{
			string altChunkId = "AltChunkId1";
            MainDocumentPart mainPart = this.data.AddMainDocumentPart();
			AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(
				AlternativeFormatImportPartType.WordprocessingML, altChunkId);
			foreach(SingleStream singleStream in documents)
			{
                OpenSettings settings = new OpenSettings();
                settings.AutoSave = false;
                WordprocessingDocument doc = WordprocessingDocument.Open(singleStream.Stream, false, settings);

                //FileMode.Open
				chunk.FeedData((Stream)singleStream.Stream);
				AltChunk altChunk = new AltChunk();
				altChunk.Id = altChunkId;
				mainPart.Document
					.Body
					.InsertAfter(altChunk, mainPart.Document.Body.Elements<Paragraph>().Last());
			}
			mainPart.Document.Save();
		}
		public void PlantFile() { }
		//public 
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
