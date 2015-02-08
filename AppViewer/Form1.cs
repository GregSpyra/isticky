using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppViewer
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void axAcroPDF_OnError(object sender, EventArgs e)
		{
		}
		private void axAcroPDF_EventCreated(object sender, EventArgs e)
		{
			this.form_axAcroPDF.setShowToolbar(false);
			this.form_axAcroPDF.setView("FitH");
			//this.form_axAcroPDF.sho
			this.form_axAcroPDF.src = @"C:\Projects\iSticky\0bb6990f-8787-4843-ab96-1b13b1a9dcba.PDF";
		}
	}
}
