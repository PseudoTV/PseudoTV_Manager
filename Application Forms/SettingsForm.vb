Imports System
Imports System.IO



Public Class SettingsForm

	Public User As String = Environment.UserName
	Public VideoDatabaseLocation As String = ""
	Public PseudoTvSettingsLocation As String = ""
	Public AddonDatabase As String = ""
	Public AddonDatabaseLocation As String = ""
	Public VDBext As String = ""
	Public Version As KodiVersion = KodiVersion.Helix

	Private VideoDBFile As String


	Private Sub DatabaseType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DatabaseType.SelectedIndexChanged
		If DatabaseType.SelectedIndex = 1 Then
			MySqlBox.Visible = True
		ElseIf DatabaseType.SelectedIndex = 0 Then
			MySqlBox.Visible = False
		End If
	End Sub

	Private Sub BrowseBtn_Click(sender As Object, e As EventArgs) Handles BrowseBtn.Click
		Dim folderBrowserDialog1 As New FolderBrowserDialog()
		Dim openFileDialog1 As New OpenFileDialog()
		folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop
		folderBrowserDialog1.SelectedPath = "C:\Users\" & User & "\AppData\Roaming\Kodi\userdata\Database"
		folderBrowserDialog1.Description = "Select Xbmc/Kodi UserData Path"

		openFileDialog1.DefaultExt = ""
		openFileDialog1.Filter = "SqliteDB files (*.db)|*MyVideos*.db"


		If (folderBrowserDialog1.ShowDialog() = DialogResult.OK) Then
			UserData.Text = folderBrowserDialog1.SelectedPath
			openFileDialog1.InitialDirectory = folderBrowserDialog1.SelectedPath
			Dim result As DialogResult = openFileDialog1.ShowDialog()

			' OK button was pressed. 
			If (result = DialogResult.OK) Then
				VideoDBFile = openFileDialog1.FileName
			End If
		End If

	End Sub

	Private Sub SettingsSaveBtn_Click(sender As Object, e As EventArgs) Handles SettingsSaveBtn.Click

		Dim SettingsFile As String = Application.StartupPath() & "\" & "Settings.txt"
		Dim Instance As Integer = 0

		If System.IO.File.Exists(SettingsFile) = False Then
			System.IO.File.Create(SettingsFile)

		End If

		If DatabaseType.SelectedIndex = 0 Then
			If XbmcVersion.SelectedIndex = 0 Then
				VDBext = "78.db"
				Version = KodiVersion.Gotham
			ElseIf XbmcVersion.SelectedIndex = 1 Then
				VDBext = "90.db"
				Version = KodiVersion.Helix
			ElseIf XbmcVersion.SelectedIndex = 2 Then
				VDBext = "93.db"
				Version = KodiVersion.Isengard
			End If
			VideoDatabaseLocation = UserData.Text & "\Database\MyVideos" & VDBext

			If TestMYSQLite(VideoDatabaseLocation) = True Then
				Dim FilePaths As String = "0" & " | " & "Default" & " | " & Instance & " | " & XbmcVersion.SelectedIndex.ToString & " | " & UserData.Text
				SaveFile(SettingsFile, FilePaths)

				'Now, update the variables in the Main form with the proper paths
				Form1.DatabaseType = 0
				Form1.RefreshSettings()

				'Refresh everything
				Form1.RefreshALL()
				Form1.RefreshTVGuide()

				Me.Visible = False

			End If

		ElseIf Address.Text <> "" And Username.Text <> "" And Password.Text <> "" Then

			If XbmcVersion.SelectedIndex = 0 Then
				VDBext = "78"
				Version = KodiVersion.Gotham
			ElseIf XbmcVersion.SelectedIndex = 1 Then
				VDBext = "90"
				Version = KodiVersion.Helix
			ElseIf XbmcVersion.SelectedIndex = 2 Then
				VDBext = "93"
				Version = KodiVersion.Isengard
			End If

			Dim ConnectionString = "server=" & Address.Text & "; user id=" & Username.Text & "; password=" & Password.Text & "; database=" & "myvideos" & VDBext & "; port=" & Port.Text

			If TestMYSQL(ConnectionString) = True Then

				Dim FilePaths As String = "1" & " | " & "Default" & " | " & Instance & " | " & XbmcVersion.SelectedIndex.ToString & " | " & ConnectionString & " | " & UserData.Text
				SaveFile(SettingsFile, FilePaths)

				'Now, update the variables in the Main form with the proper paths
				Form1.DatabaseType = 1
				Form1.MySQLConnectionString = ConnectionString
				Form1.UserDataFolder = UserData.Text

				'Refresh everything
				Form1.RefreshALL()
				Form1.RefreshTVGuide()

				Me.Visible = False
				Form1.Focus()
			End If
		End If

	End Sub

	Public Sub SettingsForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
		Locations.Enabled = False
		Button1.Enabled = False
		Button2.Enabled = False

		If Form1.DatabaseType <> "" Then
			If Form1.DatabaseType = 0 Then
				DatabaseType.SelectedIndex = 0
				Version = GetKodiVersion(Form1.VideoDatabaseLocation)
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

			End If

			If Form1.MySQLConnectionString <> "" Then
				Dim SplitString() = Split(Form1.MySQLConnectionString, ";")
				DatabaseType.SelectedIndex = 1
				If Form1.MySQLConnectionString.Contains("93") Then
					XbmcVersion.SelectedIndex = 2
					Version = KodiVersion.Isengard
				ElseIf Form1.MySQLConnectionString.Contains("78") Then
					XbmcVersion.SelectedIndex = 0
					Version = KodiVersion.Gotham
				ElseIf Form1.MySQLConnectionString.Contains("90") Then
					XbmcVersion.SelectedIndex = 1
					Version = KodiVersion.Helix
				End If
				Address.Text = Split(SplitString(0), "server=")(1)
				Username.Text = Split(SplitString(1), "user id=")(1)
				Password.Text = Split(SplitString(2), "password=")(1)
				Port.Text = Split(SplitString(4), "port=")(1)
			End If

			UserData.Text = Form1.UserDataFolder
			VideoDatabaseLocation = UserData.Text & "\Database\" & "MyVideos"
			If Version = KodiVersion.Isengard Then
				AddonDatabaseLocation = UserData.Text & "\Database\Addons19.db"
			Else
				AddonDatabaseLocation = UserData.Text & "\Database\Addons16.db"
			End If
			PseudoTvSettingsLocation = UserData.Text & "\addon_data\script.pseudotv.live\settings2.xml"
		Else
		End If

	End Sub


	Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
		With Locations.Items
			.Add("Other")
		End With
	End Sub

End Class
