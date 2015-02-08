namespace pep.AppViewer
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.form_axAcroPDF = new AxAcroPDFLib.AxAcroPDF();
			((System.ComponentModel.ISupportInitialize)(this.form_axAcroPDF)).BeginInit();
			this.SuspendLayout();
			// 
			// form_axAcroPDF
			//
			this.form_axAcroPDF.Enabled = true;
			this.form_axAcroPDF.Location = new System.Drawing.Point(47, 35);
			this.form_axAcroPDF.Name = "form_axAcroPDF"; 
			this.form_axAcroPDF.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("form_axAcroPDF.OcxState")));
			this.form_axAcroPDF.Size = new System.Drawing.Size(770, 289);
			this.form_axAcroPDF.TabIndex = 0;
			this.form_axAcroPDF.OnError += new System.EventHandler(this.axAcroPDF_OnError);
			this.form_axAcroPDF.HandleCreated += new System.EventHandler(this.axAcroPDF_EventCreated);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(856, 336);
			this.Controls.Add(this.form_axAcroPDF);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.form_axAcroPDF)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private AxAcroPDFLib.AxAcroPDF form_axAcroPDF;
	}
}

