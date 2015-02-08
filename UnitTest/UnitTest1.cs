using System;
using System.Collections.Generic;
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

			//string connectionString = @"Persist Security Info=False;Integrated Security=SSPI;database=NEHST;server=WIN-TK7VVQF2IT3;Connect Timeout=120";
			string connectionString = @"Persist Security Info=False;Trusted_Connection=False;database=NEHST;server=WIN-TK7VVQF2IT3;Connect Timeout=120;User ID=developer;password=Secure123";
			
			using ( Master candy = new Master() )
			{
				using( FileStreamer fileStreamer = new FileStreamer(connectionString) )
				{
					List<SingleStream> streams = new List<SingleStream>();
					streams.Add(new SingleStream(fileStreamer, @"FE8F3116-99A4-E411-9403-000C29E5BED1"));

				}
			}
		}
	}
}
