namespace IGAE_GUI.IGZ
{
	partial class IGZ_GeneralForm
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
            treeItems = new TreeView();
            pbTexturePreview = new PictureBox();
            cbFilterImages = new CheckBox();
            btnSaveAllObjects = new Button();
            btnTextureExtract = new Button();
            btnTextureReplace = new Button();
            extractReplaceDDSLabel = new Label();
            btnTextureExtractDDS = new Button();
            btnTextureReplaceDDS = new Button();
            btnSaveIGZ = new Button();
            ((System.ComponentModel.ISupportInitialize)pbTexturePreview).BeginInit();
            SuspendLayout();
            // 
            // treeItems
            // 
            treeItems.Font = new Font("Courier New", 8F, FontStyle.Regular, GraphicsUnit.Point);
            treeItems.Location = new Point(14, 14);
            treeItems.Margin = new Padding(4, 3, 4, 3);
            treeItems.Name = "treeItems";
            treeItems.Size = new Size(555, 549);
            treeItems.TabIndex = 1;
            treeItems.AfterSelect += SelectionChange;
            // 
            // pbTexturePreview
            // 
            pbTexturePreview.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            pbTexturePreview.BackColor = Color.Black;
            pbTexturePreview.Location = new Point(576, 292);
            pbTexturePreview.Margin = new Padding(4, 3, 4, 3);
            pbTexturePreview.Name = "pbTexturePreview";
            pbTexturePreview.Size = new Size(233, 231);
            pbTexturePreview.SizeMode = PictureBoxSizeMode.Zoom;
            pbTexturePreview.TabIndex = 0;
            pbTexturePreview.TabStop = false;
            // 
            // cbFilterImages
            // 
            cbFilterImages.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cbFilterImages.Checked = true;
            cbFilterImages.CheckState = CheckState.Checked;
            cbFilterImages.Location = new Point(583, 14);
            cbFilterImages.Margin = new Padding(4, 3, 4, 3);
            cbFilterImages.Name = "cbFilterImages";
            cbFilterImages.Size = new Size(117, 27);
            cbFilterImages.TabIndex = 10;
            cbFilterImages.Text = "Only igImage2s?";
            cbFilterImages.CheckedChanged += ChangeFilter;
            // 
            // btnSaveAllObjects
            // 
            btnSaveAllObjects.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSaveAllObjects.Location = new Point(583, 44);
            btnSaveAllObjects.Margin = new Padding(4, 3, 4, 3);
            btnSaveAllObjects.Name = "btnSaveAllObjects";
            btnSaveAllObjects.Size = new Size(88, 27);
            btnSaveAllObjects.TabIndex = 2;
            btnSaveAllObjects.Text = "Save All";
            btnSaveAllObjects.UseVisualStyleBackColor = true;
            btnSaveAllObjects.Click += ExportAllObjects;
            // 
            // btnTextureExtract
            // 
            btnTextureExtract.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnTextureExtract.Location = new Point(614, 537);
            btnTextureExtract.Margin = new Padding(4, 3, 4, 3);
            btnTextureExtract.Name = "btnTextureExtract";
            btnTextureExtract.Size = new Size(88, 27);
            btnTextureExtract.TabIndex = 3;
            btnTextureExtract.Text = "Extract";
            btnTextureExtract.UseVisualStyleBackColor = true;
            btnTextureExtract.Click += ExportTexture;
            // 
            // btnTextureReplace
            // 
            btnTextureReplace.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnTextureReplace.Location = new Point(715, 537);
            btnTextureReplace.Margin = new Padding(4, 3, 4, 3);
            btnTextureReplace.Name = "btnTextureReplace";
            btnTextureReplace.Size = new Size(88, 27);
            btnTextureReplace.TabIndex = 4;
            btnTextureReplace.Text = "Replace";
            btnTextureReplace.UseVisualStyleBackColor = true;
            btnTextureReplace.Click += ImportTexture;
            // 
            // extractReplaceDDSLabel
            // 
            extractReplaceDDSLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            extractReplaceDDSLabel.Location = new Point(583, 97);
            extractReplaceDDSLabel.Margin = new Padding(4, 3, 4, 3);
            extractReplaceDDSLabel.Name = "extractReplaceDDSLabel";
            extractReplaceDDSLabel.Size = new Size(132, 17);
            extractReplaceDDSLabel.TabIndex = 4;
            extractReplaceDDSLabel.Text = "Direct Extract/Replace";
            // 
            // btnTextureExtractDDS
            // 
            btnTextureExtractDDS.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnTextureExtractDDS.Location = new Point(583, 120);
            btnTextureExtractDDS.Margin = new Padding(4, 3, 4, 3);
            btnTextureExtractDDS.Name = "btnTextureExtractDDS";
            btnTextureExtractDDS.Size = new Size(88, 27);
            btnTextureExtractDDS.TabIndex = 4;
            btnTextureExtractDDS.Text = "Extract DDS";
            btnTextureExtractDDS.UseVisualStyleBackColor = true;
            btnTextureExtractDDS.Click += ExportDDSTexture;
            // 
            // btnTextureReplaceDDS
            // 
            btnTextureReplaceDDS.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnTextureReplaceDDS.Location = new Point(679, 120);
            btnTextureReplaceDDS.Margin = new Padding(4, 3, 4, 3);
            btnTextureReplaceDDS.Name = "btnTextureReplaceDDS";
            btnTextureReplaceDDS.Size = new Size(88, 27);
            btnTextureReplaceDDS.TabIndex = 4;
            btnTextureReplaceDDS.Text = "Replace DDS";
            btnTextureReplaceDDS.UseVisualStyleBackColor = true;
            btnTextureReplaceDDS.Click += ImportDDSTexture;
            // 
            // btnSaveIGZ
            // 
            btnSaveIGZ.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveIGZ.Location = new Point(715, 14);
            btnSaveIGZ.Margin = new Padding(4, 3, 4, 3);
            btnSaveIGZ.Name = "btnSaveIGZ";
            btnSaveIGZ.Size = new Size(88, 27);
            btnSaveIGZ.TabIndex = 2;
            btnSaveIGZ.Text = "Save IGZ";
            btnSaveIGZ.UseVisualStyleBackColor = true;
            btnSaveIGZ.Click += Save;
            // 
            // IGZ_GeneralForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(817, 577);
            Controls.Add(treeItems);
            Controls.Add(pbTexturePreview);
            Controls.Add(cbFilterImages);
            Controls.Add(btnSaveAllObjects);
            Controls.Add(btnTextureExtract);
            Controls.Add(btnTextureReplace);
            Controls.Add(extractReplaceDDSLabel);
            Controls.Add(btnTextureExtractDDS);
            Controls.Add(btnTextureReplaceDDS);
            Controls.Add(btnSaveIGZ);
            Margin = new Padding(4, 3, 4, 3);
            Name = "IGZ_GeneralForm";
            Text = "IGZ Viewer";
            FormClosing += Closing;
            ((System.ComponentModel.ISupportInitialize)pbTexturePreview).EndInit();
            ResumeLayout(false);
        }

        #endregion

        public System.Windows.Forms.TreeView treeItems;
		public System.Windows.Forms.PictureBox pbTexturePreview;
		public System.Windows.Forms.CheckBox cbFilterImages;
		public System.Windows.Forms.Button btnSaveAllObjects;
		public System.Windows.Forms.Button btnTextureExtract;
		public System.Windows.Forms.Button btnTextureReplace;
        public System.Windows.Forms.Label extractReplaceDDSLabel;
        public System.Windows.Forms.Button btnTextureExtractDDS;
        public System.Windows.Forms.Button btnTextureReplaceDDS;
        public System.Windows.Forms.Button btnSaveIGZ;
	}
}