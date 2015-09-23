Public Class Form9

    Public TVShowID As String = ""
    Public TVBanner As String = ""
    Private Sub AddBanner_Click(sender As Object, e As EventArgs) Handles Me.Click

        AddBannerPictureBox.ImageLocation = "https://github.com/Lunatixz/script.pseudotv.live/raw/development/resources/images/banner.png"
        AddBannerPictureBox.Refresh()
    End Sub

    Public Sub AddBannerViewButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddBannerViewButton.Click

        Dim UrlBanner As String = AddBannerInputBox.Text
        AddBannerInfo.Visible = False
        AddBannerAddButton.Visible = True
        AddBannerPictureBox.Visible = True
        AddBannerPictureBox.Load(UrlBanner)
        AddBannerPictureBox.Refresh()

        Dim ListItem As ListViewItem
        ListItem = Form1.TVShowList.SelectedItems.Item(0)

        TVShowID = ListItem.SubItems(2).Text

        Dim SelectArray(0)
        SelectArray(0) = 7

        'Shoot it over to the ReadRecord sub, 
        Dim ReturnArraySplit() As String = DbReadRecord(Form1.VideoDatabaseLocation, "SELECT * FROM tvshow WHERE idShow='" & TVShowID & "'", SelectArray)

        TVBanner = ReturnArraySplit(0)

    End Sub

    Private Sub AddBannerAddButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddBannerAddButton.Click
        Dim MediaType As String = "banner"

        If Form1.txtShowLocation.TextLength >= 6 Then
            If Form1.txtShowLocation.Text.Substring(0, 6) = "smb://" Then
                Form1.txtShowLocation.Text = Form1.txtShowLocation.Text.Replace("/", "\")
                Form1.txtShowLocation.Text = "\\" & Form1.txtShowLocation.Text.Substring(6)
            End If
        End If

        ' Displays a SaveFileDialog so the user can save the Image
        ' assigned to TVBannerSelect.
        Dim saveFileDialog1 As New SaveFileDialog()
        saveFileDialog1.InitialDirectory = Form1.txtShowLocation.Text
        saveFileDialog1.Filter = "JPeg Image|*.jpg"
        saveFileDialog1.Title = "Save an Image File"
        saveFileDialog1.FileName = "banner.jpg"
        saveFileDialog1.ShowDialog()

        Dim FileToSaveAs As String = System.IO.Path.Combine(Form1.txtShowLocation.Text, saveFileDialog1.FileName)
        AddBannerPictureBox.Image.Save(FileToSaveAs, System.Drawing.Imaging.ImageFormat.Jpeg)

        TVBanner = TVBanner & "<thumb aspect=""banner"">" & AddBannerInputBox.Text & "</thumb>"

        DbExecute("UPDATE art SET url = '" & AddBannerInputBox.ToString & "' WHERE media_id = '" & Form1.TVShowLabel.Text & "' and type = '" & MediaType & "'")
        DbExecute("UPDATE tvshow SET c06 = '" & TVBanner & "' WHERE idShow='" & TVShowID & "'")
        AddBannerStatus.Text = "Updated " & Form1.txtShowLocation.Text & " Successfully with " & AddBannerInputBox.Text & ""

    End Sub

    Private Sub AddBannerCloseButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddBannerCloseButton.Click
        Me.Close()
        Form1.Focus()
    End Sub

    Private Sub AddBannerPictureBox_Click(sender As Object, e As EventArgs) Handles AddBannerPictureBox.Click

    End Sub

    Private Sub AddBannerInfo_Click(sender As Object, e As EventArgs) Handles AddBannerInfo.Click

    End Sub
End Class
