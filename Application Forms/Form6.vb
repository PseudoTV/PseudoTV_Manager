Imports System
Imports System.IO
Imports System.Text.RegularExpressions

Public Class Form6

    Public User As String = Environment.UserName
    Private VideoDBFile As String
    Private AddonDBFile As String
	Private VersionString As String
	Public Version As KodiVersion = KodiVersion.Helix

    Private VideoDBFileDialog As New OpenFileDialog()
    Private SettingsFileDialog As New OpenFileDialog()
    Private AddonDBFileDialog As New OpenFileDialog()

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
		If XbmcVersion.SelectedIndex = 0 Then
			VersionString = "XBMC"
		Else
			VersionString = "Kodi"
		End If

		VideoDBFileDialog.InitialDirectory = "C:\Users\" & User & "\AppData\Roaming\" & VersionString & "\userdata\Database"

        VideoDBFileDialog.DefaultExt = ""
        VideoDBFileDialog.Filter = "SqliteDB files (*.db)|*MyVideos*.db"

		If (VideoDBFileDialog.ShowDialog() = DialogResult.OK) Then
			TextBox1.Text = VideoDBFileDialog.FileName
		End If
	End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
		SettingsFileDialog.InitialDirectory = "C:\Users\" & User & "\AppData\Roaming\" & VersionString & "\userdata\addon_data\script.pseudotv.live"

        SettingsFileDialog.DefaultExt = ""
        SettingsFileDialog.Filter = "Settings2 files (*.xml)|**.xml"

		If (SettingsFileDialog.ShowDialog() = DialogResult.OK) Then
			TextBox2.Text = SettingsFileDialog.FileName
		End If

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
		AddonDBFileDialog.InitialDirectory = "C:\Users\" & User & "\AppData\Roaming\" & VersionString & "\userdata\Database"

        AddonDBFileDialog.DefaultExt = ""
		AddonDBFileDialog.Filter = "SqliteDB files (*.db)|*Addons*.db"

		If (AddonDBFileDialog.ShowDialog() = DialogResult.OK) Then
			TextBox8.Text = AddonDBFileDialog.FileName
		End If

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

		'Dim SettingsFile As String = Application.StartupPath() & "\" & "Settings.txt"

        'See if there's already a text file in place, if not then create one.

		'If System.IO.File.Exists(SettingsFile) = False Then
		'System.IO.File.Create(SettingsFile)
		'End If

		'Verify that all 3 files indeed exist at least
		If System.IO.File.Exists(TextBox1.Text) = True And System.IO.File.Exists(TextBox2.Text) = True And System.IO.File.Exists(TextBox8.Text) = True Then

			Version = GetKodiVersion(TextBox1.Text)

			If TestMYSQLite(TextBox1.Text) = True Then

				'Save them to the settings file
				'Dim FilePaths As String = "0" & " | " & TextBox1.Text & " | " & TextBox2.Text & " | " & TextBox8.Text
				'SaveFile(SettingsFile, FilePaths)

				'Now, update the variables in the Main form with the proper paths
				Form1.DatabaseType = 0
				My.Settings.DatabaseType = 0
				Form1.VideoDatabaseLocation = TextBox1.Text
				My.Settings.VideoDatabaseLocation = TextBox1.Text
				Form1.PseudoTvSettingsLocation = TextBox2.Text
				My.Settings.PseudoTvSettingsLocation = TextBox2.Text
				Form1.AddonDatabaseLocation = TextBox8.Text
				My.Settings.AddonDatabaseLocation = TextBox8.Text
				Form1.Version = Me.Version
				My.Settings.Version = Me.Version
				My.Settings.Save()
				'Refresh everything
				Form1.RefreshALL()
				Form1.RefreshTVGuide()

				Me.Visible = False
				Form1.Focus()
			End If
		ElseIf TextBox3.Text <> "" And TextBox4.Text <> "" And TextBox6.Text <> "" And System.IO.File.Exists(TextBox2.Text) = True And System.IO.File.Exists(TextBox8.Text) = True Then

			'server=localhost; user id=mike; password=12345; database=in_out

			Dim ConnectionString = "server=" & TextBox3.Text & "; user id=" & TextBox4.Text & "; password=" & TextBox5.Text & "; database=" & TextBox6.Text & "; port=" & TextBox7.Text

			If TestMYSQL(ConnectionString) = True Then

				'Dim FilePaths As String = "1" & " | " & ConnectionString & " | " & TextBox2.Text & " | " & TextBox8.Text
				'SaveFile(SettingsFile, FilePaths)

				'Now, update the variables in the Main form with the proper paths
				Form1.DatabaseType = 1
				My.Settings.DatabaseType = 1
				Form1.MySQLConnectionString = ConnectionString
				My.Settings.MySQLConnectionString = ConnectionString
				Form1.PseudoTvSettingsLocation = TextBox2.Text
				My.Settings.PseudoTvSettingsLocation = TextBox2.Text
				Form1.AddonDatabaseLocation = TextBox8.Text
				My.Settings.AddonDatabaseLocation = TextBox8.Text
				Form1.Version = Me.Version
				My.Settings.Version = Me.Version
				My.Settings.Save()
				'Refresh everything
				Form1.RefreshALL()
				Form1.RefreshTVGuide()

				Me.Visible = False
				Form1.Focus()
			End If
		End If



    End Sub

    Private Sub Form6_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

		Select Case Version
			Case KodiVersion.Gotham
				XbmcVersion.SelectedIndex = 0
			Case KodiVersion.Helix
				XbmcVersion.SelectedIndex = 1
			Case KodiVersion.Isengard
				XbmcVersion.SelectedIndex = 2
			Case KodiVersion.Jarvis
				XbmcVersion.SelectedIndex = 3
		End Select

        If Form1.VideoDatabaseLocation <> "" Then
            TextBox1.Text = Form1.VideoDatabaseLocation
            TextBox2.Text = Form1.PseudoTvSettingsLocation
			TextBox8.Text = Form1.AddonDatabaseLocation
		Else
			FindKodiSettings()
		End If

        If Form1.MySQLConnectionString <> "" Then
            Dim SplitString() = Split(Form1.MySQLConnectionString, ";")

            TextBox2.Text = Form1.PseudoTvSettingsLocation
            TextBox3.Text = Split(SplitString(0), "server=")(1)
            TextBox4.Text = Split(SplitString(1), "user id=")(1)
            TextBox5.Text = Split(SplitString(2), "password=")(1)
            TextBox6.Text = Split(SplitString(3), "database=")(1)
            TextBox7.Text = Split(SplitString(4), "port=")(1)
        End If

	End Sub

	Private Sub FindKodiSettings()
		Dim folderKodi As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\kodi\userdata"
		Dim folderXbmc As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\.xbmc\userdata"
		Dim databaseFolder As String
		Dim addonDataFolder As String

		If (Directory.Exists(folderKodi)) Then
			databaseFolder = folderKodi & "\Database"
			addonDataFolder = folderKodi & "\addon_data"
		ElseIf (Directory.Exists(folderXbmc)) Then
			databaseFolder = folderXbmc & "\Database"
			addonDataFolder = folderXbmc & "\addon_data"
		Else
			Return
		End If

		Dim regex As Regex = New Regex("(Addons|MyVideos)(\d+).db")
		Dim databaseDir As New IO.DirectoryInfo(databaseFolder)
		Dim filelist As IO.FileInfo() = databaseDir.GetFiles()
		Dim file As IO.FileInfo

		For Each file In filelist
			Dim match As Match = regex.Match(file.Name)
			If match.Success Then
				If (match.Groups(1).Value = "MyVideos") Then
					Version = GetKodiVersion(file.Name)
					TextBox1.Text = file.FullName
				ElseIf (match.Groups(1).Value = "Addons") Then
					TextBox8.Text = file.FullName
				End If
			End If
		Next

		'C:\Users\Scott\AppData\Roaming\Kodi\userdata\addon_data\script.pseudotv.live\settings2.xml
		If (System.IO.File.Exists(addonDataFolder & "\script.pseudotv.live\settings2.xml")) Then
			TextBox2.Text = addonDataFolder & "\script.pseudotv.live\settings2.xml"
		End If

		Select Case Version
			Case KodiVersion.Gotham
				XbmcVersion.SelectedIndex = 0
			Case KodiVersion.Helix
				XbmcVersion.SelectedIndex = 1
			Case KodiVersion.Isengard
				XbmcVersion.SelectedIndex = 2
			Case KodiVersion.Jarvis
				XbmcVersion.SelectedIndex = 3
		End Select
	End Sub

	Public Function ReadVersion(ByVal GenreName As String)
		'This looks up the Genre based on the name and returns the proper Genre ID

		Dim GenreID As String = Nothing

		Dim SelectArray(0)
		SelectArray(0) = 0

		'Shoot it over to the ReadRecord sub
		Dim ReturnArray() As String = DbReadRecord(TextBox1.Text, "SELECT idVersion FROM version", SelectArray)

		'The ID # is all we need.
		'Just make sure it's not a null reference.
		If ReturnArray Is Nothing Then
			MsgBox("nothing!")
		Else
			GenreID = ReturnArray(0)
		End If

		Return GenreID
	End Function

End Class