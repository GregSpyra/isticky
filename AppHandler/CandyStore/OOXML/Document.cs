using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppHandler.CandyStore.OOXML
{
	class Document
	{
		public static WordprocessingDocument GetData()
		{
			return WordprocessingDocument.Create( CandyDelivery.FileStreamer.GetData(), DocumentFormat.OpenXml.WordprocessingDocumentType.Document);
		}
	}
}
