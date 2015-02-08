using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pep.AppHandler.CandyStore
{
	/// <summary>
	/// Class provides information about different documents (candies) supported
	/// </summary>
	public class Candy
	{
		#region Enums
		/// <summary>
		/// Enumerator for supported document types
		/// </summary>
		enum Type
		{
			OOXML = 0x01,
			PDF = 0x02
		};
		#endregion
	}
}
