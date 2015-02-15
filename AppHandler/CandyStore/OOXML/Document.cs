using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using pep.AppHandler.CandyDelivery;

namespace pep.AppHandler.CandyStore.OOXML
{
	/// <summary>
	/// Class handing low level OOXML document operations
	/// </summary>
	public class XDocument : IDisposable
	{
		#region Constraints
		private const string URI_REL_POLICY = @"/docProps/policy.xml";
		private const string CNT_PART_TYPE_POLICY = @"application/vnd.openxmlformats-officedocument.customXmlProperties+xml";
		#endregion

		#region Variables
		private bool disposed = false;
		private WordprocessingDocument data;
		private SingleStream singleStream;
		private byte[] policy;
		#endregion

		#region Properties
		/// <summary>
		/// OOXML document property
		/// </summary>
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
		/// <param name="singleStream">Document file stream</param>
		public XDocument(SingleStream singleStream)
		{
			this.singleStream = singleStream;
			this.WrapDocument(FileAccess.Read);
		}

		/// <summary>
		/// Creates an instance of a single OOXML document supporting basic access control functionality
		/// </summary>
		/// <param name="singleStream">Document file stream</param>
		/// <param name="fileAccess">Access type</param>
		public XDocument(SingleStream singleStream, FileAccess fileAccess)
		{
			this.singleStream = singleStream;
			this.WrapDocument(fileAccess);
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Returns single OOXML document
		/// </summary>
		/// <returns>Single OOXML document</returns>
		public WordprocessingDocument GetDocument()
		{
			return this.data;
		}

		/// <summary>
		/// Method saves document from stream into file system
		/// G$ Issues - do not use before fix; documents corrupted>>
		/// </summary>
		/// <param name="localPath">New document name and location</param>
		public void UnloadDocument(string localPath)
		{
			using(FileStream fileStream = new FileStream(localPath, FileMode.Create))
			{
				this.singleStream.Stream.Position = 0;
				this.singleStream.Stream.CopyTo(fileStream);
			}
		}

		/// <summary>
		/// Adds new XACML policy to OOXML document - no force
		/// </summary>
		/// <param name="xmlDocument">XACML policy document</param>
		public  void AddPolicy(XmlDocument xmlDocument)
		{
			AddPolicy(xmlDocument, false);
		}

		/// <summary>
		/// Adds new XACML policy to OOXML document
		/// </summary>
		/// <param name="xmlDocument">XACML policy document</param>
		/// <param name="force">Force old policy replacement if exists</param>
		public void AddPolicy(XmlDocument xmlDocument, bool force)
		{
			this.data.Close();
			using( Package package = Package.Open(this.singleStream.Stream, FileMode.Open, FileAccess.ReadWrite) )
			{
				Uri uriLocation = new Uri(URI_REL_POLICY, UriKind.Relative);
				if(!package.PartExists(uriLocation) || force)
				{
					PackagePart packagePart = package.CreatePart(uriLocation, CNT_PART_TYPE_POLICY);
					using(Stream stream = packagePart.GetStream(FileMode.Create, FileAccess.ReadWrite))
					{
						xmlDocument.Save(stream);
					}
				}
			}
			this.data = WordprocessingDocument.Open(this.singleStream.Stream, true);

			//MainDocumentPart mainPart = this.data.MainDocumentPart;
			//if(force)
			//{ 
			//	mainPart.DeleteParts<CustomXmlPart>(mainPart.CustomXmlParts);
			//}
			//CustomXmlPart xmlPart = new CustomXmlPart();
			

			//CustomFilePropertiesPart customProperty;
			//if (this.data.CustomFilePropertiesPart == null)
			//{
			//	customProperty = this.data.AddCustomFilePropertiesPart();
			//}
			//else
			//{
			//	customProperty = this.data.CustomFilePropertiesPart;
			//}
			//xmlDocument.Save(customProperty.GetStream());
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Method wraps document stream into OOXML document
		/// </summary>
		private void WrapDocument(FileAccess fileAccess)
		{
			OpenSettings settings = new OpenSettings();
			settings.AutoSave = false;
			this.data = WordprocessingDocument.Open(singleStream.Stream, (fileAccess == FileAccess.ReadWrite), settings);
		}

		/// <summary>
		/// Method tries openning OOXML document from the stream
		/// </summary>
		/// <param name="singleStream">Document stream</param>
		/// <param name="document">Opened out OOXML document</param>
		/// <returns></returns>
		private bool TryOpenFromStream(SingleStream singleStream, out WordprocessingDocument document)
		{
			OpenSettings settings = new OpenSettings();
			settings.AutoSave = false;
			try
			{
				document = WordprocessingDocument.Open(singleStream.Stream, false, settings);
			}
			catch
			{
				document = null;
				return false;
			}
			return true;
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
					if (this.data != null)
						this.data.Close();
						this.data.Dispose();
					if (this.singleStream != null)
					{
						singleStream.Dispose();
					}
				}

				this.disposed = true;
			}
		}
		#endregion
	}
}
