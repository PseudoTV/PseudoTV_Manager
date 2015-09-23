<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form9
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
		Me.AddBannerInputBox = New System.Windows.Forms.TextBox()
		Me.AddBannerAddButton = New System.Windows.Forms.Button()
		Me.AddBannerCloseButton = New System.Windows.Forms.Button()
		Me.AddBannerPictureBox = New System.Windows.Forms.PictureBox()
		Me.AddBannerViewButton = New System.Windows.Forms.Button()
		Me.AddBannerReturnString = New System.Windows.Forms.TextBox()
		Me.AddBannerStatus = New System.Windows.Forms.Label()
		Me.AddBannerInfo = New System.Windows.Forms.Label()
		CType(Me.AddBannerPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'AddBannerInputBox
		'
		Me.AddBannerInputBox.Location = New System.Drawing.Point(13, 13)
		Me.AddBannerInputBox.Name = "AddBannerInputBox"
		Me.AddBannerInputBox.Size = New System.Drawing.Size(758, 20)
		Me.AddBannerInputBox.TabIndex = 0
		'
		'AddBannerAddButton
		'
		Me.AddBannerAddButton.Location = New System.Drawing.Point(670, 39)
		Me.AddBannerAddButton.Name = "AddBannerAddButton"
		Me.AddBannerAddButton.Size = New System.Drawing.Size(42, 23)
		Me.AddBannerAddButton.TabIndex = 1
		Me.AddBannerAddButton.Text = "Add"
		Me.AddBannerAddButton.UseVisualStyleBackColor = True
		Me.AddBannerAddButton.Visible = False
		'
		'AddBannerCloseButton
		'
		Me.AddBannerCloseButton.Location = New System.Drawing.Point(718, 39)
		Me.AddBannerCloseButton.Name = "AddBannerCloseButton"
		Me.AddBannerCloseButton.Size = New System.Drawing.Size(51, 23)
		Me.AddBannerCloseButton.TabIndex = 2
		Me.AddBannerCloseButton.Text = "Close"
		Me.AddBannerCloseButton.UseVisualStyleBackColor = True
		'
		'AddBannerPictureBox
		'
		Me.AddBannerPictureBox.Location = New System.Drawing.Point(11, 71)
		Me.AddBannerPictureBox.Margin = New System.Windows.Forms.Padding(0)
		Me.AddBannerPictureBox.MaximumSize = New System.Drawing.Size(758, 140)
		Me.AddBannerPictureBox.MinimumSize = New System.Drawing.Size(758, 140)
		Me.AddBannerPictureBox.Name = "AddBannerPictureBox"
		Me.AddBannerPictureBox.Size = New System.Drawing.Size(758, 140)
		Me.AddBannerPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.AddBannerPictureBox.TabIndex = 3
		Me.AddBannerPictureBox.TabStop = False
		'
		'AddBannerViewButton
		'
		Me.AddBannerViewButton.Location = New System.Drawing.Point(13, 39)
		Me.AddBannerViewButton.Name = "AddBannerViewButton"
		Me.AddBannerViewButton.Size = New System.Drawing.Size(38, 23)
		Me.AddBannerViewButton.TabIndex = 4
		Me.AddBannerViewButton.Text = "View"
		Me.AddBannerViewButton.UseVisualStyleBackColor = True
		'
		'AddBannerReturnString
		'
		Me.AddBannerReturnString.Location = New System.Drawing.Point(11, 242)
		Me.AddBannerReturnString.Name = "AddBannerReturnString"
		Me.AddBannerReturnString.Size = New System.Drawing.Size(758, 20)
		Me.AddBannerReturnString.TabIndex = 6
		'
		'AddBannerStatus
		'
		Me.AddBannerStatus.AutoSize = True
		Me.AddBannerStatus.Location = New System.Drawing.Point(13, 275)
		Me.AddBannerStatus.Name = "AddBannerStatus"
		Me.AddBannerStatus.Size = New System.Drawing.Size(0, 13)
		Me.AddBannerStatus.TabIndex = 7
		'
		'AddBannerInfo
		'
		Me.AddBannerInfo.AutoSize = True
		Me.AddBannerInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!)
		Me.AddBannerInfo.Location = New System.Drawing.Point(186, 39)
		Me.AddBannerInfo.Name = "AddBannerInfo"
		Me.AddBannerInfo.Size = New System.Drawing.Size(345, 24)
		Me.AddBannerInfo.TabIndex = 8
		Me.AddBannerInfo.Text = "Paste a link from the web, and click view"
		Me.AddBannerInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'Form9
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(781, 300)
		Me.Controls.Add(Me.AddBannerInfo)
		Me.Controls.Add(Me.AddBannerStatus)
		Me.Controls.Add(Me.AddBannerReturnString)
		Me.Controls.Add(Me.AddBannerViewButton)
		Me.Controls.Add(Me.AddBannerPictureBox)
		Me.Controls.Add(Me.AddBannerCloseButton)
		Me.Controls.Add(Me.AddBannerAddButton)
		Me.Controls.Add(Me.AddBannerInputBox)
		Me.Name = "Form9"
		Me.ShowIcon = False
		Me.Text = "Paste Banner Link"
		CType(Me.AddBannerPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
    Friend WithEvents AddBannerInputBox As System.Windows.Forms.TextBox
    Friend WithEvents AddBannerAddButton As System.Windows.Forms.Button
    Friend WithEvents AddBannerCloseButton As System.Windows.Forms.Button
    Friend WithEvents AddBannerPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents AddBannerViewButton As System.Windows.Forms.Button
    Friend WithEvents AddBannerReturnString As System.Windows.Forms.TextBox
    Friend WithEvents AddBannerStatus As System.Windows.Forms.Label
    Friend WithEvents AddBannerInfo As System.Windows.Forms.Label
End Class
