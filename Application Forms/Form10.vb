Public Class Form10

    Public TVShowID As String = ""
    Public TVBanner As String = ""

    Private Sub AddPoster_Click(sender As Object, e As EventArgs) Handles Me.Click

        AddPosterPictureBox.ImageLocation = "https://github.com/Lunatixz/script.pseudotv.live/raw/development/resources/images/poster.png"
        AddPosterPictureBox.Refresh()
    End Sub

    Public Sub AddPosterViewButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddPosterViewButton.Click

        Dim UrlPoster As String = AddPosterUrl.Text
        AddPosterInfo.Visible = False
        AddPosterAddButton.Visible = True
        AddPosterPictureBox.Visible = True
        AddPosterPictureBox.Load(UrlPoster)
        AddPosterPictureBox.Refresh()

        Dim ListItem As ListViewItem
        ListItem = Form1.TVShowList.SelectedItems.Item(0)

        TVShowID = ListItem.SubItems(2).Text

        Dim SelectArray(0)
        SelectArray(0) = 7

        'Shoot it over to the ReadRecord sub, 
        Dim ReturnArraySplit() As String = DbReadRecord(Form1.VideoDatabaseLocation, "SELECT * FROM tvshow WHERE idShow='" & TVShowID & "'", SelectArray)

        TVBanner = ReturnArraySplit(0)
        AddPosterReturnString.Text = Form1.txtShowLocation.Text

    End Sub

    Private Sub AddPosterAddButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddPosterAddButton.Click
        Dim MediaType As String = "poster"

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
        saveFileDialog1.FileName = "poster.jpg"
        saveFileDialog1.ShowDialog()

        Dim FileToSaveAs As String = System.IO.Path.Combine(Form1.txtShowLocation.Text, saveFileDialog1.FileName)
        AddPosterPictureBox.Image.Save(FileToSaveAs, System.Drawing.Imaging.ImageFormat.Jpeg)

        TVBanner = TVBanner & "<thumb aspect=""poster"">" & AddPosterUrl.Text & "</thumb>"

        DbExecute("UPDATE art SET url = '" & AddPosterUrl.ToString & "' WHERE media_id = '" & Form1.TVShowLabel.Text & "' and type = '" & MediaType & "'")
        DbExecute("UPDATE tvshow SET c06 = '" & TVBanner & "' WHERE idShow='" & TVShowID & "'")
        AddPosterStatus.Text = "Updated " & Form1.txtShowLocation.Text & " Successfully with " & AddPosterUrl.Text & ""

    End Sub

    Private Sub AddPosterCloseButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddPosterCloseButton.Click
        Me.Close()
        Form1.Focus()
    End Sub

    Private Sub AddPosterPictureBox_Click(sender As Object, e As EventArgs) Handles AddPosterPictureBox.Click

    End Sub

    Private Sub AddPosterInfo_Click(sender As Object, e As EventArgs) Handles AddPosterInfo.Click

    End Sub
End Class
