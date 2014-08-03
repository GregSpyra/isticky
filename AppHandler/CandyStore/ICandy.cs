using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pep.AppHandler.CandyStore
{
	/// <summary>
	/// Interface for wrapped data content
	/// Delivers secure execution space for sticky policy data
	/// </summary>
	public interface ICandy
	{
		void OpenDefault();

		void PlantFile(byte[] FileContent);
	}
}
