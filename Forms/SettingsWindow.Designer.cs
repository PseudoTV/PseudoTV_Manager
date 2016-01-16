namespace PseudoTV_Manager.Forms
{
    partial class SettingsWindow
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
            this.XbmcVersion = new System.Windows.Forms.ComboBox();
            this.XbmcVersionLbl = new System.Windows.Forms.Label();
            this.BtnAddonsDbLocationBrowse = new System.Windows.Forms.Button();
            this.TxtAddonDatabaseLocation = new System.Windows.Forms.TextBox();
            this.AddonDbLocDef = new System.Windows.Forms.Label();
            this.AddonDbLoc = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.TxtVideoDbLocation = new System.Windows.Forms.TextBox();
            this.BtnVideoDbLocationBrowse = new System.Windows.Forms.Button();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.TxtMySqlPort = new System.Windows.Forms.TextBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.TxtMySqlDatabase = new System.Windows.Forms.TextBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.TxtMySqlPassword = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.TxtMySqlUserId = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.TxtMySqlServer = new System.Windows.Forms.TextBox();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.BtnPseudoTvSettingsLocationBrowse = new System.Windows.Forms.Button();
            this.OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.TxtPseudoTvSettingsLocation = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.BtnSaveSettings = new System.Windows.Forms.Button();
            this.TabPage1.SuspendLayout();
            this.TabPage2.SuspendLayout();
            this.TabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // XbmcVersion
            // 
            this.XbmcVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.XbmcVersion.FormattingEnabled = true;
            this.XbmcVersion.Items.AddRange(new object[] {
            "Gotham",
            "Helix",
            "Isengard",
            "Jarvis"});
            this.XbmcVersion.Location = new System.Drawing.Point(80, 9);
            this.XbmcVersion.Name = "XbmcVersion";
            this.XbmcVersion.Size = new System.Drawing.Size(121, 21);
            this.XbmcVersion.TabIndex = 27;
            // 
            // XbmcVersionLbl
            // 
            this.XbmcVersionLbl.AutoSize = true;
            this.XbmcVersionLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.XbmcVersionLbl.Location = new System.Drawing.Point(14, 10);
            this.XbmcVersionLbl.Name = "XbmcVersionLbl";
            this.XbmcVersionLbl.Size = new System.Drawing.Size(60, 17);
            this.XbmcVersionLbl.TabIndex = 26;
            this.XbmcVersionLbl.Text = "Version:";
            // 
            // BtnAddonsDbLocationBrowse
            // 
            this.BtnAddonsDbLocationBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAddonsDbLocationBrowse.Location = new System.Drawing.Point(425, 396);
            this.BtnAddonsDbLocationBrowse.Name = "BtnAddonsDbLocationBrowse";
            this.BtnAddonsDbLocationBrowse.Size = new System.Drawing.Size(30, 18);
            this.BtnAddonsDbLocationBrowse.TabIndex = 25;
            this.BtnAddonsDbLocationBrowse.Text = "...";
            this.BtnAddonsDbLocationBrowse.UseVisualStyleBackColor = true;
            this.BtnAddonsDbLocationBrowse.Click += new System.EventHandler(this.BtnAddonDbLocationBrowse_Click);
            // 
            // TxtAddonDatabaseLocation
            // 
            this.TxtAddonDatabaseLocation.Location = new System.Drawing.Point(13, 394);
            this.TxtAddonDatabaseLocation.Name = "TxtAddonDatabaseLocation";
            this.TxtAddonDatabaseLocation.Size = new System.Drawing.Size(406, 20);
            this.TxtAddonDatabaseLocation.TabIndex = 24;
            // 
            // AddonDbLocDef
            // 
            this.AddonDbLocDef.AutoSize = true;
            this.AddonDbLocDef.Location = new System.Drawing.Point(10, 365);
            this.AddonDbLocDef.Name = "AddonDbLocDef";
            this.AddonDbLocDef.Size = new System.Drawing.Size(399, 26);
            this.AddonDbLocDef.TabIndex = 23;
            this.AddonDbLocDef.Text = "Typically located :\r\nC:\\Users\\Username\\AppData\\Roaming\\XBMC\\userdata\\Database\\Add" +
    "ons19.db \r\n";
            // 
            // AddonDbLoc
            // 
            this.AddonDbLoc.AutoSize = true;
            this.AddonDbLoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddonDbLoc.Location = new System.Drawing.Point(11, 349);
            this.AddonDbLoc.Name = "AddonDbLoc";
            this.AddonDbLoc.Size = new System.Drawing.Size(172, 16);
            this.AddonDbLoc.TabIndex = 22;
            this.AddonDbLoc.Text = "Addons Database Location";
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Location = new System.Drawing.Point(3, 121);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(74, 26);
            this.Label9.TabIndex = 9;
            this.Label9.Text = "Port:\r\n(Default 3306)";
            // 
            // TabPage1
            // 
            this.TabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.TabPage1.Controls.Add(this.Label2);
            this.TabPage1.Controls.Add(this.Label1);
            this.TabPage1.Controls.Add(this.TxtVideoDbLocation);
            this.TabPage1.Controls.Add(this.BtnVideoDbLocationBrowse);
            this.TabPage1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TabPage1.Location = new System.Drawing.Point(4, 22);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage1.Size = new System.Drawing.Size(454, 202);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "SQLite (Default)";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(7, 23);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(409, 26);
            this.Label2.TabIndex = 1;
            this.Label2.Text = "Typically located :\r\nC:\\Users\\Username\\AppData\\Roaming\\XBMC\\userdata\\Database\\MyV" +
    "ideos78.db ";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(7, 7);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(164, 16);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Video Database Location:";
            // 
            // TxtVideoDbLocation
            // 
            this.TxtVideoDbLocation.Location = new System.Drawing.Point(7, 52);
            this.TxtVideoDbLocation.Name = "TxtVideoDbLocation";
            this.TxtVideoDbLocation.Size = new System.Drawing.Size(406, 20);
            this.TxtVideoDbLocation.TabIndex = 2;
            // 
            // BtnVideoDbLocationBrowse
            // 
            this.BtnVideoDbLocationBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnVideoDbLocationBrowse.Location = new System.Drawing.Point(417, 53);
            this.BtnVideoDbLocationBrowse.Name = "BtnVideoDbLocationBrowse";
            this.BtnVideoDbLocationBrowse.Size = new System.Drawing.Size(30, 18);
            this.BtnVideoDbLocationBrowse.TabIndex = 3;
            this.BtnVideoDbLocationBrowse.Text = "...";
            this.BtnVideoDbLocationBrowse.UseVisualStyleBackColor = true;
            this.BtnVideoDbLocationBrowse.Click += new System.EventHandler(this.BtnVideoDbLocationBrowse_Click);
            // 
            // TabPage2
            // 
            this.TabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.TabPage2.Controls.Add(this.Label9);
            this.TabPage2.Controls.Add(this.TxtMySqlPort);
            this.TabPage2.Controls.Add(this.Label8);
            this.TabPage2.Controls.Add(this.TxtMySqlDatabase);
            this.TabPage2.Controls.Add(this.Label7);
            this.TabPage2.Controls.Add(this.TxtMySqlPassword);
            this.TabPage2.Controls.Add(this.Label6);
            this.TabPage2.Controls.Add(this.TxtMySqlUserId);
            this.TabPage2.Controls.Add(this.Label5);
            this.TabPage2.Controls.Add(this.TxtMySqlServer);
            this.TabPage2.Location = new System.Drawing.Point(4, 22);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage2.Size = new System.Drawing.Size(454, 202);
            this.TabPage2.TabIndex = 1;
            this.TabPage2.Text = "MySQL";
            // 
            // TxtMySqlPort
            // 
            this.TxtMySqlPort.Location = new System.Drawing.Point(137, 127);
            this.TxtMySqlPort.Name = "TxtMySqlPort";
            this.TxtMySqlPort.Size = new System.Drawing.Size(215, 20);
            this.TxtMySqlPort.TabIndex = 8;
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Location = new System.Drawing.Point(3, 163);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(86, 26);
            this.Label8.TabIndex = 7;
            this.Label8.Text = "Video Database \r\nTable Name:";
            this.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TxtMySqlDatabase
            // 
            this.TxtMySqlDatabase.Location = new System.Drawing.Point(137, 167);
            this.TxtMySqlDatabase.Name = "TxtMySqlDatabase";
            this.TxtMySqlDatabase.Size = new System.Drawing.Size(215, 20);
            this.TxtMySqlDatabase.TabIndex = 6;
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(3, 87);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(56, 13);
            this.Label7.TabIndex = 5;
            this.Label7.Text = "Password:";
            // 
            // TxtMySqlPassword
            // 
            this.TxtMySqlPassword.Location = new System.Drawing.Point(137, 84);
            this.TxtMySqlPassword.Name = "TxtMySqlPassword";
            this.TxtMySqlPassword.Size = new System.Drawing.Size(215, 20);
            this.TxtMySqlPassword.TabIndex = 4;
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(3, 51);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(58, 13);
            this.Label6.TabIndex = 3;
            this.Label6.Text = "Username:";
            // 
            // TxtMySqlUserId
            // 
            this.TxtMySqlUserId.Location = new System.Drawing.Point(137, 48);
            this.TxtMySqlUserId.Name = "TxtMySqlUserId";
            this.TxtMySqlUserId.Size = new System.Drawing.Size(215, 20);
            this.TxtMySqlUserId.TabIndex = 2;
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(3, 13);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(82, 13);
            this.Label5.TabIndex = 1;
            this.Label5.Text = "Server Address:";
            // 
            // TxtMySqlServer
            // 
            this.TxtMySqlServer.Location = new System.Drawing.Point(137, 10);
            this.TxtMySqlServer.Name = "TxtMySqlServer";
            this.TxtMySqlServer.Size = new System.Drawing.Size(215, 20);
            this.TxtMySqlServer.TabIndex = 0;
            // 
            // TabControl1
            // 
            this.TabControl1.Controls.Add(this.TabPage1);
            this.TabControl1.Controls.Add(this.TabPage2);
            this.TabControl1.Location = new System.Drawing.Point(13, 36);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(462, 228);
            this.TabControl1.TabIndex = 21;
            // 
            // BtnPseudoTvSettingsLocationBrowse
            // 
            this.BtnPseudoTvSettingsLocationBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPseudoTvSettingsLocationBrowse.Location = new System.Drawing.Point(425, 326);
            this.BtnPseudoTvSettingsLocationBrowse.Name = "BtnPseudoTvSettingsLocationBrowse";
            this.BtnPseudoTvSettingsLocationBrowse.Size = new System.Drawing.Size(30, 18);
            this.BtnPseudoTvSettingsLocationBrowse.TabIndex = 19;
            this.BtnPseudoTvSettingsLocationBrowse.Text = "...";
            this.BtnPseudoTvSettingsLocationBrowse.UseVisualStyleBackColor = true;
            this.BtnPseudoTvSettingsLocationBrowse.Click += new System.EventHandler(this.BtnPseudoTvSettingsLocationBrowse_Click);
            // 
            // OpenFileDialog1
            // 
            this.OpenFileDialog1.FileName = "OpenFileDialog1";
            // 
            // TxtPseudoTvSettingsLocation
            // 
            this.TxtPseudoTvSettingsLocation.Location = new System.Drawing.Point(13, 326);
            this.TxtPseudoTvSettingsLocation.Name = "TxtPseudoTvSettingsLocation";
            this.TxtPseudoTvSettingsLocation.Size = new System.Drawing.Size(406, 20);
            this.TxtPseudoTvSettingsLocation.TabIndex = 18;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(10, 297);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(252, 26);
            this.Label3.TabIndex = 17;
            this.Label3.Text = "Typically located:\r\nuserdata\\addon_data\\script.pseudotv\\settings2.xml";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.Location = new System.Drawing.Point(10, 281);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(188, 16);
            this.Label4.TabIndex = 16;
            this.Label4.Text = "PseudoTV Settings2.XML File:";
            // 
            // BtnSaveSettings
            // 
            this.BtnSaveSettings.Location = new System.Drawing.Point(187, 420);
            this.BtnSaveSettings.Name = "BtnSaveSettings";
            this.BtnSaveSettings.Size = new System.Drawing.Size(75, 23);
            this.BtnSaveSettings.TabIndex = 20;
            this.BtnSaveSettings.Text = "Save";
            this.BtnSaveSettings.UseVisualStyleBackColor = true;
            this.BtnSaveSettings.Click += new System.EventHandler(this.SaveSettings_Click);
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 453);
            this.Controls.Add(this.XbmcVersion);
            this.Controls.Add(this.XbmcVersionLbl);
            this.Controls.Add(this.BtnAddonsDbLocationBrowse);
            this.Controls.Add(this.TxtAddonDatabaseLocation);
            this.Controls.Add(this.AddonDbLocDef);
            this.Controls.Add(this.AddonDbLoc);
            this.Controls.Add(this.TabControl1);
            this.Controls.Add(this.BtnPseudoTvSettingsLocationBrowse);
            this.Controls.Add(this.TxtPseudoTvSettingsLocation);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.BtnSaveSettings);
            this.Name = "SettingsWindow";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsWindow_Load);
            this.TabPage1.ResumeLayout(false);
            this.TabPage1.PerformLayout();
            this.TabPage2.ResumeLayout(false);
            this.TabPage2.PerformLayout();
            this.TabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ComboBox XbmcVersion;
        internal System.Windows.Forms.Label XbmcVersionLbl;
        internal System.Windows.Forms.Button BtnAddonsDbLocationBrowse;
        internal System.Windows.Forms.TextBox TxtAddonDatabaseLocation;
        internal System.Windows.Forms.Label AddonDbLocDef;
        internal System.Windows.Forms.Label AddonDbLoc;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.TabPage TabPage1;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox TxtVideoDbLocation;
        internal System.Windows.Forms.Button BtnVideoDbLocationBrowse;
        internal System.Windows.Forms.TabPage TabPage2;
        internal System.Windows.Forms.TextBox TxtMySqlPort;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.TextBox TxtMySqlDatabase;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.TextBox TxtMySqlPassword;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.TextBox TxtMySqlUserId;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.TextBox TxtMySqlServer;
        internal System.Windows.Forms.TabControl TabControl1;
        internal System.Windows.Forms.Button BtnPseudoTvSettingsLocationBrowse;
        internal System.Windows.Forms.OpenFileDialog OpenFileDialog1;
        internal System.Windows.Forms.TextBox TxtPseudoTvSettingsLocation;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Button BtnSaveSettings;
    }
}