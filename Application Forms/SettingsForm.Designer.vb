<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SettingsForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Me.MySqlBox = New System.Windows.Forms.GroupBox()
		Me.Port = New System.Windows.Forms.TextBox()
		Me.Password = New System.Windows.Forms.TextBox()
		Me.Username = New System.Windows.Forms.TextBox()
		Me.Address = New System.Windows.Forms.TextBox()
		Me.PortLbl = New System.Windows.Forms.Label()
		Me.PasswordLbl = New System.Windows.Forms.Label()
		Me.UsernameLbl = New System.Windows.Forms.Label()
		Me.AddressLbl = New System.Windows.Forms.Label()
		Me.SettingsSaveBtn = New System.Windows.Forms.Button()
		Me.BrowseBtn = New System.Windows.Forms.Button()
		Me.UserData = New System.Windows.Forms.TextBox()
		Me.UserDataLbl = New System.Windows.Forms.Label()
		Me.Locations = New System.Windows.Forms.ListBox()
		Me.Button1 = New System.Windows.Forms.Button()
		Me.Button2 = New System.Windows.Forms.Button()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.DatabaseType = New System.Windows.Forms.ComboBox()
		Me.DatabaseTypeLbl = New System.Windows.Forms.Label()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.XbmcVersion = New System.Windows.Forms.ComboBox()
		Me.XbmcVersionLbl = New System.Windows.Forms.Label()
		Me.MySqlBox.SuspendLayout()
		Me.SuspendLayout()
		'
		'MySqlBox
		'
		Me.MySqlBox.Controls.Add(Me.Port)
		Me.MySqlBox.Controls.Add(Me.Password)
		Me.MySqlBox.Controls.Add(Me.Username)
		Me.MySqlBox.Controls.Add(Me.Address)
		Me.MySqlBox.Controls.Add(Me.PortLbl)
		Me.MySqlBox.Controls.Add(Me.PasswordLbl)
		Me.MySqlBox.Controls.Add(Me.UsernameLbl)
		Me.MySqlBox.Controls.Add(Me.AddressLbl)
		Me.MySqlBox.Location = New System.Drawing.Point(15, 30)
		Me.MySqlBox.Name = "MySqlBox"
		Me.MySqlBox.Size = New System.Drawing.Size(363, 134)
		Me.MySqlBox.TabIndex = 8
		Me.MySqlBox.TabStop = False
		Me.MySqlBox.Text = "MySql"
		'
		'Port
		'
		Me.Port.Location = New System.Drawing.Point(124, 99)
		Me.Port.Name = "Port"
		Me.Port.Size = New System.Drawing.Size(229, 20)
		Me.Port.TabIndex = 7
		'
		'Password
		'
		Me.Password.Location = New System.Drawing.Point(124, 73)
		Me.Password.Name = "Password"
		Me.Password.Size = New System.Drawing.Size(229, 20)
		Me.Password.TabIndex = 6
		'
		'Username
		'
		Me.Username.Location = New System.Drawing.Point(124, 48)
		Me.Username.Name = "Username"
		Me.Username.Size = New System.Drawing.Size(229, 20)
		Me.Username.TabIndex = 5
		'
		'Address
		'
		Me.Address.Location = New System.Drawing.Point(124, 20)
		Me.Address.Name = "Address"
		Me.Address.Size = New System.Drawing.Size(229, 20)
		Me.Address.TabIndex = 4
		'
		'PortLbl
		'
		Me.PortLbl.AutoSize = True
		Me.PortLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!)
		Me.PortLbl.Location = New System.Drawing.Point(6, 103)
		Me.PortLbl.Name = "PortLbl"
		Me.PortLbl.Size = New System.Drawing.Size(38, 17)
		Me.PortLbl.TabIndex = 3
		Me.PortLbl.Text = "Port:"
		'
		'PasswordLbl
		'
		Me.PasswordLbl.AutoSize = True
		Me.PasswordLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!)
		Me.PasswordLbl.Location = New System.Drawing.Point(7, 76)
		Me.PasswordLbl.Name = "PasswordLbl"
		Me.PasswordLbl.Size = New System.Drawing.Size(73, 17)
		Me.PasswordLbl.TabIndex = 2
		Me.PasswordLbl.Text = "Password:"
		'
		'UsernameLbl
		'
		Me.UsernameLbl.AutoSize = True
		Me.UsernameLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!)
		Me.UsernameLbl.Location = New System.Drawing.Point(7, 48)
		Me.UsernameLbl.Name = "UsernameLbl"
		Me.UsernameLbl.Size = New System.Drawing.Size(77, 17)
		Me.UsernameLbl.TabIndex = 1
		Me.UsernameLbl.Text = "Username:"
		'
		'AddressLbl
		'
		Me.AddressLbl.AutoSize = True
		Me.AddressLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!)
		Me.AddressLbl.Location = New System.Drawing.Point(7, 20)
		Me.AddressLbl.Name = "AddressLbl"
		Me.AddressLbl.Size = New System.Drawing.Size(110, 17)
		Me.AddressLbl.TabIndex = 0
		Me.AddressLbl.Text = "Server Address:"
		'
		'SettingsSaveBtn
		'
		Me.SettingsSaveBtn.Location = New System.Drawing.Point(442, 165)
		Me.SettingsSaveBtn.Name = "SettingsSaveBtn"
		Me.SettingsSaveBtn.Size = New System.Drawing.Size(51, 23)
		Me.SettingsSaveBtn.TabIndex = 19
		Me.SettingsSaveBtn.Text = "Save"
		Me.SettingsSaveBtn.UseVisualStyleBackColor = True
		'
		'BrowseBtn
		'
		Me.BrowseBtn.Location = New System.Drawing.Point(385, 165)
		Me.BrowseBtn.Name = "BrowseBtn"
		Me.BrowseBtn.Size = New System.Drawing.Size(51, 23)
		Me.BrowseBtn.TabIndex = 18
		Me.BrowseBtn.Text = "Browse"
		Me.BrowseBtn.UseVisualStyleBackColor = True
		'
		'UserData
		'
		Me.UserData.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.PseudoTV_Manager.My.MySettings.Default, "UserDataFolder", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
		Me.UserData.Location = New System.Drawing.Point(139, 167)
		Me.UserData.Name = "UserData"
		Me.UserData.Size = New System.Drawing.Size(229, 20)
		Me.UserData.TabIndex = 17
		'
		'UserDataLbl
		'
		Me.UserDataLbl.AutoSize = True
		Me.UserDataLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!)
		Me.UserDataLbl.Location = New System.Drawing.Point(12, 167)
		Me.UserDataLbl.Name = "UserDataLbl"
		Me.UserDataLbl.Size = New System.Drawing.Size(116, 17)
		Me.UserDataLbl.TabIndex = 16
		Me.UserDataLbl.Text = "UserData Folder:"
		'
		'Locations
		'
		Me.Locations.FormattingEnabled = True
		Me.Locations.Items.AddRange(New Object() {"Default"})
		Me.Locations.Location = New System.Drawing.Point(385, 36)
		Me.Locations.Name = "Locations"
		Me.Locations.Size = New System.Drawing.Size(96, 43)
		Me.Locations.TabIndex = 20
		'
		'Button1
		'
		Me.Button1.Location = New System.Drawing.Point(384, 85)
		Me.Button1.Name = "Button1"
		Me.Button1.Size = New System.Drawing.Size(38, 23)
		Me.Button1.TabIndex = 21
		Me.Button1.Text = "Add"
		Me.Button1.UseVisualStyleBackColor = True
		'
		'Button2
		'
		Me.Button2.Location = New System.Drawing.Point(442, 86)
		Me.Button2.Name = "Button2"
		Me.Button2.Size = New System.Drawing.Size(38, 23)
		Me.Button2.TabIndex = 22
		Me.Button2.Text = "Del"
		Me.Button2.UseVisualStyleBackColor = True
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(12, 203)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(398, 13)
		Me.Label1.TabIndex = 23
		Me.Label1.Text = "Once your version is set, and you click browse, this should find your userdata fo" & _
	"lder"
		'
		'DatabaseType
		'
		Me.DatabaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.DatabaseType.FormattingEnabled = True
		Me.DatabaseType.Items.AddRange(New Object() {"SQLite", "MySQL"})
		Me.DatabaseType.Location = New System.Drawing.Point(384, 8)
		Me.DatabaseType.Name = "DatabaseType"
		Me.DatabaseType.Size = New System.Drawing.Size(65, 21)
		Me.DatabaseType.TabIndex = 7
		'
		'DatabaseTypeLbl
		'
		Me.DatabaseTypeLbl.AutoSize = True
		Me.DatabaseTypeLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!)
		Me.DatabaseTypeLbl.Location = New System.Drawing.Point(269, 8)
		Me.DatabaseTypeLbl.Name = "DatabaseTypeLbl"
		Me.DatabaseTypeLbl.Size = New System.Drawing.Size(109, 17)
		Me.DatabaseTypeLbl.TabIndex = 6
		Me.DatabaseTypeLbl.Text = "Database Type:"
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Location = New System.Drawing.Point(12, 216)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(139, 13)
		Me.Label2.TabIndex = 24
		Me.Label2.Text = "for xbmc/kodi automatically."
		'
		'XbmcVersion
		'
		Me.XbmcVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.XbmcVersion.FormattingEnabled = True
		Me.XbmcVersion.Items.AddRange(New Object() {"Gotham", "Helix", "Isengard", "Jarvis"})
		Me.XbmcVersion.Location = New System.Drawing.Point(78, 8)
		Me.XbmcVersion.Name = "XbmcVersion"
		Me.XbmcVersion.Size = New System.Drawing.Size(121, 21)
		Me.XbmcVersion.TabIndex = 5
		'
		'XbmcVersionLbl
		'
		Me.XbmcVersionLbl.AutoSize = True
		Me.XbmcVersionLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!)
		Me.XbmcVersionLbl.Location = New System.Drawing.Point(12, 9)
		Me.XbmcVersionLbl.Name = "XbmcVersionLbl"
		Me.XbmcVersionLbl.Size = New System.Drawing.Size(60, 17)
		Me.XbmcVersionLbl.TabIndex = 4
		Me.XbmcVersionLbl.Text = "Version:"
		'
		'SettingsForm
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(502, 258)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.Button2)
		Me.Controls.Add(Me.Button1)
		Me.Controls.Add(Me.Locations)
		Me.Controls.Add(Me.SettingsSaveBtn)
		Me.Controls.Add(Me.BrowseBtn)
		Me.Controls.Add(Me.UserData)
		Me.Controls.Add(Me.UserDataLbl)
		Me.Controls.Add(Me.MySqlBox)
		Me.Controls.Add(Me.DatabaseType)
		Me.Controls.Add(Me.DatabaseTypeLbl)
		Me.Controls.Add(Me.XbmcVersion)
		Me.Controls.Add(Me.XbmcVersionLbl)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
		Me.Name = "SettingsForm"
		Me.Text = "Settings"
		Me.TopMost = True
		Me.MySqlBox.ResumeLayout(False)
		Me.MySqlBox.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
    Friend WithEvents MySqlBox As System.Windows.Forms.GroupBox
    Friend WithEvents Port As System.Windows.Forms.TextBox
    Friend WithEvents Password As System.Windows.Forms.TextBox
    Friend WithEvents Username As System.Windows.Forms.TextBox
    Friend WithEvents Address As System.Windows.Forms.TextBox
    Friend WithEvents PortLbl As System.Windows.Forms.Label
    Friend WithEvents PasswordLbl As System.Windows.Forms.Label
    Friend WithEvents UsernameLbl As System.Windows.Forms.Label
    Friend WithEvents AddressLbl As System.Windows.Forms.Label
    Friend WithEvents SettingsSaveBtn As System.Windows.Forms.Button
    Friend WithEvents BrowseBtn As System.Windows.Forms.Button
    Friend WithEvents UserData As System.Windows.Forms.TextBox
    Friend WithEvents UserDataLbl As System.Windows.Forms.Label
    Friend WithEvents Locations As System.Windows.Forms.ListBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DatabaseType As System.Windows.Forms.ComboBox
    Friend WithEvents DatabaseTypeLbl As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents XbmcVersion As System.Windows.Forms.ComboBox
    Friend WithEvents XbmcVersionLbl As System.Windows.Forms.Label
End Class
