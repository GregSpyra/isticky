using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pep.AppHandler.CandyStore
{
	/// <summary>
	/// Interface for wrapped data content
	/// Delivers secure execution space for sticky policy data
	/// </summary>
	public interface ICandy : IDisposable
	{

		/// <summary>
		/// Builds interpreted XACML master document wrapper
		/// </summary>
		/// <param name="documents">Documents file streams</param>
		void Wrap(List<FileStream> documents);

//		void PlantFile(byte[] FileContent);
	}
}
