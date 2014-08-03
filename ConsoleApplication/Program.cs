using System;
using System.Collections.Generic;
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
			string filePath = @"C:\Projects\iSticky\ConsoleApplication\ResearchAndReferencing.pdf";
			using (pep.AppHandler.CandyStore.HPDF pdfCandy = new pep.AppHandler.CandyStore.HPDF(File.ReadAllBytes(filePath)))
			{
			
				pdfCandy.OpenDefault();
			}
        }
    }
}
