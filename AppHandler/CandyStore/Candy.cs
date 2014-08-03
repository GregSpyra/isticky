using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pep.AppHandler.CandyStore
{
	class Candy
	{
		#region Enums
		public enum FileTypeExtension
		{
			Unsupported = 0,
			PDF = 1
		}
		#endregion

		#region Variables
		private static readonly byte[] FILE_SIGNATURE_PDF = new byte[] { 0x25, 0x50, 0x44, 0x46 };
		#endregion

		#region Public Static Methods
		public static FileTypeExtension GetFileTypeExtensionFromSignature(ref byte[] FileContent)
		{
			byte[] temp = FileContent.Take(FILE_SIGNATURE_PDF.Length).ToArray();
			if( StructuralComparisons.StructuralEqualityComparer.Equals(FILE_SIGNATURE_PDF, FileContent.Take(FILE_SIGNATURE_PDF.Length).ToArray()) )
			{
				return FileTypeExtension.PDF;
			}
			return FileTypeExtension.Unsupported;
		}
		#endregion
	}
}
