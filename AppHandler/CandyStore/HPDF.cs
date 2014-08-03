using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pep.AppHandler.CandyStore
{
    public class HPDF : ICandy, IDisposable
	{
		#region Variables
		private bool _bDisposed;

		private MemoryMappedFile _mmFile;
		//private MemoryMappedViewStream _mmViewStream;
		private string _filePath;
		#endregion

		#region Constructors
		public HPDF()
		{

		}

		public HPDF(byte[] FileContent)
		{
			this.PlantFile(FileContent, out this._filePath);
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
			try
			{
				Process.Start(this._filePath);
			}
			catch (Exception eX)
			{
				throw new Exception(string.Format("{0}::{1}", new StackFrame(0, true).GetMethod().Name, eX.Message));
			}
		}

		
		/// <summary>
		/// Creates MemoryMappedFile for unwrapped policy data
		/// </summary>
		/// <param name="FileContent">Data file content</param>
		public void PlantFile(byte[] FileContent)
		{
			this.PlantFile(FileContent, out this._filePath);
		}

		/// <summary>
		/// Creates MemoryMappedFile for unwrapped policy data
		/// </summary>
		/// <param name="FileContent">Data file content</param>
		/// <param name="FilePath">Path for newly generated file</param>
		private void PlantFile(byte[] FileContent, out string FilePath)
		{
			Candy.FileTypeExtension fileExtension = Candy.GetFileTypeExtensionFromSignature(ref FileContent);
			FilePath = String.Format(@"{0}.{1}", Guid.NewGuid().ToString(), fileExtension);

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
