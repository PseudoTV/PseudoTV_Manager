﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form6
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
		Me.Label1 = New System.Windows.Forms.Label()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.TextBox1 = New System.Windows.Forms.TextBox()
		Me.Button1 = New System.Windows.Forms.Button()
		Me.Button2 = New System.Windows.Forms.Button()
		Me.TextBox2 = New System.Windows.Forms.TextBox()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.Button3 = New System.Windows.Forms.Button()
		Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
		Me.TabControl1 = New System.Windows.Forms.TabControl()
		Me.TabPage1 = New System.Windows.Forms.TabPage()
		Me.TabPage2 = New System.Windows.Forms.TabPage()
		Me.Label9 = New System.Windows.Forms.Label()
		Me.TextBox7 = New System.Windows.Forms.TextBox()
		Me.Label8 = New System.Windows.Forms.Label()
		Me.TextBox6 = New System.Windows.Forms.TextBox()
		Me.Label7 = New System.Windows.Forms.Label()
		Me.TextBox5 = New System.Windows.Forms.TextBox()
		Me.Label6 = New System.Windows.Forms.Label()
		Me.TextBox4 = New System.Windows.Forms.TextBox()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.TextBox3 = New System.Windows.Forms.TextBox()
		Me.Button4 = New System.Windows.Forms.Button()
		Me.TextBox8 = New System.Windows.Forms.TextBox()
		Me.AddonDbLocDef = New System.Windows.Forms.Label()
		Me.AddonDbLoc = New System.Windows.Forms.Label()
		Me.XbmcVersion = New System.Windows.Forms.ComboBox()
		Me.XbmcVersionLbl = New System.Windows.Forms.Label()
		Me.TabControl1.SuspendLayout()
		Me.TabPage1.SuspendLayout()
		Me.TabPage2.SuspendLayout()
		Me.SuspendLayout()
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.Location = New System.Drawing.Point(7, 7)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(164, 16)
		Me.Label1.TabIndex = 0
		Me.Label1.Text = "Video Database Location:"
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Location = New System.Drawing.Point(7, 23)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(409, 26)
		Me.Label2.TabIndex = 1
		Me.Label2.Text = "Typically located :" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "C:\Users\Username\AppData\Roaming\XBMC\userdata\Database\MyV" & _
	"ideos78.db "
		'
		'TextBox1
		'
		Me.TextBox1.Location = New System.Drawing.Point(7, 52)
		Me.TextBox1.Name = "TextBox1"
		Me.TextBox1.Size = New System.Drawing.Size(406, 20)
		Me.TextBox1.TabIndex = 2
		'
		'Button1
		'
		Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Button1.Location = New System.Drawing.Point(417, 53)
		Me.Button1.Name = "Button1"
		Me.Button1.Size = New System.Drawing.Size(30, 18)
		Me.Button1.TabIndex = 3
		Me.Button1.Text = "..."
		Me.Button1.UseVisualStyleBackColor = True
		'
		'Button2
		'
		Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Button2.Location = New System.Drawing.Point(423, 325)
		Me.Button2.Name = "Button2"
		Me.Button2.Size = New System.Drawing.Size(30, 18)
		Me.Button2.TabIndex = 7
		Me.Button2.Text = "..."
		Me.Button2.UseVisualStyleBackColor = True
		'
		'TextBox2
		'
		Me.TextBox2.Location = New System.Drawing.Point(11, 325)
		Me.TextBox2.Name = "TextBox2"
		Me.TextBox2.Size = New System.Drawing.Size(406, 20)
		Me.TextBox2.TabIndex = 6
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Location = New System.Drawing.Point(8, 296)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(252, 26)
		Me.Label3.TabIndex = 5
		Me.Label3.Text = "Typically located:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "userdata\addon_data\script.pseudotv\settings2.xml"
		'
		'Label4
		'
		Me.Label4.AutoSize = True
		Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label4.Location = New System.Drawing.Point(8, 280)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(188, 16)
		Me.Label4.TabIndex = 4
		Me.Label4.Text = "PseudoTV Settings2.XML File:"
		'
		'Button3
		'
		Me.Button3.Location = New System.Drawing.Point(185, 419)
		Me.Button3.Name = "Button3"
		Me.Button3.Size = New System.Drawing.Size(75, 23)
		Me.Button3.TabIndex = 8
		Me.Button3.Text = "Save"
		Me.Button3.UseVisualStyleBackColor = True
		'
		'OpenFileDialog1
		'
		Me.OpenFileDialog1.FileName = "OpenFileDialog1"
		'
		'TabControl1
		'
		Me.TabControl1.Controls.Add(Me.TabPage1)
		Me.TabControl1.Controls.Add(Me.TabPage2)
		Me.TabControl1.Location = New System.Drawing.Point(11, 35)
		Me.TabControl1.Name = "TabControl1"
		Me.TabControl1.SelectedIndex = 0
		Me.TabControl1.Size = New System.Drawing.Size(462, 228)
		Me.TabControl1.TabIndex = 9
		'
		'TabPage1
		'
		Me.TabPage1.BackColor = System.Drawing.SystemColors.Control
		Me.TabPage1.Controls.Add(Me.Label2)
		Me.TabPage1.Controls.Add(Me.Label1)
		Me.TabPage1.Controls.Add(Me.TextBox1)
		Me.TabPage1.Controls.Add(Me.Button1)
		Me.TabPage1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.TabPage1.Location = New System.Drawing.Point(4, 22)
		Me.TabPage1.Name = "TabPage1"
		Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
		Me.TabPage1.Size = New System.Drawing.Size(454, 202)
		Me.TabPage1.TabIndex = 0
		Me.TabPage1.Text = "SQLite (Default)"
		'
		'TabPage2
		'
		Me.TabPage2.BackColor = System.Drawing.SystemColors.Control
		Me.TabPage2.Controls.Add(Me.Label9)
		Me.TabPage2.Controls.Add(Me.TextBox7)
		Me.TabPage2.Controls.Add(Me.Label8)
		Me.TabPage2.Controls.Add(Me.TextBox6)
		Me.TabPage2.Controls.Add(Me.Label7)
		Me.TabPage2.Controls.Add(Me.TextBox5)
		Me.TabPage2.Controls.Add(Me.Label6)
		Me.TabPage2.Controls.Add(Me.TextBox4)
		Me.TabPage2.Controls.Add(Me.Label5)
		Me.TabPage2.Controls.Add(Me.TextBox3)
		Me.TabPage2.Location = New System.Drawing.Point(4, 22)
		Me.TabPage2.Name = "TabPage2"
		Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
		Me.TabPage2.Size = New System.Drawing.Size(454, 202)
		Me.TabPage2.TabIndex = 1
		Me.TabPage2.Text = "MySQL"
		'
		'Label9
		'
		Me.Label9.AutoSize = True
		Me.Label9.Location = New System.Drawing.Point(3, 121)
		Me.Label9.Name = "Label9"
		Me.Label9.Size = New System.Drawing.Size(74, 26)
		Me.Label9.TabIndex = 9
		Me.Label9.Text = "Port:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Default 3306)"
		'
		'TextBox7
		'
		Me.TextBox7.Location = New System.Drawing.Point(137, 127)
		Me.TextBox7.Name = "TextBox7"
		Me.TextBox7.Size = New System.Drawing.Size(215, 20)
		Me.TextBox7.TabIndex = 8
		'
		'Label8
		'
		Me.Label8.AutoSize = True
		Me.Label8.Location = New System.Drawing.Point(3, 163)
		Me.Label8.Name = "Label8"
		Me.Label8.Size = New System.Drawing.Size(86, 26)
		Me.Label8.TabIndex = 7
		Me.Label8.Text = "Video Database " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Table Name:"
		Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'TextBox6
		'
		Me.TextBox6.Location = New System.Drawing.Point(137, 167)
		Me.TextBox6.Name = "TextBox6"
		Me.TextBox6.Size = New System.Drawing.Size(215, 20)
		Me.TextBox6.TabIndex = 6
		'
		'Label7
		'
		Me.Label7.AutoSize = True
		Me.Label7.Location = New System.Drawing.Point(3, 87)
		Me.Label7.Name = "Label7"
		Me.Label7.Size = New System.Drawing.Size(56, 13)
		Me.Label7.TabIndex = 5
		Me.Label7.Text = "Password:"
		'
		'TextBox5
		'
		Me.TextBox5.Location = New System.Drawing.Point(137, 84)
		Me.TextBox5.Name = "TextBox5"
		Me.TextBox5.Size = New System.Drawing.Size(215, 20)
		Me.TextBox5.TabIndex = 4
		'
		'Label6
		'
		Me.Label6.AutoSize = True
		Me.Label6.Location = New System.Drawing.Point(3, 51)
		Me.Label6.Name = "Label6"
		Me.Label6.Size = New System.Drawing.Size(58, 13)
		Me.Label6.TabIndex = 3
		Me.Label6.Text = "Username:"
		'
		'TextBox4
		'
		Me.TextBox4.Location = New System.Drawing.Point(137, 48)
		Me.TextBox4.Name = "TextBox4"
		Me.TextBox4.Size = New System.Drawing.Size(215, 20)
		Me.TextBox4.TabIndex = 2
		'
		'Label5
		'
		Me.Label5.AutoSize = True
		Me.Label5.Location = New System.Drawing.Point(3, 13)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(82, 13)
		Me.Label5.TabIndex = 1
		Me.Label5.Text = "Server Address:"
		'
		'TextBox3
		'
		Me.TextBox3.Location = New System.Drawing.Point(137, 10)
		Me.TextBox3.Name = "TextBox3"
		Me.TextBox3.Size = New System.Drawing.Size(215, 20)
		Me.TextBox3.TabIndex = 0
		'
		'Button4
		'
		Me.Button4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Button4.Location = New System.Drawing.Point(423, 395)
		Me.Button4.Name = "Button4"
		Me.Button4.Size = New System.Drawing.Size(30, 18)
		Me.Button4.TabIndex = 13
		Me.Button4.Text = "..."
		Me.Button4.UseVisualStyleBackColor = True
		'
		'TextBox8
		'
		Me.TextBox8.Location = New System.Drawing.Point(11, 393)
		Me.TextBox8.Name = "TextBox8"
		Me.TextBox8.Size = New System.Drawing.Size(406, 20)
		Me.TextBox8.TabIndex = 12
		'
		'AddonDbLocDef
		'
		Me.AddonDbLocDef.AutoSize = True
		Me.AddonDbLocDef.Location = New System.Drawing.Point(8, 364)
		Me.AddonDbLocDef.Name = "AddonDbLocDef"
		Me.AddonDbLocDef.Size = New System.Drawing.Size(399, 26)
		Me.AddonDbLocDef.TabIndex = 11
		Me.AddonDbLocDef.Text = "Typically located :" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "C:\Users\Username\AppData\Roaming\XBMC\userdata\Database\Add" & _
	"ons19.db " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
		'
		'AddonDbLoc
		'
		Me.AddonDbLoc.AutoSize = True
		Me.AddonDbLoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.AddonDbLoc.Location = New System.Drawing.Point(9, 348)
		Me.AddonDbLoc.Name = "AddonDbLoc"
		Me.AddonDbLoc.Size = New System.Drawing.Size(172, 16)
		Me.AddonDbLoc.TabIndex = 10
		Me.AddonDbLoc.Text = "Addons Database Location"
		'
		'XbmcVersion
		'
		Me.XbmcVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.XbmcVersion.FormattingEnabled = True
		Me.XbmcVersion.Items.AddRange(New Object() {"Gotham", "Helix", "Isengard", "Jarvis"})
		Me.XbmcVersion.Location = New System.Drawing.Point(78, 8)
		Me.XbmcVersion.Name = "XbmcVersion"
		Me.XbmcVersion.Size = New System.Drawing.Size(121, 21)
		Me.XbmcVersion.TabIndex = 15
		'
		'XbmcVersionLbl
		'
		Me.XbmcVersionLbl.AutoSize = True
		Me.XbmcVersionLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!)
		Me.XbmcVersionLbl.Location = New System.Drawing.Point(12, 9)
		Me.XbmcVersionLbl.Name = "XbmcVersionLbl"
		Me.XbmcVersionLbl.Size = New System.Drawing.Size(60, 17)
		Me.XbmcVersionLbl.TabIndex = 14
		Me.XbmcVersionLbl.Text = "Version:"
		'
		'Form6
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(484, 453)
		Me.Controls.Add(Me.XbmcVersion)
		Me.Controls.Add(Me.XbmcVersionLbl)
		Me.Controls.Add(Me.Button4)
		Me.Controls.Add(Me.TextBox8)
		Me.Controls.Add(Me.AddonDbLocDef)
		Me.Controls.Add(Me.AddonDbLoc)
		Me.Controls.Add(Me.TabControl1)
		Me.Controls.Add(Me.Button3)
		Me.Controls.Add(Me.Button2)
		Me.Controls.Add(Me.TextBox2)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.Label4)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
		Me.Name = "Form6"
		Me.Text = "Settings"
		Me.TopMost = True
		Me.TabControl1.ResumeLayout(False)
		Me.TabPage1.ResumeLayout(False)
		Me.TabPage1.PerformLayout()
		Me.TabPage2.ResumeLayout(False)
		Me.TabPage2.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TextBox6 As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TextBox7 As System.Windows.Forms.TextBox
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents TextBox8 As System.Windows.Forms.TextBox
    Friend WithEvents AddonDbLocDef As System.Windows.Forms.Label
    Friend WithEvents AddonDbLoc As System.Windows.Forms.Label
    Friend WithEvents XbmcVersion As System.Windows.Forms.ComboBox
    Friend WithEvents XbmcVersionLbl As System.Windows.Forms.Label
End Class
