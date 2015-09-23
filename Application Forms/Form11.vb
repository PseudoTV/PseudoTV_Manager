Public Class Form11

    Public MovieID As String = ""
    Public MoviePoster As String = ""

    Private Sub AddMoviePoster_Click(sender As Object, e As EventArgs) Handles Me.Click

        AddMoviePosterPictureBox.ImageLocation = "https://github.com/Lunatixz/script.pseudotv.live/raw/development/resources/images/poster.png"
        AddMoviePosterPictureBox.Refresh()
    End Sub

    Public Sub AddMoviePosterViewButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddMoviePosterViewButton.Click

        Dim UrlPoster As String = AddMoviePosterUrl.Text
        AddMoviePosterInfo.Visible = False
        AddMoviePosterAddButton.Visible = True
        AddMoviePosterPictureBox.Visible = True
        AddMoviePosterPictureBox.Load(UrlPoster)
        AddMoviePosterPictureBox.Refresh()

        Dim ListItem As ListViewItem
        ListItem = Form1.MovieList.SelectedItems.Item(0)

        MovieID = ListItem.SubItems(1).Text


        Dim SelectArray(0)
        SelectArray(0) = 10

        'Shoot it over to the ReadRecord sub, 
        Dim ReturnArraySplit() As String = DbReadRecord(Form1.VideoDatabaseLocation, "SELECT * FROM movie WHERE idmovie='" & MovieID & "'", SelectArray)

        MoviePoster = ReturnArraySplit(0)

        AddMoviePosterReturnString.Text = System.IO.Path.GetDirectoryName(Form1.MovieLocation.Text)

    End Sub

    Private Sub AddMoviePosterAddButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddMoviePosterAddButton.Click

        Dim Type As String = "poster"
        Dim MediaType As String = "movie"


        If Form1.MovieLocation.TextLength >= 6 Then
            If Form1.MovieLocation.Text.Substring(0, 6) = "smb://" Then
                Form1.MovieLocation.Text = Form1.MovieLocation.Text.Replace("/", "\")
                Form1.MovieLocation.Text = "\\" & Form1.MovieLocation.Text.Substring(6)
            End If
        End If


        Dim directoryName As String = ""
        directoryName = System.IO.Path.GetDirectoryName(Form1.MovieLocation.Text)

        ' Displays a SaveFileDialog so the user can save the Image
        ' assigned to TVPosterSelect.
        Dim saveFileDialog1 As New SaveFileDialog()
        saveFileDialog1.InitialDirectory = directoryName
        saveFileDialog1.Filter = "JPeg Image|*.jpg"
        saveFileDialog1.Title = "Save an Image File"
        saveFileDialog1.FileName = "poster.jpg"
        saveFileDialog1.ShowDialog()

        Dim FileToSaveAs As String = System.IO.Path.Combine(saveFileDialog1.InitialDirectory, saveFileDialog1.FileName)
        AddMoviePosterPictureBox.Image.Save(FileToSaveAs, System.Drawing.Imaging.ImageFormat.Jpeg)

        MoviePoster = MoviePoster & "<thumb>" & AddMoviePosterUrl.Text & "</thumb>"

        DbExecute("UPDATE art SET url = '" & AddMoviePosterUrl.ToString & "' WHERE media_id = '" & Form1.MovieIDLabel.Text & "' and type = '" & MediaType & "'")
        DbExecute("UPDATE movie SET c08 = '" & MoviePoster & "' WHERE idmovie='" & MovieID & "'")
        AddMoviePosterStatus.Text = "Updated " & Form1.MovieLocation.Text & " Successfully with " & AddMoviePosterUrl.Text & ""

        For iCount = 1 To 16000
        Next iCount

        Me.Close()

    End Sub

    Private Sub AddMoviePosterCloseButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddMoviePosterCloseButton.Click
        Me.Close()
        Form1.Focus()
    End Sub

    Private Sub AddMoviePosterPictureBox_Click(sender As Object, e As EventArgs) Handles AddMoviePosterPictureBox.Click

    End Sub

    Private Sub AddMoviePosterInfo_Click(sender As Object, e As EventArgs) Handles AddMoviePosterInfo.Click

    End Sub
End Class
