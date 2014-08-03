using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pep.AppHandler
{
	/// <summary>
	/// Interface for wrapped data content
	/// Delivers secure execution space for sticky policy data
	/// </summary>
    public interface ICandy
	{
		void OpenDefault();

		void PlantFile(byte[] FileContent, out string FilePath);
	}

    public class HPDF : ICandy, IDisposable
	{
		#region Variables
		private bool _bDisposed;

		private MemoryMappedFile _mmFile;
		//private MemoryMappedViewStream _mmViewStream;
		#endregion

		#region Constructors
		public HPDF()
		{

		}
		#endregion

		#region Public Methods

		/// <summary>
		/// Method opens data using default assigned Windows appliction
		/// Data with only NoCoat enabled policy can be opened.
		/// Data with disabled NoCoat policy can be opened only if data type
		/// is supported for secure file management
		/// </summary>
		public void OpenDefault()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates MemoryMappedFile for unwrapped policy data
		/// </summary>
		/// <param name="FileContent">Data file content</param>
		/// <param name="FilePath">Path for newly generated file</param>
		public void PlantFile(byte[] FileContent, out string FilePath)
		{
			FilePath = Guid.NewGuid().ToString();
			this._mmFile = MemoryMappedFile.CreateNew(FilePath, FileContent.LongLength);
			
			using (MemoryMappedViewStream mmViewStream = this._mmFile.CreateViewStream())
			{
				using (BinaryWriter binWriter = new BinaryWriter(mmViewStream))
				{
					binWriter.Write(FileContent);
				}
			}
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool bDisposing)
		{
			if (!this._bDisposed)
			{
				if (bDisposing)
				{
					//this._mmViewStream.Dispose();
					this._mmFile.Dispose();
				}

				this._bDisposed = true;
			}
		}
		#endregion
	}
}
