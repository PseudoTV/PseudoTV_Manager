<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form10
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
		Me.AddPosterPictureBox = New System.Windows.Forms.PictureBox()
		Me.AddPosterUrl = New System.Windows.Forms.TextBox()
		Me.AddPosterViewButton = New System.Windows.Forms.Button()
		Me.AddPosterAddButton = New System.Windows.Forms.Button()
		Me.AddPosterCloseButton = New System.Windows.Forms.Button()
		Me.AddPosterInfo = New System.Windows.Forms.Label()
		Me.AddPosterStatus = New System.Windows.Forms.Label()
		Me.AddPosterReturnString = New System.Windows.Forms.TextBox()
		CType(Me.AddPosterPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'AddPosterPictureBox
		'
		Me.AddPosterPictureBox.Location = New System.Drawing.Point(12, 72)
		Me.AddPosterPictureBox.Name = "AddPosterPictureBox"
		Me.AddPosterPictureBox.Size = New System.Drawing.Size(316, 519)
		Me.AddPosterPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.AddPosterPictureBox.TabIndex = 0
		Me.AddPosterPictureBox.TabStop = False
		'
		'AddPosterUrl
		'
		Me.AddPosterUrl.Location = New System.Drawing.Point(12, 12)
		Me.AddPosterUrl.Name = "AddPosterUrl"
		Me.AddPosterUrl.Size = New System.Drawing.Size(316, 20)
		Me.AddPosterUrl.TabIndex = 1
		'
		'AddPosterViewButton
		'
		Me.AddPosterViewButton.Location = New System.Drawing.Point(12, 38)
		Me.AddPosterViewButton.Name = "AddPosterViewButton"
		Me.AddPosterViewButton.Size = New System.Drawing.Size(44, 23)
		Me.AddPosterViewButton.TabIndex = 2
		Me.AddPosterViewButton.Text = "View"
		Me.AddPosterViewButton.UseVisualStyleBackColor = True
		'
		'AddPosterAddButton
		'
		Me.AddPosterAddButton.Location = New System.Drawing.Point(234, 38)
		Me.AddPosterAddButton.Name = "AddPosterAddButton"
		Me.AddPosterAddButton.Size = New System.Drawing.Size(44, 23)
		Me.AddPosterAddButton.TabIndex = 3
		Me.AddPosterAddButton.Text = "Add"
		Me.AddPosterAddButton.UseVisualStyleBackColor = True
		Me.AddPosterAddButton.Visible = False
		'
		'AddPosterCloseButton
		'
		Me.AddPosterCloseButton.Location = New System.Drawing.Point(284, 38)
		Me.AddPosterCloseButton.Name = "AddPosterCloseButton"
		Me.AddPosterCloseButton.Size = New System.Drawing.Size(44, 23)
		Me.AddPosterCloseButton.TabIndex = 4
		Me.AddPosterCloseButton.Text = "Close"
		Me.AddPosterCloseButton.UseVisualStyleBackColor = True
		'
		'AddPosterInfo
		'
		Me.AddPosterInfo.Anchor = System.Windows.Forms.AnchorStyles.None
		Me.AddPosterInfo.AutoSize = True
		Me.AddPosterInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.25!)
		Me.AddPosterInfo.Location = New System.Drawing.Point(34, 121)
		Me.AddPosterInfo.Name = "AddPosterInfo"
		Me.AddPosterInfo.Size = New System.Drawing.Size(258, 20)
		Me.AddPosterInfo.TabIndex = 5
		Me.AddPosterInfo.Text = "Paste link into box, and click view"
		'
		'AddPosterStatus
		'
		Me.AddPosterStatus.AutoSize = True
		Me.AddPosterStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
		Me.AddPosterStatus.Location = New System.Drawing.Point(12, 620)
		Me.AddPosterStatus.Name = "AddPosterStatus"
		Me.AddPosterStatus.Size = New System.Drawing.Size(0, 13)
		Me.AddPosterStatus.TabIndex = 6
		'
		'AddPosterReturnString
		'
		Me.AddPosterReturnString.Location = New System.Drawing.Point(12, 597)
		Me.AddPosterReturnString.Name = "AddPosterReturnString"
		Me.AddPosterReturnString.Size = New System.Drawing.Size(316, 20)
		Me.AddPosterReturnString.TabIndex = 7
		'
		'Form10
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(340, 641)
		Me.Controls.Add(Me.AddPosterReturnString)
		Me.Controls.Add(Me.AddPosterStatus)
		Me.Controls.Add(Me.AddPosterInfo)
		Me.Controls.Add(Me.AddPosterCloseButton)
		Me.Controls.Add(Me.AddPosterAddButton)
		Me.Controls.Add(Me.AddPosterViewButton)
		Me.Controls.Add(Me.AddPosterUrl)
		Me.Controls.Add(Me.AddPosterPictureBox)
		Me.Name = "Form10"
		Me.ShowIcon = False
		Me.Text = "Paste Poster Url"
		CType(Me.AddPosterPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
    Friend WithEvents AddPosterPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents AddPosterUrl As System.Windows.Forms.TextBox
    Friend WithEvents AddPosterViewButton As System.Windows.Forms.Button
    Friend WithEvents AddPosterAddButton As System.Windows.Forms.Button
    Friend WithEvents AddPosterCloseButton As System.Windows.Forms.Button
    Friend WithEvents AddPosterInfo As System.Windows.Forms.Label
    Friend WithEvents AddPosterStatus As System.Windows.Forms.Label
    Friend WithEvents AddPosterReturnString As System.Windows.Forms.TextBox
End Class
