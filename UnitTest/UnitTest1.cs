using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pep.AppHandler.CandyStore.OOXML;
using pep.AppHandler.CandyDelivery;

namespace UnitTest
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{

			string connectionString = @"Persist Security Info=False;Integrated Security=SSPI;database=NEHST;server=WIN-TK7VVQF2IT3;Connect Timeout=120";
			//string connectionString = @"Persist Security Info=False;Trusted_Connection=False;database=NEHST;server=WIN-TK7VVQF2IT3;Connect Timeout=120;User ID=developer;password=Secure123";
			

			using( SingleStream singleStream = new SingleStream (new FileStreamer(connectionString), @"FE8F3116-99A4-E411-9403-000C29E5BED1" ) )
			{
				using (XDocument candy = new XDocument(singleStream, FileAccess.ReadWrite))
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.Load(@"Z:\Projects\Hamster2.5\policy.xml");
					candy.AddPolicy(xmlDocument);

					candy.UnloadDocument(@"Z:\Projects\Hamster2.5\hamster_eval.docx");
				}
			}
		}

		[TestMethod]
		public void TestMethod2()
		{
			string connectionString = @"Persist Security Info=False;Integrated Security=SSPI;database=NEHST;server=WIN-TK7VVQF2IT3;Connect Timeout=120";
			//string connectionString = @"Persist Security Info=False;Trusted_Connection=False;database=NEHST;server=WIN-TK7VVQF2IT3;Connect Timeout=120;User ID=developer;password=Secure123";

			using (FileStream fileStream = new FileStream(@"Z:\Projects\Hamster2.5\Hamster2_Ewa.docx", FileMode.Open))
			{
				using (SingleStream singleStream = new SingleStream(new FileStreamer(connectionString),  (Stream)fileStream))
				{
					using (XDocument candy = new XDocument(singleStream, FileAccess.ReadWrite))
					{
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.Load(@"Z:\Projects\Hamster2.5\policy.xml");
						candy.AddPolicy(xmlDocument);
					}
				}
			}
		}
	}
}
