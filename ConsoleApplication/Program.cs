using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pep.AppHandler.ConsoleApplicationn
{
    class Program
    {
        static void Main(string[] args)
        {
			string pwd = SimpleCrypto.DecryptString(@"jIm+nJyQipGLv7asrI+GjZ7RkZqL");
			string filePath = @"C:\Projects\iSticky\ConsoleApplication\ResearchAndReferencing.pdf";
			using (pep.AppHandler.CandyStore.HPDF pdfCandy = new pep.AppHandler.CandyStore.HPDF(File.ReadAllBytes(filePath)))
			{
				pdfCandy.OpenDefault();
			}
        }
    }

	public class SimpleCrypto : IDisposable
	{
		#region Private Variables
		private bool bDisposed;
		#endregion

		#region Constructors
		public SimpleCrypto()
		{
			this.bDisposed = false;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Encrypts text into base64 format string. Binary value of each text character is simply chagned with binary negation.
		/// </summary>
		public string Encrypt(string PlainText)
		{
			try
			{
				return Convert.ToBase64String(PlainText.ToCharArray().Select(cChar => (byte)(~Convert.ToByte(cChar))).ToArray());
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("{0}::{1}", new StackFrame(0, true).GetMethod().Name, ex.Message));
			}
		}

		/// <summary>
		/// Decrypts text from base64 format string. Binary value of each text character is simply chagned with binary negation.
		/// </summary>
		public string Decrypt(string DecryptedText)
		{
			try
			{
				return new String(Convert.FromBase64String(DecryptedText).Select(bByte => Convert.ToChar((byte)~bByte)).ToArray());
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("{0}::{1}", new StackFrame(0, true).GetMethod().Name, ex.Message));
			}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Encrypts text into base64 format string. Binary value of each text character is simply chagned with binary negation.
		/// </summary>
		public static string EncryptString(string PlainText)
		{
			try
			{
				return Convert.ToBase64String(PlainText.ToCharArray().Select(cChar => (byte)(~Convert.ToByte(cChar))).ToArray());
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("{0}::{1}", new StackFrame(0, true).GetMethod().Name, ex.Message));
			}
		}

		/// <summary>
		/// Decrypts text from base64 format string. Binary value of each text character is simply chagned with binary negation.
		/// </summary>
		public static string DecryptString(string EncryptedText)
		{
			try
			{
				return new String(Convert.FromBase64String(EncryptedText).Select(bByte => Convert.ToChar((byte)~bByte)).ToArray());
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("{0}::{1}", new StackFrame(0, true).GetMethod().Name, ex.Message));
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
			if (!this.bDisposed)
			{
				if (bDisposing)
				{

				}

				this.bDisposed = true;
			}
		}

		#endregion
	}
}
