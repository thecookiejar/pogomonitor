namespace PokeMonitor
{
    partial class PokeForm
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
            this.bnMain = new System.Windows.Forms.Button();
            this.listSpawns = new System.Windows.Forms.ListBox();
            this.btnReset = new System.Windows.Forms.WebBrowser();
            this.bnReset = new System.Windows.Forms.Button();
            this.listPokedex = new System.Windows.Forms.CheckedListBox();
            this.lblProgress = new System.Windows.Forms.Label();
            this.bnClear = new System.Windows.Forms.Button();
            this.bnOthers = new System.Windows.Forms.Button();
            this.btnGPX = new System.Windows.Forms.Button();
            this.cbxNotification = new System.Windows.Forms.CheckBox();
            this.cbxSGMapOffset = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // bnMain
            // 
            this.bnMain.Location = new System.Drawing.Point(822, 26);
            this.bnMain.Name = "bnMain";
            this.bnMain.Size = new System.Drawing.Size(466, 28);
            this.bnMain.TabIndex = 0;
            this.bnMain.Text = "Start";
            this.bnMain.UseVisualStyleBackColor = true;
            this.bnMain.Click += new System.EventHandler(this.bnMain_Click);
            // 
            // listSpawns
            // 
            this.listSpawns.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listSpawns.FormattingEnabled = true;
            this.listSpawns.HorizontalScrollbar = true;
            this.listSpawns.ItemHeight = 15;
            this.listSpawns.Location = new System.Drawing.Point(822, 60);
            this.listSpawns.Name = "listSpawns";
            this.listSpawns.Size = new System.Drawing.Size(466, 259);
            this.listSpawns.TabIndex = 1;
            this.listSpawns.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pokeList_MouseClick);
            this.listSpawns.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pokeList_MouseDoubleClick);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(-499, 5);
            this.btnReset.MinimumSize = new System.Drawing.Size(20, 20);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(1315, 772);
            this.btnReset.TabIndex = 2;
            // 
            // bnReset
            // 
            this.bnReset.Location = new System.Drawing.Point(824, 348);
            this.bnReset.Name = "bnReset";
            this.bnReset.Size = new System.Drawing.Size(101, 29);
            this.bnReset.TabIndex = 5;
            this.bnReset.Text = "Reset";
            this.bnReset.UseVisualStyleBackColor = true;
            this.bnReset.Click += new System.EventHandler(this.bnReset_Click);
            // 
            // listPokedex
            // 
            this.listPokedex.CheckOnClick = true;
            this.listPokedex.FormattingEnabled = true;
            this.listPokedex.Location = new System.Drawing.Point(825, 382);
            this.listPokedex.Name = "listPokedex";
            this.listPokedex.Size = new System.Drawing.Size(463, 349);
            this.listPokedex.TabIndex = 6;
            this.listPokedex.SelectedIndexChanged += new System.EventHandler(this.listPokedex_SelectedIndexChanged);
            // 
            // lblProgress
            // 
            this.lblProgress.BackColor = System.Drawing.SystemColors.ControlDark;
            this.lblProgress.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblProgress.Location = new System.Drawing.Point(822, 322);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(466, 23);
            this.lblProgress.TabIndex = 7;
            // 
            // bnClear
            // 
            this.bnClear.Location = new System.Drawing.Point(1026, 348);
            this.bnClear.Name = "bnClear";
            this.bnClear.Size = new System.Drawing.Size(68, 29);
            this.bnClear.TabIndex = 8;
            this.bnClear.Text = "Clear";
            this.bnClear.UseVisualStyleBackColor = true;
            this.bnClear.Click += new System.EventHandler(this.bnClear_Click);
            // 
            // bnOthers
            // 
            this.bnOthers.Location = new System.Drawing.Point(931, 348);
            this.bnOthers.Name = "bnOthers";
            this.bnOthers.Size = new System.Drawing.Size(89, 29);
            this.bnOthers.TabIndex = 9;
            this.bnOthers.Text = "Others";
            this.bnOthers.UseVisualStyleBackColor = true;
            this.bnOthers.Click += new System.EventHandler(this.bnOthers_Click);
            // 
            // btnGPX
            // 
            this.btnGPX.Location = new System.Drawing.Point(825, 737);
            this.btnGPX.Name = "btnGPX";
            this.btnGPX.Size = new System.Drawing.Size(166, 39);
            this.btnGPX.TabIndex = 10;
            this.btnGPX.Text = "Generate GPX";
            this.btnGPX.UseVisualStyleBackColor = true;
            this.btnGPX.Click += new System.EventHandler(this.btnGPX_Click);
            // 
            // cbxNotification
            // 
            this.cbxNotification.AutoSize = true;
            this.cbxNotification.Location = new System.Drawing.Point(822, 5);
            this.cbxNotification.Name = "cbxNotification";
            this.cbxNotification.Size = new System.Drawing.Size(115, 17);
            this.cbxNotification.TabIndex = 11;
            this.cbxNotification.Text = "Enable Notification";
            this.cbxNotification.UseVisualStyleBackColor = true;
            this.cbxNotification.CheckedChanged += new System.EventHandler(this.cbxNotification_CheckedChanged);
            // 
            // cbxSGMapOffset
            // 
            this.cbxSGMapOffset.AutoSize = true;
            this.cbxSGMapOffset.Checked = true;
            this.cbxSGMapOffset.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxSGMapOffset.Location = new System.Drawing.Point(953, 5);
            this.cbxSGMapOffset.Name = "cbxSGMapOffset";
            this.cbxSGMapOffset.Size = new System.Drawing.Size(115, 17);
            this.cbxSGMapOffset.TabIndex = 12;
            this.cbxSGMapOffset.Text = "Has SGMap Offset";
            this.cbxSGMapOffset.UseVisualStyleBackColor = true;
            this.cbxSGMapOffset.CheckedChanged += new System.EventHandler(this.cbxSGMapToggle_CheckedChanged);
            // 
            // PokeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1292, 785);
            this.Controls.Add(this.cbxSGMapOffset);
            this.Controls.Add(this.cbxNotification);
            this.Controls.Add(this.btnGPX);
            this.Controls.Add(this.bnOthers);
            this.Controls.Add(this.bnClear);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.listPokedex);
            this.Controls.Add(this.bnReset);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.listSpawns);
            this.Controls.Add(this.bnMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "PokeForm";
            this.Text = "Pokemon Monitor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bnMain;
        private System.Windows.Forms.ListBox listSpawns;
        private System.Windows.Forms.WebBrowser btnReset;
        private System.Windows.Forms.Button bnReset;
        private System.Windows.Forms.CheckedListBox listPokedex;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Button bnClear;
        private System.Windows.Forms.Button bnOthers;
        private System.Windows.Forms.Button btnGPX;
        private System.Windows.Forms.CheckBox cbxNotification;
        private System.Windows.Forms.CheckBox cbxSGMapOffset;
    }
}

