<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form11
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
		Me.AddMoviePosterPictureBox = New System.Windows.Forms.PictureBox()
		Me.AddMoviePosterUrl = New System.Windows.Forms.TextBox()
		Me.AddMoviePosterViewButton = New System.Windows.Forms.Button()
		Me.AddMoviePosterAddButton = New System.Windows.Forms.Button()
		Me.AddMoviePosterCloseButton = New System.Windows.Forms.Button()
		Me.AddMoviePosterInfo = New System.Windows.Forms.Label()
		Me.AddMoviePosterStatus = New System.Windows.Forms.Label()
		Me.AddMoviePosterReturnString = New System.Windows.Forms.TextBox()
		CType(Me.AddMoviePosterPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'AddMoviePosterPictureBox
		'
		Me.AddMoviePosterPictureBox.Location = New System.Drawing.Point(12, 72)
		Me.AddMoviePosterPictureBox.Name = "AddMoviePosterPictureBox"
		Me.AddMoviePosterPictureBox.Size = New System.Drawing.Size(316, 519)
		Me.AddMoviePosterPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.AddMoviePosterPictureBox.TabIndex = 0
		Me.AddMoviePosterPictureBox.TabStop = False
		'
		'AddMoviePosterUrl
		'
		Me.AddMoviePosterUrl.Location = New System.Drawing.Point(12, 12)
		Me.AddMoviePosterUrl.Name = "AddMoviePosterUrl"
		Me.AddMoviePosterUrl.Size = New System.Drawing.Size(316, 20)
		Me.AddMoviePosterUrl.TabIndex = 1
		'
		'AddMoviePosterViewButton
		'
		Me.AddMoviePosterViewButton.Location = New System.Drawing.Point(12, 38)
		Me.AddMoviePosterViewButton.Name = "AddMoviePosterViewButton"
		Me.AddMoviePosterViewButton.Size = New System.Drawing.Size(44, 23)
		Me.AddMoviePosterViewButton.TabIndex = 2
		Me.AddMoviePosterViewButton.Text = "View"
		Me.AddMoviePosterViewButton.UseVisualStyleBackColor = True
		'
		'AddMoviePosterAddButton
		'
		Me.AddMoviePosterAddButton.Location = New System.Drawing.Point(234, 38)
		Me.AddMoviePosterAddButton.Name = "AddMoviePosterAddButton"
		Me.AddMoviePosterAddButton.Size = New System.Drawing.Size(44, 23)
		Me.AddMoviePosterAddButton.TabIndex = 3
		Me.AddMoviePosterAddButton.Text = "Add"
		Me.AddMoviePosterAddButton.UseVisualStyleBackColor = True
		Me.AddMoviePosterAddButton.Visible = False
		'
		'AddMoviePosterCloseButton
		'
		Me.AddMoviePosterCloseButton.Location = New System.Drawing.Point(284, 38)
		Me.AddMoviePosterCloseButton.Name = "AddMoviePosterCloseButton"
		Me.AddMoviePosterCloseButton.Size = New System.Drawing.Size(44, 23)
		Me.AddMoviePosterCloseButton.TabIndex = 4
		Me.AddMoviePosterCloseButton.Text = "Close"
		Me.AddMoviePosterCloseButton.UseVisualStyleBackColor = True
		'
		'AddMoviePosterInfo
		'
		Me.AddMoviePosterInfo.Anchor = System.Windows.Forms.AnchorStyles.None
		Me.AddMoviePosterInfo.AutoSize = True
		Me.AddMoviePosterInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.25!)
		Me.AddMoviePosterInfo.Location = New System.Drawing.Point(34, 121)
		Me.AddMoviePosterInfo.Name = "AddMoviePosterInfo"
		Me.AddMoviePosterInfo.Size = New System.Drawing.Size(258, 20)
		Me.AddMoviePosterInfo.TabIndex = 5
		Me.AddMoviePosterInfo.Text = "Paste link into box, and click view"
		'
		'AddMoviePosterStatus
		'
		Me.AddMoviePosterStatus.AutoSize = True
		Me.AddMoviePosterStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
		Me.AddMoviePosterStatus.Location = New System.Drawing.Point(12, 620)
		Me.AddMoviePosterStatus.Name = "AddMoviePosterStatus"
		Me.AddMoviePosterStatus.Size = New System.Drawing.Size(0, 13)
		Me.AddMoviePosterStatus.TabIndex = 6
		'
		'AddMoviePosterReturnString
		'
		Me.AddMoviePosterReturnString.Location = New System.Drawing.Point(12, 597)
		Me.AddMoviePosterReturnString.Name = "AddMoviePosterReturnString"
		Me.AddMoviePosterReturnString.Size = New System.Drawing.Size(316, 20)
		Me.AddMoviePosterReturnString.TabIndex = 7
		'
		'Form11
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(340, 641)
		Me.Controls.Add(Me.AddMoviePosterReturnString)
		Me.Controls.Add(Me.AddMoviePosterStatus)
		Me.Controls.Add(Me.AddMoviePosterInfo)
		Me.Controls.Add(Me.AddMoviePosterCloseButton)
		Me.Controls.Add(Me.AddMoviePosterAddButton)
		Me.Controls.Add(Me.AddMoviePosterViewButton)
		Me.Controls.Add(Me.AddMoviePosterUrl)
		Me.Controls.Add(Me.AddMoviePosterPictureBox)
		Me.Name = "Form11"
		Me.ShowIcon = False
		Me.Text = "Paste Poster Url"
		CType(Me.AddMoviePosterPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
    Friend WithEvents AddMoviePosterPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents AddMoviePosterUrl As System.Windows.Forms.TextBox
    Friend WithEvents AddMoviePosterViewButton As System.Windows.Forms.Button
    Friend WithEvents AddMoviePosterAddButton As System.Windows.Forms.Button
    Friend WithEvents AddMoviePosterCloseButton As System.Windows.Forms.Button
    Friend WithEvents AddMoviePosterInfo As System.Windows.Forms.Label
    Friend WithEvents AddMoviePosterStatus As System.Windows.Forms.Label
    Friend WithEvents AddMoviePosterReturnString As System.Windows.Forms.TextBox
End Class
