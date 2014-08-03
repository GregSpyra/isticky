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
		private FileStream _fileStream;
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
				using(Process processHandle = new Process {StartInfo =  new ProcessStartInfo(this._filePath)})
				{
					processHandle.Start();
					processHandle.WaitForExit();
				}
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
			FilePath = String.Format(@"{0}\{1}.{2}", Path.GetTempPath(), Guid.NewGuid(), fileExtension);
			
			this._fileStream = new FileStream(FilePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

			using (BinaryWriter binWriter = new BinaryWriter(this._fileStream))
			{
				binWriter.Write(FileContent);
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
					this._fileStream.Dispose();
					File.Delete(this._filePath);
				}

				this._bDisposed = true;
			}
		}
		#endregion
	}
}
