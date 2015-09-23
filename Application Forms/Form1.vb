'Import the SQLite DLL file.
'C:\SQLite\THERE!

Imports System
Imports System.IO
Imports System.Diagnostics
Imports System.Uri

Public Class Form1
    'For sorting columns in listviews
    Private m_SortingColumn As ColumnHeader
    Private m_SortingColumn2 As ColumnHeader

    Public DatabaseType As Integer
    Public MySQLConnectionString As String
    Public VideoDatabaseLocation As String
    Public PseudoTvSettingsLocation As String
    Public AddonDatabaseLocation As String
    Public XbmcVersion As Integer
    Public UserDataFolder As String
    Public PluginNotInclude As String
    Public YouTubeMulti As String
	Dim ResetHours As Integer
	Public Version As KodiVersion = KodiVersion.Helix


    Public Function LookUpGenre(ByVal GenreName As String)
        'This looks up the Genre based on the name and returns the proper Genre ID

        Dim GenreID As String = Nothing

        Dim SelectArray(0)
		SelectArray(0) = 0

		Dim genrePar As String = "name"
		If (Version < KodiVersion.Isengard) Then
			genrePar = "strGenre"
		End If

        'Shoot it over to the ReadRecord sub
		Dim ReturnArray() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM genre where " + genrePar + "='" & GenreName & "'", SelectArray)

        'The ID # is all we need.
        'Just make sure it's not a null reference.
        If ReturnArray Is Nothing Then
            MsgBox("nothing!")
        Else
            GenreID = ReturnArray(0)
        End If

        Return GenreID
    End Function

    Public Function LookUpNetwork(ByVal Network As String)
        'This looks up the Network name and returns the proper Network ID

        Dim NetworkID As String = Nothing

        Dim SelectArray(0)
		SelectArray(0) = 0

		Dim studioPar As String = "name"
		If (Version < KodiVersion.Isengard) Then
			studioPar = "strStudio"
		End If

        'Shoot it over to the ReadRecord sub
		Dim ReturnArray() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM studio where " + studioPar + "='" & Network & "'", SelectArray)

        'The ID # is all we need.
        'Just make sure it's not a null reference.
        If ReturnArray Is Nothing Then
        Else
            NetworkID = ReturnArray(0)
        End If
        Return NetworkID

    End Function

    Public Sub RefreshTVGuide()
        'Clear the TV name and the List items
        TVGuideShowName.Text = ""
        TVGuideList.Items.Clear()

        Dim FullFile As String

        Dim TotalChannels As Integer = 0

        'This will hold an array of our channel #s
        Dim ChannelArray() As String = Nothing
        Dim ChannelNum As Integer

        Dim FILE_LOCATION As String = PseudoTvSettingsLocation

        If System.IO.File.Exists(PseudoTvSettingsLocation) = True Then
            'Load everything into the FullFile string
            FullFile = ReadFile(PseudoTvSettingsLocation)

            Dim objReader As New System.IO.StreamReader(FILE_LOCATION)

            'Loop through each line individually, then add the channel # to an array
            Do While objReader.Peek() <> -1

                Dim SingleLine = objReader.ReadLine()

                If InStr(SingleLine, "_type" & Chr(34) & " value=") Then
                    Dim Part1 = Split(SingleLine, "_type")(0)
                    Dim Part2 = Split(Part1, "_")(1)

                    ReDim Preserve ChannelArray(ChannelNum)
                    ChannelNum = ChannelNum + 1
                    ChannelArray(UBound(ChannelArray)) = Part2

                End If

            Loop

            objReader.Close()

            For x = 0 To UBound(ChannelArray)

                Dim ChannelInfo() As String

                Dim ChannelRules As String = ""
                Dim ChannelRulesAdvanced As String = ""
                Dim ChannelRulesCount As String = ""
                Dim ChannelType As String = ""
                Dim ChannelTypeDetail As String = ""
                Dim ChannelTime As String = ""
                Dim ChannelTypeDetail2 As String = ""
                Dim ChannelTypeDetail3 As String = ""
                Dim ChannelTypeDetail4 As String = ""

                'Grab everything that says setting id = Channel #
                ChannelInfo = Split(FullFile, "<setting id=" & Chr(34) & "Channel_" & ChannelArray(x) & "_", , CompareMethod.Text)

                'Now loop through everything it returned.
                For y = 1 To ChannelInfo.Count - 1
                    Dim RuleType As String
                    Dim RuleValue As String

                    RuleType = Split(ChannelInfo(y), Chr(34))(0)

                    RuleValue = Split(ChannelInfo(y), "value=" & Chr(34))(1)
                    RuleValue = Split(RuleValue, Chr(34))(0)

                    If RuleType = "changed" Then

                    ElseIf RuleType = "rulecount" Then
                        ChannelRulesCount = RuleValue
                    ElseIf RuleType = "time" Then
                        ChannelTime = RuleValue
                    ElseIf RuleType = "type" Then
                        'Update the Channel type to the value of that.
                        ChannelType = RuleValue
                    ElseIf RuleType = "1" Then
                        'Gets more information on what type the channel is, playlist location/genre/zap2it/etc.
                        ChannelTypeDetail = RuleValue
                    ElseIf RuleType = "2" Then
                        'Gets (LiveTV-8)stream source/(IPTV-9)iptv source/(Youtube-10)youtube channel type/
                        '(Rss-11)reserved/(LastFM-13)LastFM User/(BTP/Cinema Experience14)filter types or smart playlist/
                        '(Direct/SF-15, Direct Playon-16)exclude list/
                        ChannelTypeDetail2 = RuleValue
                    ElseIf RuleType = "3" Then
                        'Gets (LiveTV-8)xmltv filename/(IPTV-9)show titles/(Youtube-10, Rss-11, LastFM-13)media limits/
                        '(BTP/Cinema Experience-14)parsing resolution/(Direct/SF-15, Direct Playon-16)file limit
                        ChannelTypeDetail3 = RuleValue
                    ElseIf RuleType = "4" Then
                        'Gets (IPTV-9)show description/(Youtube-10, Rss-11, LastFM-13)sort ordering/
                        '(BTP/Cinema Experience-14)years to parse by or unused/(Direct/SF-15, Direct Playon-16)sort ordering
                        ChannelTypeDetail4 = RuleValue
                    ElseIf InStr(RuleType, "rule", CompareMethod.Text) Then
                        'Okay, It's rule information.

                        'Get the rule number.
                        Dim RuleNumber As String
                        RuleNumber = Split(RuleType, "rule_")(1)
                        RuleNumber = Split(RuleNumber, "_")(0)

                        If InStr(RuleType, "opt", CompareMethod.Text) Then
                            'Okay, it's an actual option tied to another rule.

                            Dim OptNumber = Split(RuleType, "_opt_")(1)
                            RuleNumber = Split(RuleType, "_opt_")(0)
                            RuleNumber = Split(RuleNumber, "rule_")(1)

                            'MsgBox("Opt : " & RuleNumber & " | " & OptNumber & " | " & RuleValue)
                            'ChannelRulesAdvanced = ChannelRulesAdvanced & "~" & RuleNumber & "|" & OptNumber & "|" & RuleValue
                            'MsgBox(RuleNumber & " | " & OptNumber & " | " & RuleValue)

                            'Add this to the previous rule, remove the ending 
                            'Then add this rule as Rule#:RuleValue
                            ChannelRules = ChannelRules & "|" & OptNumber & "^" & RuleValue
                        Else
                            ChannelRules = ChannelRules & "~" & RuleNumber & "|" & RuleValue
                        End If
                    Else

                    End If

                    'End result for a basic option:  ~RuleNumber|RuleValue 
                    'End result for an advanced option:  ~RuleNumber|RuleValue|Rule1^Rule1Value|Rule2^Rule2Value

                Next

                Dim str(10) As String

                str(0) = ChannelArray(x)  'Channel #.
                str(1) = ChannelType
                str(2) = ChannelTypeDetail
                str(3) = ChannelTime
                str(4) = ChannelRules
                str(5) = ChannelRulesCount
                str(6) = ChannelTypeDetail2
                str(7) = ChannelTypeDetail3
                str(8) = ChannelTypeDetail4

                Dim itm As ListViewItem
                itm = New ListViewItem(str)
                'Add to list
                TVGuideList.Items.Add(itm)

            Next

        End If

        'Sort List
        TVGuideList.ListViewItemSorter = New clsListviewSorter(0, SortOrder.Ascending)
        ' Sort. 
        TVGuideList.Sort()
    End Sub

    Public Sub RefreshGenres()
        GenresList.Items.Clear()
        Dim SelectArrayMain(1)
        SelectArrayMain(0) = 0
        SelectArrayMain(1) = 1

        'Shoot it over to the ReadRecord sub
        Dim ReturnArrayMain() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM genre", SelectArrayMain)

        'Loop through and read the name

        For x = 0 To UBound(ReturnArrayMain)

            'Sort them into an array
            Dim SplitItem() As String = Split(ReturnArrayMain(x), "~")
            'Position 0 = genre ID
            'Position 1 = genre name

            'Push array into ListViewItem

            Dim SelectArray(0)
            SelectArray(0) = 1

            'Now, grab a list of all the shows that match the GenreID
			Dim ReturnArray() As String
			If (Version >= KodiVersion.Isengard) Then
				ReturnArray = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM genre_link WHERE genre_id='" & SplitItem(0) & "' AND media_type = 'tvshow'", SelectArray)
			Else
				ReturnArray = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM genrelinktvshow WHERE idGenre='" & SplitItem(0) & "'", SelectArray)
			End If

			'This will grab the number of movies.
			Dim ReturnArray2() As String
			If (Version >= KodiVersion.Isengard) Then
				ReturnArray2 = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM genre_link WHERE genre_id='" & SplitItem(0) & "' AND media_type = 'movie'", SelectArray)
			Else
				ReturnArray2 = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM genrelinkmovie WHERE idGenre='" & SplitItem(0) & "'", SelectArray)
			End If

			Dim ShowNum
			Dim MovieNum

			'This is the total number of shows that match this genre.
			'Also, verify the returning array is something, not null before proceeding.
			If ReturnArray Is Nothing Then
				ShowNum = 0
			Else
				ShowNum = ReturnArray.Count
			End If

			If ReturnArray2 Is Nothing Then
				MovieNum = 0
			Else
				MovieNum = ReturnArray2.Count
			End If

			Dim str(4) As String
			'Genre Name
			'# of shows in genre
			'# of movies in genre
			'Total of both /\
			'Genre ID

			str(0) = SplitItem(1)
			str(1) = ShowNum
			str(2) = MovieNum
			str(3) = ShowNum + MovieNum
			str(4) = SplitItem(0)


			Dim itm As ListViewItem
			itm = New ListViewItem(str)
			'Add to list
			GenresList.Items.Add(itm)

		Next

        GenresList.Sort()
    End Sub


    Public Sub RefreshTVShows()
        TVShowList.Items.Clear()

        'Set an array with the columns you want returned
        Dim SelectArray(2)
        SelectArray(0) = 1
        SelectArray(1) = 15
        SelectArray(2) = 0

        'Shoot it over to the ReadRecord sub, 
        Dim ReturnArray() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM tvshow ORDER BY c00", SelectArray)

        'Now, read the output of the array.
        'Loop through each of the Array items.
        For x = 0 To ReturnArray.Count - 1

            'Split them by ~'s.  This is how we seperate the rows in the single-element.
            Dim str() As String = Split(ReturnArray(x), "~")

            'Now take that split string and make it an item.
            Dim itm As ListViewItem
            itm = New ListViewItem(str)

            'Add the item to the TV show list.
            TVShowList.Items.Add(itm)
        Next
    End Sub

    Private Sub RefreshPlugins()
        PluginType.Items.Clear()

        Dim addonLike As String = "plugin.video"
        Dim SelectArray(0)
        SelectArray(0) = 1

        'Grab the Plugin List
        Dim ReturnArray() As String = ReadPluginRecord(AddonDatabaseLocation, "SELECT DISTINCT addon.addonID, addon.name FROM addon, package WHERE addon.addonID = package.addonID and addon.addonID LIKE '" & addonLike & "%'", SelectArray)



        For x = 0 To ReturnArray.Count - 1

            'Split them by ~'s.  This is how we seperate the rows in the single-element.
            Dim str() As String = Split(ReturnArray(x), "~")

            'Add the item to the Plugins List
            PluginType.Items.Add(str(0))
        Next

    End Sub

    Private Sub PluginType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PluginType.SelectedIndexChanged

        Dim SelectArray(0)
        SelectArray(0) = 0

        Dim ReturnArray() As String = ReadPluginRecord(AddonDatabaseLocation, "SELECT addonID FROM addon WHERE name = '" & PluginType.SelectedItem & "'", SelectArray)

        PlayListLocation.Text = "plugin://" & ReturnArray(0) & ""

    End Sub

    Public Sub RefreshMovieList()

        MovieList.Items.Clear()

        'Set an array with the columns you want returned
        Dim SelectArray(1)
        SelectArray(0) = 2
        SelectArray(1) = 0

        'Shoot it over to the ReadRecord sub, 
        Dim ReturnArray() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM movie ORDER BY c00", SelectArray)

        'Now, read the output of the array.
        'Loop through each of the Array items.
        For x = 0 To ReturnArray.Count - 1

            'Split them by ~'s.  This is how we seperate the rows in the single-element.
            Dim str() As String = Split(ReturnArray(x), "~")

            'Now take that split string and make it an item.
            Dim itm As ListViewItem
            itm = New ListViewItem(str)

            'Add the item to the TV show list.
            MovieList.Items.Add(itm)
        Next
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        HelpList.SelectedIndex = 0

        RefreshSettings()

        TVShowList.Columns.Add("Show", 100, HorizontalAlignment.Left)
        TVShowList.Columns.Add("Network", 100, HorizontalAlignment.Left)
        TVShowList.Columns.Add("ID", 0, HorizontalAlignment.Left)

        MovieList.Columns.Add("Movie", 300, HorizontalAlignment.Left)
        MovieList.Columns.Add("ID", 0, HorizontalAlignment.Left)

        NetworkList.Columns.Add("Network", 140, HorizontalAlignment.Left)
        NetworkList.Columns.Add("# Shows", 60, HorizontalAlignment.Left)

        MovieNetworkList.Columns.Add("Studio", 170, HorizontalAlignment.Left)
        MovieNetworkList.Columns.Add("# Movies", 60, HorizontalAlignment.Left)

        GenresList.Columns.Add("Genre", 100, HorizontalAlignment.Left)
        GenresList.Columns.Add("# Shows", 60, HorizontalAlignment.Center)
        GenresList.Columns.Add("# Movies", 60, HorizontalAlignment.Center)
        GenresList.Columns.Add("# Total", 80, HorizontalAlignment.Center)
        GenresList.Columns.Add("Genre ID", 0, HorizontalAlignment.Left)

        TVGuideList.Columns.Add("Channel", 200, HorizontalAlignment.Left)
        TVGuideList.Columns.Add("Type", 0, HorizontalAlignment.Left)
        TVGuideList.Columns.Add("TypeDetail", 0, HorizontalAlignment.Left)
        TVGuideList.Columns.Add("Time", 0, HorizontalAlignment.Left)
        TVGuideList.Columns.Add("Rules", 0, HorizontalAlignment.Left)
        TVGuideList.Columns.Add("RuleCount", 0, HorizontalAlignment.Left)

        InterleavedList.Columns.Add("Chan", 50, HorizontalAlignment.Left)
        InterleavedList.Columns.Add("Min", 45, HorizontalAlignment.Left)
        InterleavedList.Columns.Add("Max", 45, HorizontalAlignment.Left)
        InterleavedList.Columns.Add("Epi", 45, HorizontalAlignment.Left)
        InterleavedList.Columns.Add("Epis", 45, HorizontalAlignment.Left)

        SchedulingList.Columns.Add("Chan", 53, HorizontalAlignment.Left)
        SchedulingList.Columns.Add("Days", 45, HorizontalAlignment.Left)
        SchedulingList.Columns.Add("Time", 45, HorizontalAlignment.Left)
        SchedulingList.Columns.Add("Epi", 45, HorizontalAlignment.Left)

		TVGuideSubMenu.Columns.Add("Shows / Movies", 300, HorizontalAlignment.Left)

    End Sub

	Public Sub RefreshSettings()

		My.Settings.Reload()
		'Settings.txt location
		'Dim SettingsFile As String = Application.StartupPath() & "\" & "Settings.txt"

		'See if there's already a text file in place, if so load it.
		'    If System.IO.File.Exists(SettingsFile) = True Then
		'        Dim FileLocations = ReadFile(SettingsFile)

		'        'Make sure there's the | symbol there so we can split it
		'        If InStr(FileLocations, " | ") Then
		'            FileLocations = Split(FileLocations, " | ")

		'            'Now count the split and make sure it has the proper amount.
		'            If UBound(FileLocations) = 3 Then

		'                If FileLocations(0) = "0" Then
		'                    'This is for a standard SQLite Entry.
		'                    DatabaseType = 0
		'                    VideoDatabaseLocation = FileLocations(1)
		'                    PseudoTvSettingsLocation = FileLocations(2)
		'		AddonDatabaseLocation = FileLocations(3)
		'		'get Kodi version based on video db version
		'		Version = GetKodiVersion(VideoDatabaseLocation)
		'                Else
		'                    DatabaseType = 1
		'                    MySQLConnectionString = FileLocations(1)
		'                    PseudoTvSettingsLocation = FileLocations(2)
		'                    AddonDatabaseLocation = FileLocations(3)
		'	End If

		'End If

		'            RefreshALL()
		'            RefreshTVGuide()

		'        End If
		If Not My.Settings.VideoDatabaseLocation = "False" And Not String.IsNullOrEmpty(My.Settings.VideoDatabaseLocation) And Not String.IsNullOrEmpty(My.Settings.PseudoTvSettingsLocation) And Not String.IsNullOrEmpty(My.Settings.AddonDatabaseLocation) Then
			DatabaseType = 0
			VideoDatabaseLocation = My.Settings.VideoDatabaseLocation
			PseudoTvSettingsLocation = My.Settings.PseudoTvSettingsLocation
			AddonDatabaseLocation = My.Settings.AddonDatabaseLocation
			Me.Version = My.Settings.Version
			RefreshALL()
			RefreshTVGuide()
		ElseIf Not My.Settings.VideoDatabaseLocation = "False" And Not String.IsNullOrEmpty(My.Settings.MySQLConnectionString) And Not String.IsNullOrEmpty(My.Settings.PseudoTvSettingsLocation) And Not String.IsNullOrEmpty(My.Settings.AddonDatabaseLocation) Then
			DatabaseType = 1
			MySQLConnectionString = My.Settings.MySQLConnectionString
			PseudoTvSettingsLocation = My.Settings.PseudoTvSettingsLocation
			AddonDatabaseLocation = My.Settings.AddonDatabaseLocation
			Me.Version = My.Settings.Version
			RefreshALL()
			RefreshTVGuide()
		Else
			'System.IO.File.Create(SettingsFile)
			MsgBox("Unable to locate the location of XBMC video library and PseudoTV's setting location.  Please enter them and save the changes.")
			Dim mySettings As New Form6
			'mySettings.Version = Me.Version
			mySettings.Show()
		End If

		Form2.Version = Me.Version
		Form3.Version = Me.Version
		Form7.Version = Me.Version
		Form8.Version = Me.Version
	End Sub

    Private Sub ListTVBanners_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListTVBanners.SelectedIndexChanged
        Dim x As Integer = ListTVBanners.SelectedIndex
		If x < 0 Then
			Return
		End If
		If ListTVBanners.Items.Count <= 0 Then
			TVBannerPictureBox.ImageLocation = Application.StartupPath & "\Images\banner.png"
		Else
			TVBannerPictureBox.ImageLocation = ListTVBanners.Items(x)
			TVBannerPictureBox.Refresh()
		End If
	End Sub

    Private Sub TVBannerSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TVBannerSelect.Click
        Dim x As Integer = ListTVBanners.SelectedIndex
        Dim Type As String = "tvshow"
        Dim MediaType As String = "banner"

        If txtShowLocation.TextLength >= 6 Then
            If txtShowLocation.Text.Substring(0, 6) = "smb://" Then
                txtShowLocation.Text = txtShowLocation.Text.Replace("/", "\")
                txtShowLocation.Text = "\\" & txtShowLocation.Text.Substring(6)
            End If
        End If

        ' Displays a SaveFileDialog so the user can save the Image
        ' assigned to TVBannerSelect.
        Dim saveFileDialog1 As New SaveFileDialog()
        saveFileDialog1.InitialDirectory = txtShowLocation.Text
        saveFileDialog1.Filter = "JPeg Image|*.jpg"
        saveFileDialog1.Title = "Save an Image File"
        saveFileDialog1.FileName = "banner.jpg"
        saveFileDialog1.ShowDialog()

        Dim FileToSaveAs As String = System.IO.Path.Combine(txtShowLocation.Text, saveFileDialog1.FileName)
        TVBannerPictureBox.Image.Save(FileToSaveAs, System.Drawing.Imaging.ImageFormat.Jpeg)

        DbExecute("UPDATE art SET url = '" & ListTVBanners.Items(x).ToString & "' WHERE media_id = '" & TVShowLabel.Text & "' and type = '" & Type & "' and type = '" & MediaType & "'")
        Status.Text = "Updated " & TxtShowName.Text & " Successfully with " & ListTVBanners.Items(x).ToString & ""
    End Sub

    Private Sub ListTVPosters_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListTVPosters.SelectedIndexChanged
        Dim x As Integer = ListTVPosters.SelectedIndex
		If x < 0 Then
			Return
		End If
        If ListTVPosters.Items.Count = 0 Then
            TVPosterPictureBox.ImageLocation = Application.StartupPath & "\Images\poster.png"
        Else
            TVPosterPictureBox.ImageLocation = ListTVPosters.Items(x)
            TVPosterPictureBox.Refresh()
        End If

    End Sub
    Private Sub TVPosterSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TVPosterSelect.Click
        Dim x As Integer = ListTVPosters.SelectedIndex
        Dim MediaType As String = "poster"


        If txtShowLocation.TextLength >= 6 Then
            If txtShowLocation.Text.Substring(0, 6) = "smb://" Then
                txtShowLocation.Text = txtShowLocation.Text.Replace("/", "\")
                txtShowLocation.Text = "\\" & txtShowLocation.Text.Substring(6)
            End If
        End If

        ' Displays a SaveFileDialog so the user can save the Image
        ' assigned to TVPosterSelect.
        Dim saveFileDialog1 As New SaveFileDialog()
        saveFileDialog1.InitialDirectory = txtShowLocation.Text
        saveFileDialog1.Filter = "JPeg Image|*.jpg"
        saveFileDialog1.Title = "Save an Image File"
        saveFileDialog1.FileName = "poster.jpg"
        saveFileDialog1.ShowDialog()

        Dim FileToSaveAs As String = System.IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, saveFileDialog1.FileName)
        TVPosterPictureBox.Image.Save(FileToSaveAs, System.Drawing.Imaging.ImageFormat.Jpeg)

        DbExecute("UPDATE art SET url = '" & ListTVPosters.Items(x).ToString & "' WHERE media_id = '" & TVShowLabel.Text & "' and type = '" & MediaType & "'")
        Status.Text = "Updated " & TxtShowName.Text & " Successfully with " & ListTVPosters.Items(x).ToString & ""

    End Sub

    Private Sub TVShowList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TVShowList.SelectedIndexChanged
		If TVShowList.SelectedItems.Count > 0 Then

			Dim ListItem As ListViewItem
			ListItem = TVShowList.SelectedItems.Item(0)

			Dim TVShowName
			Dim TVShowID

			TVShowID = ListItem.SubItems(2).Text
			TVShowName = ListItem.SubItems(0).Text

			TVShowLabel.Text = TVShowID

			Dim TVShowArray(4)
			TVShowArray(0) = 1
			TVShowArray(1) = 9
			TVShowArray(2) = 15
			TVShowArray(3) = 17
			TVShowArray(4) = 7

			'Shoot it over to the ReadRecord sub, 
			Dim TVShowReturnArray() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM tvshow WHERE idShow='" & TVShowID & "'", TVShowArray)

			Dim TVShowReturnArraySplit() As String

			'We only have 1 response, since it searches by ID. So, just break it into parts. 
			TVShowReturnArraySplit = Split(TVShowReturnArray(0), "~")

			Dim TVPathArray(1)
			TVPathArray(0) = 0
			TVPathArray(1) = 1

			'Shoot it over to the ReadRecord sub, 
			Dim TVidPathArray() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM tvshowlinkpath WHERE idShow='" & TVShowID & "'", TVPathArray)

			Dim TVidPathArraySplit() As String

			TVidPathArraySplit = Split(TVidPathArray(0), "~")

			Dim TVShowLocationArray(1)
			TVShowLocationArray(0) = 0
			TVShowLocationArray(1) = 1

			Dim TVShowLocationArrayReturn() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM path WHERE idPath='" & TVidPathArraySplit(1) & "'", TVShowLocationArray)

			Dim TVShowLocationSplit() As String

			TVShowLocationSplit = Split(TVShowLocationArrayReturn(0), "~")

			TxtShowName.Text = TVShowReturnArraySplit(0)

			Dim TVGenres As String = TVShowReturnArraySplit(1)
			If TVShowReturnArraySplit(2) = "" Then
				txtShowNetwork.SelectedIndex = -1
			Else
				txtShowNetwork.Text = TVShowReturnArraySplit(2)
			End If

			txtShowLocation.Text = TVShowLocationSplit(1)

			Dim TVPoster As String = TVShowReturnArraySplit(4)
			ListTVPosters.Items.Clear()

			Dim TVBanner As String = TVShowReturnArraySplit(4)
			ListTVBanners.Items.Clear()

			If InStr(TVPoster, "<thumb aspect=""poster"">") > 0 Then
				Dim TVPosterSplit() As String = Split(TVPoster, "<thumb aspect=""poster"">")

				For X = 1 To UBound(TVPosterSplit)
					Dim i As Integer = TVPosterSplit(X).IndexOf("<thumb aspect=""poster"">")
					TVPosterSplit(X) = TVPosterSplit(X).Substring(i + 1, TVPosterSplit(X).IndexOf("</thumb>"))
					ListTVPosters.Items.Add(TVPosterSplit(X))
				Next
			Else
				ListTVPosters.Items.Add("Nothing Found")
			End If

			If InStr(TVBanner, "<thumb aspect=""banner"">") > 0 Then
				Dim TVBannerSplit() As String = Split(TVBanner, "<thumb aspect=""banner"">")

				For X = 1 To UBound(TVBannerSplit)
					Dim i As Integer = TVBannerSplit(X).IndexOf("<thumb aspect=""banner"">")
					TVBannerSplit(X) = TVBannerSplit(X).Substring(i + 1, TVBannerSplit(X).IndexOf("</thumb>"))
					ListTVBanners.Items.Add(TVBannerSplit(X))
				Next
			Else
				ListTVBanners.Items.Add("Nothing Found")
			End If

			'Loop through each TV Genre, if there more than one.
			ListTVGenres.Items.Clear()
			If InStr(TVGenres, " / ") > 0 Then
				Dim TVGenresSplit() As String = Split(TVGenres, " / ")

				For x = 0 To UBound(TVGenresSplit)
					ListTVGenres.Items.Add(TVGenresSplit(x))
				Next
			ElseIf TVGenres <> "" Then
				ListTVGenres.Items.Add(TVGenres)
			End If

			If txtShowLocation.TextLength >= 6 Then
				If txtShowLocation.Text.Substring(0, 6) = "smb://" Then
					txtShowLocation.Text = txtShowLocation.Text.Replace("/", "\")
					txtShowLocation.Text = "\\" & txtShowLocation.Text.Substring(6)
				End If
			End If

			If System.IO.File.Exists(txtShowLocation.Text & "poster.jpg") Then
				TVPosterPictureBox.ImageLocation = txtShowLocation.Text & "poster.jpg"
			Else
				TVPosterPictureBox.ImageLocation = "https://github.com/Lunatixz/script.pseudotv.live/raw/development/resources/images/poster.png"
			End If

			If System.IO.File.Exists(txtShowLocation.Text & "banner.jpg") Then
				TVBannerPictureBox.ImageLocation = txtShowLocation.Text & "banner.jpg"
			Else
				TVBannerPictureBox.ImageLocation = "https://github.com/Lunatixz/script.pseudotv.live/raw/development/resources/images/banner.png"
			End If
		End If

    End Sub

    Private Sub TVShowList_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles TVShowList.ColumnClick
        ' Get the new sorting column. 
        Dim new_sorting_column As ColumnHeader = TVShowList.Columns(e.Column)
        ' Figure out the new sorting order. 
        Dim sort_order As System.Windows.Forms.SortOrder
        If m_SortingColumn Is Nothing Then
            ' New column. Sort ascending. 
            sort_order = SortOrder.Ascending
        Else ' See if this is the same column. 
            If new_sorting_column.Equals(m_SortingColumn) Then
                ' Same column. Switch the sort order. 
                If m_SortingColumn.Text.StartsWith("> ") Then
                    sort_order = SortOrder.Descending
                Else
                    sort_order = SortOrder.Ascending
                End If
            Else
                ' New column. Sort ascending. 
                sort_order = SortOrder.Ascending
            End If
            ' Remove the old sort indicator. 
            m_SortingColumn.Text = m_SortingColumn.Text.Substring(2)
        End If
        ' Display the new sort order. 
        m_SortingColumn = new_sorting_column
        If sort_order = SortOrder.Ascending Then
            m_SortingColumn.Text = "> " & m_SortingColumn.Text
        Else
            m_SortingColumn.Text = "< " & m_SortingColumn.Text
        End If
        ' Create a comparer. 
        TVShowList.ListViewItemSorter = New clsListviewSorter(e.Column, sort_order)
        ' Sort. 
        TVShowList.Sort()
    End Sub

    Public Function ConvertGenres(ByVal Genrelist As ListBox)
        'Converts the existing ListTVGenre's contents to the proper format.

        Dim TVGenresString As String = ""
        For x = 0 To Genrelist.Items.Count - 1
            If x = 0 Then
                TVGenresString = Genrelist.Items(x).ToString
            Else
                TVGenresString = TVGenresString & " / " & Genrelist.Items(x).ToString
            End If
        Next

        Return TVGenresString
    End Function

    Public Sub RefreshAllGenres()
        Dim SavedText = Option2.Text
        Option2.Items.Clear()
        'Set an array with the columns you want returned
        Dim SelectArray(0)
        SelectArray(0) = 1

        'Shoot it over to the ReadRecord sub, 
        Dim ReturnArray() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM genre", SelectArray)

        'Now, read the output of the array.

        'Loop through each of the Array items.
        For x = 0 To ReturnArray.Count - 1
            Option2.Items.Add(ReturnArray(x))
        Next
        Option2.Sorted = True
        Option2.Text = SavedText
    End Sub

    Public Sub RefreshAllStudios()
        Dim SavedText = Option2.Text

        'Clear all
        Option2.Items.Clear()
		'Form3.ListBox1.Items.Clear()
        txtShowNetwork.Items.Clear()
        txtMovieNetwork.Items.Clear()
        Form8.ListBox1.Items.Clear()

        'Set an array with the columns you want returned
        Dim SelectArray(0)
        SelectArray(0) = 1

        'Shoot it over to the ReadRecord sub, 
        Dim ReturnArray() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM studio", SelectArray)

        'Now, read the output of the array.

        'Loop through each of the Array items.
        For x = 0 To ReturnArray.Count - 1
            Option2.Items.Add(ReturnArray(x))
			'Form3.ListBox1.Items.Add(ReturnArray(x))
            txtShowNetwork.Items.Add(ReturnArray(x))
            txtMovieNetwork.Items.Add(ReturnArray(x))
            Form8.ListBox1.Items.Add(ReturnArray(x))
        Next

        'Sort them all.
        Option2.Sorted = True
		'Form3.ListBox1.Sorted = True
        Form8.ListBox1.Sorted = True
        txtShowNetwork.Sorted = True
        txtMovieNetwork.Sorted = True
        Option2.Text = SavedText

    End Sub

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged

    End Sub

    Private Sub NetworkList_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles NetworkList.ColumnClick
        ' Get the new sorting column. 
        Dim new_sorting_column As ColumnHeader = NetworkList.Columns(e.Column)
        ' Figure out the new sorting order. 
        Dim sort_order As System.Windows.Forms.SortOrder
        If m_SortingColumn Is Nothing Then
            ' New column. Sort ascending. 
            sort_order = SortOrder.Ascending
        Else ' See if this is the same column. 
            If new_sorting_column.Equals(m_SortingColumn) Then
                ' Same column. Switch the sort order. 
                If m_SortingColumn.Text.StartsWith("> ") Then
                    sort_order = SortOrder.Descending
                Else
                    sort_order = SortOrder.Ascending
                End If
            Else
                ' New column. Sort ascending. 
                sort_order = SortOrder.Ascending
            End If
            ' Remove the old sort indicator. 
            m_SortingColumn.Text = m_SortingColumn.Text.Substring(2)
        End If
        ' Display the new sort order. 
        m_SortingColumn = new_sorting_column
        If sort_order = SortOrder.Ascending Then
            m_SortingColumn.Text = "> " & m_SortingColumn.Text
        Else
            m_SortingColumn.Text = "< " & m_SortingColumn.Text
        End If
        ' Create a comparer. 
        NetworkList.ListViewItemSorter = New clsListviewSorter(e.Column, sort_order)
        ' Sort. 
        NetworkList.Sort()
    End Sub

    Private Sub NetworkList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NetworkList.SelectedIndexChanged
		NetworkListSubList.Items.Clear()

        If NetworkList.SelectedIndices.Count > 0 Then

            Dim SelectArray(0)
            SelectArray(0) = 1

            Dim ReturnArray() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM tvshow WHERE c14='" & NetworkList.Items(NetworkList.SelectedIndices(0)).SubItems(0).Text & "'", SelectArray)

            For x = 0 To ReturnArray.Count - 1
                NetworkListSubList.Items.Add(ReturnArray(x))
            Next

        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
		If TVShowList.SelectedIndices.Count > 0 Then
			Form2.Visible = True
			Form2.Focus()
		End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If ListTVGenres.SelectedIndex >= 0 Then

            'Grab the 3rd column from the TVShowList, which is the TVShowID
            Dim GenreID = LookUpGenre(ListTVGenres.Items(ListTVGenres.SelectedIndex).ToString)

            'Now, remove the link in the database.
            'DbExecute("DELETE FROM genrelinktvshow WHERE idGenre = '" & GenreID & "' AND idShow ='" & TVShowList.Items(TVShowList.SelectedIndices(0)).SubItems(2).Text & "'")


            ListTVGenres.Items.RemoveAt(ListTVGenres.SelectedIndex)
            ' SaveTVShow_Click(Nothing, Nothing)
            RefreshGenres()
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        RefreshALL()
    End Sub

    Public Sub RefreshALL()
        If VideoDatabaseLocation <> "" Or MySQLConnectionString <> "" And PseudoTvSettingsLocation <> "" Then
            RefreshMovieList()
            RefreshTVShows()
            RefreshPlugins()
            RefreshAllStudios()
            RefreshNetworkList()
            RefreshNetworkListMovies()
            RefreshGenres()
            TxtShowName.Text = ""
            txtShowLocation.Text = ""
            TVPosterPictureBox.ImageLocation = ""
            MovieLocation.Text = ""
            MoviePicture.ImageLocation = ""
        End If
    End Sub

    Private Sub GenresList_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles GenresList.ColumnClick
        ' Get the new sorting column. 
        Dim new_sorting_column As ColumnHeader = GenresList.Columns(e.Column)
        ' Figure out the new sorting order. 
        Dim sort_order As System.Windows.Forms.SortOrder
        If m_SortingColumn2 Is Nothing Then
            ' New column. Sort ascending. 
            sort_order = SortOrder.Ascending
        Else ' See if this is the same column. 
            If new_sorting_column.Equals(m_SortingColumn2) Then
                ' Same column. Switch the sort order. 
                If m_SortingColumn2.Text.StartsWith("> ") Then
                    sort_order = SortOrder.Descending
                Else
                    sort_order = SortOrder.Ascending
                End If
            Else
                ' New column. Sort ascending. 
                sort_order = SortOrder.Ascending
            End If
            ' Remove the old sort indicator. 
            m_SortingColumn2.Text = m_SortingColumn2.Text.Substring(2)
        End If
        ' Display the new sort order. 
        m_SortingColumn2 = new_sorting_column
        If sort_order = SortOrder.Ascending Then
            m_SortingColumn2.Text = "> " & m_SortingColumn2.Text
        Else
            m_SortingColumn2.Text = "< " & m_SortingColumn2.Text
        End If
        ' Create a comparer. 
        GenresList.ListViewItemSorter = New clsListviewSorter(e.Column, sort_order)
        ' Sort. 
        GenresList.Sort()
    End Sub

    Private Sub GenresList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GenresList.SelectedIndexChanged
        GenresListSubList.Items.Clear()
        GenresListSubListMovies.Items.Clear()

        If GenresList.SelectedIndices.Count > 0 Then
            Dim SelectArray(0)
			SelectArray(0) = 1

			'Now, gather a list of all the show IDs that match the genreID
			Dim ReturnArray() As String
			If (Version >= KodiVersion.Isengard) Then
				ReturnArray = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM genre_link WHERE genre_id='" & GenresList.Items(GenresList.SelectedIndices(0)).SubItems(4).Text & "' AND media_type = 'tvshow'", SelectArray)
			Else
				ReturnArray = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM genrelinktvshow WHERE idGenre='" & GenresList.Items(GenresList.SelectedIndices(0)).SubItems(4).Text & "'", SelectArray)
			End If

			'Now loop through each one individually.

			If ReturnArray Is Nothing Then
			Else
				For x = 0 To ReturnArray.Count - 1
					Dim ShowNameArray(0) As String
					SelectArray(0) = 1

					Dim ReturnArray2() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM tvshow WHERE idShow='" & ReturnArray(x) & "'", SelectArray)

					'Now add that name to the list.
					GenresListSubList.Items.Add(ReturnArray2(0))
				Next
			End If

			'MOVIES REPEAT THIS PROCESS.
			Dim ReturnArrayMovies() As String
			If (Version >= KodiVersion.Isengard) Then
				ReturnArrayMovies = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM genre_link WHERE genre_id='" & GenresList.Items(GenresList.SelectedIndices(0)).SubItems(4).Text & "' AND media_type = 'movie'", SelectArray)
			Else
				ReturnArrayMovies = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM genrelinkmovie WHERE idGenre='" & GenresList.Items(GenresList.SelectedIndices(0)).SubItems(4).Text & "'", SelectArray)
			End If

			'Now loop through each one individually 
			If ReturnArrayMovies Is Nothing Then
			Else
				For x = 0 To ReturnArrayMovies.Count - 1
					Dim ShowNameArray(0) As String
					SelectArray(0) = 1

					Dim ReturnArray2() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM movie WHERE idMovie='" & ReturnArrayMovies(x) & "'", SelectArray)

					'Now add that name to the list.
					GenresListSubListMovies.Items.Add(ReturnArray2(0))
				Next
			End If
		End If

	End Sub

	Private Sub SaveTVShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveTVShow.Click

		If TVShowList.SelectedItems.Count > 0 Then

			' Fix any issues with shows and 's.
			Dim TvShowName As String = TxtShowName.Text
			'Convert show genres into the format ex:  genre1 / genre2 / etc.
			Dim ShowGenres = ConvertGenres(ListTVGenres)
			TvShowName = Replace(TvShowName, "'", "''")
			'Grab the Network ID based on the name
			Dim NetworkID = LookUpNetwork(txtShowNetwork.Text)

			If (Version >= KodiVersion.Isengard) Then
				DbExecute("DELETE FROM studio_link WHERE media_id = '" & TVShowLabel.Text & "'")
				DbExecute("INSERT INTO studio_link(studio_id, media_id, media_type) VALUES ('" & NetworkID & "', '" & TVShowLabel.Text & "', 'tvshow')")
			Else
				DbExecute("DELETE FROM studiolinktvshow WHERE idShow = '" & TVShowLabel.Text & "'")
				DbExecute("INSERT INTO studiolinktvshow (idStudio, idShow) VALUES ('" & NetworkID & "', '" & TVShowLabel.Text & "')")
			End If



			DbExecute("UPDATE tvshow SET c00 = '" & TvShowName & "', c08 = '" & ShowGenres & "', c14 ='" & txtShowNetwork.Text & "' WHERE idShow = '" & TVShowLabel.Text & "'")
			Status.Text = "Updated " & TxtShowName.Text & " Successfully"

			If (Version >= KodiVersion.Isengard) Then
				'Remove all genres from tv show
				DbExecute("DELETE FROM genre_link WHERE media_id = '" & TVShowLabel.Text & "'")
				'add each one.  one by one.
				For x = 0 To ListTVGenres.Items.Count - 1
					Dim GenreID = LookUpGenre(ListTVGenres.Items(x).ToString)
					DbExecute("INSERT INTO genre_link (idGenre, idShow) VALUES ('" & GenreID & "', '" & TVShowLabel.Text & "', 'tvshow')")
				Next
			Else
				'Remove all genres from tv show
				DbExecute("DELETE FROM genrelinktvshow  WHERE idShow = '" & TVShowLabel.Text & "'")

				'add each one.  one by one.
				For x = 0 To ListTVGenres.Items.Count - 1
					Dim GenreID = LookUpGenre(ListTVGenres.Items(x).ToString)
					DbExecute("INSERT INTO genrelinktvshow (idGenre, idShow) VALUES ('" & GenreID & "', '" & TVShowLabel.Text & "')")
				Next
			End If

			

			'Now update the tv show table

			Dim SavedName = txtShowNetwork.Text

			'Refresh Things
			RefreshNetworkList()
			RefreshGenres()

			'Reset the text
			'txtShowNetwork.Text = SavedName

			Dim returnindex = TVShowList.SelectedIndices(0)
			RefreshALL()
			TVShowList.Items(returnindex).Selected = True


		End If

	End Sub



	Public Sub RefreshTVGuideSublist(ByVal ListType As String, ByVal ListValue As String)
		TVGuideSubMenu.Items.Clear()

		Dim TVChannelTypeValue = ListValue

		If ListType = 0 Then
			'Playlist

			'Add Info for PlayList editing/loading.

		ElseIf ListType = 1 Then
			'This is a TV Network.

			'Make sure there's a value in this box.
			If TVChannelTypeValue <> "" Then
				Dim ChannelPreview(0)
				ChannelPreview(0) = 1

				Dim ReturnArray() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM tvshow WHERE c14='" & TVChannelTypeValue & "'", ChannelPreview)

				'Make sure the Array is not null.
				If ReturnArray Is Nothing Then
				Else
					For x = 0 To ReturnArray.Count - 1
						'Add each item it returns to the list.
						TVGuideSubMenu.Items.Add(ReturnArray(x))
					Next
				End If
			End If

		ElseIf ListType = 2 Then
			'Movie Studio

			'Make sure there's a value in this box.
			If TVChannelTypeValue <> "" Then
				Dim SelectArray(0)
				SelectArray(0) = 2


				'Now, gather a list of all the show IDs that match the genreID
				Dim ReturnArray() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM movie WHERE c18='" & TVChannelTypeValue & "'", SelectArray)

				'Now loop through each one individually.
				If ReturnArray Is Nothing Then
				Else
					For x = 0 To ReturnArray.Count - 1
						'Now add that name to the list.
						TVGuideSubMenu.Items.Add(ReturnArray(x))
					Next
				End If
			End If

		ElseIf ListType = 3 Then
			'TV Genre

			'Make sure there's a value in this box.
			If TVChannelTypeValue <> "" Then
				Dim SelectArray(0)
				SelectArray(0) = 1


				'Now, gather a list of all the show IDs that match the genreID
				Dim ReturnArray() As String
				If (Version >= KodiVersion.Isengard) Then
					ReturnArray = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM genre_link WHERE genre_id='" & LookUpGenre(TVChannelTypeValue) & "' AND media_type = 'tvshow'", SelectArray)
				Else
					ReturnArray = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM genrelinktvshow WHERE idGenre='" & LookUpGenre(TVChannelTypeValue) & "'", SelectArray)
				End If


				'Now loop through each one individually.
				For x = 0 To ReturnArray.Count - 1
					Dim ShowNameArray(0)
					ShowNameArray(0) = 1

					Dim ReturnArray2() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM tvshow WHERE idShow='" & ReturnArray(x) & "'", ShowNameArray)

					'Now add that name to the list.
					TVGuideSubMenu.Items.Add(ReturnArray2(0))
				Next
			End If

		ElseIf ListType = 4 Then
			'Movie Genre

			Dim SelectArrayMovies(0)
			SelectArrayMovies(0) = 1

			Dim ReturnArrayMovies() As String
			If (Version >= KodiVersion.Isengard) Then
				ReturnArrayMovies = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM genre_link WHERE genre_id='" & LookUpGenre(TVChannelTypeValue) & "' AND media_type = 'movie'", SelectArrayMovies)
			Else
				ReturnArrayMovies = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM genrelinkmovie WHERE idGenre='" & LookUpGenre(TVChannelTypeValue) & "'", SelectArrayMovies)
			End If

			'Now loop through each one individually.
			If ReturnArrayMovies Is Nothing Then
			Else
				For x = 0 To UBound(ReturnArrayMovies)

					Dim ShowArray(0)
					ShowArray(0) = 2

					Dim ReturnArray2() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM movie WHERE idMovie='" & ReturnArrayMovies(x) & "'", ShowArray)

					'Now add that name to the list.
					TVGuideSubMenu.Items.Add(ReturnArray2(0))
				Next
			End If
		ElseIf ListType = 5 Then
			'Mixed Genre

			'Make sure there's a value in this box.
			If TVChannelTypeValue <> "" Then
				Dim SelectArray(0)
				SelectArray(0) = 1


				'Now, gather a list of all the show IDs that match the genreID

				Dim ReturnArray() As String
				If (Version >= KodiVersion.Isengard) Then
					ReturnArray = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM genre_link WHERE genre_id='" & LookUpGenre(TVChannelTypeValue) & "' AND media_type = 'tvshow'", SelectArray)
				Else
					ReturnArray = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM genrelinktvshow WHERE idGenre='" & LookUpGenre(TVChannelTypeValue) & "'", SelectArray)
				End If

				'Now loop through each one individually.
				If ReturnArray Is Nothing Then
				Else
					For x = 0 To UBound(ReturnArray)
						Dim ShowArray(0)
						ShowArray(0) = 1

						Dim ReturnArray2() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM tvshow WHERE idShow='" & ReturnArray(x) & "'", ShowArray)

						'Now add that name to the list.
						TVGuideSubMenu.Items.Add(ReturnArray2(0))
					Next
				End If
				'------------------------------------
				'Repeat this step for the Movies now.

				Dim SelectArrayMovies(0)
				SelectArrayMovies(0) = 1

				Dim ReturnArrayMovies() As String
				If (Version >= KodiVersion.Isengard) Then
					ReturnArrayMovies = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM genre_link WHERE genre_id='" & LookUpGenre(TVChannelTypeValue) & "' AND media_type = 'movie'", SelectArrayMovies)
				Else
					ReturnArrayMovies = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM genrelinkmovie WHERE idGenre='" & LookUpGenre(TVChannelTypeValue) & "'", SelectArrayMovies)
				End If


				'Now loop through each one individually.
				'Verify it's not NULL.
				If ReturnArrayMovies Is Nothing Then
				Else
					For x = 0 To UBound(ReturnArrayMovies)
						Dim ShowArray(0)
						ShowArray(0) = 2

						Dim ReturnArray2() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM movie WHERE idMovie='" & ReturnArrayMovies(x) & "'", ShowArray)

						'Now add that name to the list.
						TVGuideSubMenu.Items.Add(ReturnArray2(0))
					Next
				End If


			End If

		ElseIf ListType = 6 Then
			'TV Show
		ElseIf ListType = 7 Then
			'Directory

		End If

		'Now loop through all the shows listed to NOT show, compare them to the list of shows and make any of them have a red background if they match.
		For x = 0 To NotShows.Items.Count - 1
			Dim NotShow = NotShows.Items(x).ToString

			For y = 0 To TVGuideSubMenu.Items.Count - 1
				If StrComp(NotShow, TVGuideSubMenu.Items(y).SubItems(0).Text, CompareMethod.Text) = 0 Then
					TVGuideSubMenu.Items(y).BackColor = Color.Red
				End If
			Next

		Next
		TVGuideSubMenu.Sort()
	End Sub

	Private Sub TVGuideList_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles TVGuideList.ColumnClick
		' Get the new sorting column. 
		Dim new_sorting_column As ColumnHeader = TVGuideList.Columns(e.Column)
		' Figure out the new sorting order. 
		Dim sort_order As System.Windows.Forms.SortOrder
		If m_SortingColumn Is Nothing Then
			' New column. Sort ascending. 
			sort_order = SortOrder.Ascending
		Else ' See if this is the same column. 
			If new_sorting_column.Equals(m_SortingColumn) Then
				' Same column. Switch the sort order. 
				If m_SortingColumn.Text.StartsWith("> ") Then
					sort_order = SortOrder.Descending
				Else
					sort_order = SortOrder.Ascending
				End If
			Else
				' New column. Sort ascending. 
				sort_order = SortOrder.Ascending
			End If
			' Remove the old sort indicator. 
			m_SortingColumn.Text = m_SortingColumn.Text.Substring(2)
		End If
		' Display the new sort order. 
		m_SortingColumn = new_sorting_column
		If sort_order = SortOrder.Ascending Then
			m_SortingColumn.Text = "> " & m_SortingColumn.Text
		Else
			m_SortingColumn.Text = "< " & m_SortingColumn.Text
		End If
		' Create a comparer. 
		TVGuideList.ListViewItemSorter = New clsListviewSorter(e.Column, sort_order)
		' Sort. 
		TVGuideList.Sort()
	End Sub


	Private Sub TVGuideList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TVGuideList.SelectedIndexChanged
		If TVGuideList.SelectedIndices.Count > 0 Then



			'Reset the checked options.
			ChkLogo.Checked = False
			chkDontPlayChannel.Checked = False
			ChkRandom.Checked = False
			ChkRealTime.Checked = False
			ChkResume.Checked = False
			ChkIceLibrary.Checked = False
			ChkExcludeBCT.Checked = False
			ChkPopup.Checked = False
			ChkUnwatched.Checked = False
			ChkWatched.Checked = False
			ChkPause.Checked = False
			ChkPlayInOrder.Checked = False
			ResetDays.Clear()
			ChannelName.Clear()

			'Clear other form items.
			TVGuideSubMenu.Items.Clear()
			Dim PlayListNumber = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(1).Text
			Option2.Items.Clear()
			InterleavedList.Items.Clear()
			SchedulingList.Items.Clear()
			NotShows.Items.Clear()

			'Display the Channel Number.
			TVGuideShowName.Text = "Channel " & TVGuideList.SelectedItems(0).SubItems(0).Text

			If PlayListNumber <> 9999 Then

				PlayListType.SelectedIndex = PlayListNumber

				Dim Option1 As Integer = 0

				Dim TVChannelTypeValue = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(2).Text

				If PlayListNumber = 0 Then
					'Playlist
					Option1 = 1

					'Add Info for PlayList editing/loading.

				ElseIf PlayListNumber = 1 Then
					'This is a TV Network.

					For x = 0 To NetworkList.Items.Count - 1
						Option2.Items.Add(NetworkList.Items(x).SubItems(0).Text)
					Next

					'Make sure there's a value in this box.
					If TVChannelTypeValue <> "" Then
						RefreshTVGuideSublist(PlayListNumber, TVChannelTypeValue)
					End If

				ElseIf PlayListNumber = 2 Then
					'Movie Studio
					RefreshAllStudios()

				ElseIf PlayListNumber = 3 Then
					'TV Genre
					For x = 0 To GenresList.Items.Count - 1
						Option2.Items.Add(GenresList.Items(x).SubItems(0).Text)
					Next

				ElseIf PlayListNumber = 4 Then
					'Movie Genre
					RefreshAllGenres()

				ElseIf PlayListNumber = 5 Then
					'Mixed Genre
					RefreshAllGenres()

				ElseIf PlayListNumber = 6 Then
					'TV Show
					For x = 0 To TVShowList.Items.Count - 1
						Option2.Items.Add(TVShowList.Items(x).SubItems(0).Text)
					Next
				ElseIf PlayListNumber = 7 Then
					'Directory
					Option1 = 1
				ElseIf PlayListNumber = 8 Then
					'LiveTV
					Option1 = 2
				ElseIf PlayListNumber = 9 Then
					'InternetTV
					Option1 = 3
				ElseIf PlayListNumber = 10 Or PlayListNumber = 11 Then
					'YoutubeTV or RSS
					Option1 = 4
				ElseIf PlayListNumber = 13 Then
					'Music Videos
					Option1 = 5
				ElseIf PlayListNumber = 14 Then
					'Extras
					Option1 = 6
				ElseIf PlayListNumber = 15 Then
					'Direct Plugin Type
					Option1 = 7
				End If

				'Now, we loop through the advanced rules to populate those properly.

				'break this array into all the rules for this channel.
				Dim AllRules = Split(TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(4).Text, "~")

				'Loop through all of them.
				'But, only the ones it "says" it has.

				Dim RuleCount As Integer


				If TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(5).Text <> "" Then
					RuleCount = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(5).Text
				End If


				For y = 1 To RuleCount
					'For y = 1 To UBound(AllRules)
					Dim RuleSettings() = Split(AllRules(y), "|")

					'Rule #1, #2, etc.
					Dim RuleNum = RuleSettings(0)
					'Value, most of the time this is the only thing we need.
					Dim RuleValue = RuleSettings(1)


					If RuleValue = 5 Then
						chkDontPlayChannel.Checked = True
					ElseIf RuleValue = 10 Then
						ChkRandom.Checked = True
					ElseIf RuleValue = 7 Then
						ChkRealTime.Checked = True
					ElseIf RuleValue = 9 Then
						ChkResume.Checked = True
					ElseIf RuleValue = 11 Then
						ChkUnwatched.Checked = True
					ElseIf RuleValue = 4 Then
						ChkWatched.Checked = True
					ElseIf RuleValue = 8 Then
						ChkPause.Checked = True
					ElseIf RuleValue = 12 Then
						ChkPlayInOrder.Checked = True
					Else

						'Okay, so it's not something requiring a single option.

						'Now loop through all the sub-options of each rule.
						Dim SubOptions(4) As String

						For z = 2 To UBound(RuleSettings)
							Dim OptionNum = Split(RuleSettings(z), "^")(0)
							Dim OptionValue = Split(RuleSettings(z), "^")(1)


							If RuleValue = 13 Then
								'MsgBox(RuleValue)
								ResetDays.Text = (OptionValue / 60)
								Exit For
							ElseIf RuleValue = 1 Then
								ChannelName.Text = OptionValue
								Exit For
							ElseIf RuleValue = 15 And OptionValue = "Yes" Then
								ChkLogo.Checked = True
								Exit For
							ElseIf RuleValue = 14 And OptionValue = "Yes" Then
								ChkIceLibrary.Checked = True
								Exit For
							ElseIf RuleValue = 17 And OptionValue = "Yes" Then
								ChkExcludeBCT.Checked = True
								Exit For
							ElseIf RuleValue = 18 And OptionValue = "Yes" Then
								ChkPopup.Checked = True
								Exit For
							ElseIf RuleValue = 2 Then
								NotShows.Items.Add(OptionValue)
								Exit For
							ElseIf RuleValue = 6 Then
								'Add this option to a sub-item array to add later to the
								'Object at the end
								SubOptions(OptionNum - 1) = OptionValue

								If OptionNum = 5 Then
									'last option.
									'create + insert object
									Dim itm As ListViewItem
									itm = New ListViewItem(SubOptions)
									'Add to list
									InterleavedList.Items.Add(itm)
									'Remove it from the loop.  We only need 4 options here.
									Exit For
								Else

								End If
							ElseIf RuleValue = 3 Then
								'Add this option to a sub-item array to add later to the
								'Object at the end
								SubOptions(OptionNum - 1) = OptionValue
								If OptionNum = 4 Then
									'last option.
									'create + insert object
									Dim itm As ListViewItem
									itm = New ListViewItem(SubOptions)
									'Add to list

									SchedulingList.Items.Add(itm)
									Exit For
								Else

								End If
							End If

						Next
					End If
				Next


				RefreshTVGuideSublist(PlayListNumber, TVChannelTypeValue)

				If Option1 = 1 Then
					PlayListLocation.Text = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(2).Text
					SortTypeBox.SelectedIndex = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(8).Text
					Dim index As Integer
					index = MediaLimitBox.FindString(TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(7).Text)
					MediaLimitBox.SelectedIndex = index
				ElseIf Option1 = 2 Then
					PlayListLocation.Text = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(2).Text
					StrmUrlBox.Text = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(6).Text
					ShowTitleBox.Text = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(7).Text
				ElseIf Option1 = 3 Then
					PlayListLocation.Text = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(2).Text
					StrmUrlBox.Text = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(6).Text
					ShowTitleBox.Text = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(7).Text
					ShowDescBox.Text = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(8).Text
				ElseIf Option1 = 4 Then
					PlayListLocation.Text = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(2).Text
					YouTubeType.SelectedIndex = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(6).Text - 1
					Dim index As Integer
					index = MediaLimitBox.FindString(TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(7).Text)
					MediaLimitBox.SelectedIndex = index
					SortTypeBox.SelectedIndex = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(8).Text
					If YouTubeType.SelectedIndex = 6 Or YouTubeType.SelectedIndex = 7 Then
						Dim ReturnMulti As String = ""

						ReturnMulti = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(2).Text

						If InStr(ReturnMulti, "|") >= 0 Then
							Dim ReturnMultiSplit() As String = Split(ReturnMulti, "|")

							For X = 0 To UBound(ReturnMultiSplit)
								NotShows.Items.Add(ReturnMultiSplit(X))
							Next

						Else

							ShowDescBox.Visible = True
							ShowDescBox.Text = ReturnMulti

						End If

						YouTubeMulti = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(2).Text

					End If

				ElseIf Option1 = 5 Then
					PlayListLocation.Text = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(6).Text
					SubChannelType.SelectedIndex = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(6).Text - 1
					Dim index As Integer
					index = MediaLimitBox.FindString(TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(7).Text)
					MediaLimitBox.SelectedIndex = index
					SortTypeBox.SelectedIndex = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(8).Text
				ElseIf Option1 = 7 Then
					PlayListLocation.Text = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(2).Text
					SortTypeBox.SelectedIndex = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(8).Text
					Dim index As Integer
					index = MediaLimitBox.FindString(TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(7).Text)
					MediaLimitBox.SelectedIndex = index

					Dim ReturnPlugin As String = ""

					Dim SelectArray(0)
					SelectArray(0) = 0

					ReturnPlugin = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(2).Text

					Dim ReturnProperPlugin As String = ReturnPlugin.Remove(0, 9)

					Dim ReturnArray() As String = ReadPluginRecord(AddonDatabaseLocation, "SELECT name FROM addon WHERE addonID = '" & ReturnProperPlugin & "'", SelectArray)

					Dim index2 As Integer
					index2 = PluginType.FindString(ReturnArray(0))
					PluginType.SelectedIndex = index2

					PluginNotInclude = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(6).Text

					If InStr(PluginNotInclude, ",") >= 0 Then
						Dim PluginNotIncludeSplit() As String = Split(PluginNotInclude, ",")

						For X = 0 To UBound(PluginNotIncludeSplit)
							NotShows.Items.Add(PluginNotIncludeSplit(X))
						Next

					Else

					End If

				Else
					Option2.Text = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(2).Text
				End If
				TVGuideSubMenu.Sort()

			End If
		End If
	End Sub


	Private Sub Option2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Option2.SelectedIndexChanged
		If PlayListType.SelectedIndex >= 0 And Option2.Text <> "" Then
			RefreshTVGuideSublist(PlayListType.SelectedIndex, Option2.Text)
		End If

	End Sub

	Private Sub PlayListType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayListType.SelectedIndexChanged
		'Clear the Sub-menu
		TVGuideSubMenu.Items.Clear()
		With PlayListLocation
			.Location = New System.Drawing.Point(267, 120)
		End With
		PlayListLocation.Text = ""
		Label6.Visible = False
		Label6.Text = ""
		Button5.Visible = False
		PlayListLocation.Visible = False
		PluginType.Visible = False
		StrmUrl.Visible = False
		StrmUrlBox.Visible = False
		Option2.Visible = False
		YouTubeType.Visible = False
		ShowTitle.Visible = False
		ShowTitleBox.Visible = False
		ShowDesc.Visible = False
		ShowDescBox.Visible = False
		MediaLimit.Visible = False
		MediaLimitBox.Visible = False
		SortType.Visible = False
		SortTypeBox.Visible = False
		SubChannelType.Visible = False
		TVGuideSubMenu.Visible = False
		InterleavedList.Visible = False
		Label7.Visible = False
		SchedulingList.Visible = False
		Label11.Visible = False
		NotShows.Visible = False
		Label12.Visible = False
		AddExcludeBtn.Visible = False
		DelExcludeBtn.Visible = False
		PluginNotInclude = ""

		If PlayListType.SelectedIndex = 0 Then
			Button5.Visible = True
			Label6.Text = "Location:"
			Label6.Visible = True
			PlayListLocation.Visible = True
		ElseIf PlayListType.SelectedIndex = 1 Or PlayListType.SelectedIndex = 2 Or PlayListType.SelectedIndex = 3 Or PlayListType.SelectedIndex = 4 Or PlayListType.SelectedIndex = 5 Or PlayListType.SelectedIndex = 6 Then
			Option2.Visible = True
			TVGuideSubMenu.Visible = True
			InterleavedList.Visible = True
			Label7.Visible = True
			SchedulingList.Visible = True
			Label11.Visible = True
			NotShows.Visible = True
			Label12.Visible = True
			Button8.Visible = True
			Button9.Visible = True
			Button10.Visible = True
			Button11.Visible = True
			Button12.Visible = True
		ElseIf PlayListType.SelectedIndex = 7 Then
			Label6.Text = "Location:"
			Label6.Visible = True
			PlayListLocation.Visible = True
			With PlayListLocation
				.Location = New System.Drawing.Point(270, 120)
			End With
			PlayListLocation.Visible = True
			With MediaLimit
				.Location = New System.Drawing.Point(160, 160)
			End With
			MediaLimit.Visible = True
			With MediaLimitBox
				.Location = New System.Drawing.Point(162, 180)
			End With
			MediaLimitBox.SelectedIndex = 0
			MediaLimitBox.Visible = True
			With SortType
				.Location = New System.Drawing.Point(225, 160)
			End With
			SortType.Visible = True
			With SortTypeBox
				.Location = New System.Drawing.Point(227, 180)
			End With
			SortTypeBox.SelectedIndex = 0
			SortTypeBox.Visible = True
			NotShows.Visible = True
			NotShows.Visible = False
			AddExcludeBtn.Visible = False
			DelExcludeBtn.Visible = False
			Label12.Visible = False
			Button5.Visible = True
		ElseIf PlayListType.SelectedIndex = 8 Then
			Label6.Text = "Channel id:"
			Label6.Visible = True
			PlayListLocation.Visible = True
			StrmUrl.Visible = True
			StrmUrlBox.Visible = True
			ShowTitle.Text = "XMLTV Filename:"
			ShowTitle.Visible = True
			ShowTitleBox.Visible = True
		ElseIf PlayListType.SelectedIndex = 9 Then
			Label6.Text = "Duration:"
			Label6.Visible = True
			PlayListLocation.Visible = True
			StrmUrl.Text = "Source path:"
			StrmUrl.Visible = True
			StrmUrlBox.Visible = True
		ElseIf PlayListType.SelectedIndex = 10 Then
			NotShows.Items.Clear()
			Button5.Visible = False
			YouTubeType.SelectedIndex = 0
			YouTubeType.Visible = True
			Label6.Text = "Channel/User:"
			Label6.Visible = True
			With PlayListLocation
				.Location = New System.Drawing.Point(270, 120)
			End With
			PlayListLocation.Visible = True
			With MediaLimit
				.Location = New System.Drawing.Point(160, 160)
			End With
			MediaLimit.Visible = True
			With MediaLimitBox
				.Location = New System.Drawing.Point(162, 180)
			End With
			MediaLimitBox.SelectedIndex = 0
			MediaLimitBox.Visible = True
			With SortType
				.Location = New System.Drawing.Point(225, 160)
			End With
			SortType.Visible = True
			With SortTypeBox
				.Location = New System.Drawing.Point(227, 180)
			End With
			SortTypeBox.SelectedIndex = 0
			SortTypeBox.Visible = True
			NotShows.Visible = True
			NotShows.Visible = False
			AddExcludeBtn.Visible = False
			DelExcludeBtn.Visible = False
			Label12.Visible = False
		ElseIf PlayListType.SelectedIndex = 11 Then
			Label6.Text = "Source path:"
			Label6.Visible = True
			PlayListLocation.Visible = True
			With MediaLimit
				.Location = New System.Drawing.Point(160, 160)
			End With
			MediaLimit.Visible = True
			With MediaLimitBox
				.Location = New System.Drawing.Point(162, 180)
			End With
			MediaLimitBox.SelectedIndex = 0
			MediaLimitBox.Visible = True
			With SortType
				.Location = New System.Drawing.Point(225, 160)
			End With
			SortType.Visible = True
			With SortTypeBox
				.Location = New System.Drawing.Point(227, 180)
			End With
			SortTypeBox.SelectedIndex = 0
			SortTypeBox.Visible = True
			Button8.Visible = False
			Button9.Visible = False
			Button10.Visible = False
			Button11.Visible = False
			Button12.Visible = False
		ElseIf PlayListType.SelectedIndex = 13 Then
			Label6.Text = "LastFM Username:"
			Label6.Visible = True
			PlayListLocation.Visible = True
			With MediaLimit
				.Location = New System.Drawing.Point(160, 160)
			End With
			MediaLimit.Visible = True
			With MediaLimitBox
				.Location = New System.Drawing.Point(162, 180)
			End With
			MediaLimitBox.SelectedIndex = 0
			MediaLimitBox.Visible = True
			With SortType
				.Location = New System.Drawing.Point(225, 160)
			End With
			SortType.Visible = True
			With SortTypeBox
				.Location = New System.Drawing.Point(227, 180)
			End With
			SortTypeBox.SelectedIndex = 0
			SortTypeBox.Visible = True
			SubChannelType.Items.Clear()
			With SubChannelType.Items
				.Add("LastFM")
				.Add("MyMusicTV")
			End With
			SubChannelType.Visible = True
			SubChannelType.SelectedIndex = 0
		ElseIf PlayListType.SelectedIndex = 14 Then
			Label6.Visible = False
			With MediaLimit
				.Location = New System.Drawing.Point(160, 160)
			End With
			MediaLimit.Visible = True
			With MediaLimitBox
				.Location = New System.Drawing.Point(162, 180)
			End With
			MediaLimitBox.SelectedIndex = 0
			MediaLimitBox.Visible = True
			With SortType
				.Location = New System.Drawing.Point(225, 160)
			End With
			SortType.Visible = True
			With SortTypeBox
				.Location = New System.Drawing.Point(227, 180)
			End With
			SortTypeBox.SelectedIndex = 0
			SortTypeBox.Visible = True
			SubChannelType.Items.Clear()
			With SubChannelType.Items
				.Add("popcorn")
				.Add("cinema")
			End With
			SubChannelType.Visible = True
			SubChannelType.SelectedIndex = 0
		ElseIf PlayListType.SelectedIndex = 15 Then
			NotShows.Items.Clear()
			Label6.Visible = False
			With PlayListLocation
				.Location = New System.Drawing.Point(220, 120)
			End With
			PlayListLocation.Visible = True
			With PluginType
				.Location = New System.Drawing.Point(227, 86)
			End With
			PluginType.Visible = True
			With MediaLimit
				.Location = New System.Drawing.Point(160, 160)
			End With
			MediaLimit.Visible = True
			With MediaLimitBox
				.Location = New System.Drawing.Point(162, 180)
			End With
			MediaLimitBox.SelectedIndex = 0
			MediaLimitBox.Visible = True
			With SortType
				.Location = New System.Drawing.Point(225, 160)
			End With
			SortType.Visible = True
			With SortTypeBox
				.Location = New System.Drawing.Point(227, 180)
			End With
			SortTypeBox.SelectedIndex = 0
			SortTypeBox.Visible = True
			NotShows.Visible = True
			Label12.Text = "Do not include these items"
			Label12.Visible = True
			AddExcludeBtn.Visible = True
			DelExcludeBtn.Visible = True
			Button8.Visible = False
			Button9.Visible = False
			Button10.Visible = False
			Button11.Visible = False
			Button12.Visible = False

		Else
			Option2.Visible = True
			PlayListLocation.Visible = False

		End If

		Option2.Items.Clear()
		Option2.Text = ""


		If PlayListType.SelectedIndex = 0 Then
			For x = 0 To NetworkList.Items.Count - 1
				Option2.Items.Add(NetworkList.Items(x).SubItems(0).Text)
			Next
		ElseIf PlayListType.SelectedIndex = 1 Then
			For x = 0 To NetworkList.Items.Count - 1
				Option2.Items.Add(NetworkList.Items(x).SubItems(0).Text)
			Next
		ElseIf PlayListType.SelectedIndex = 2 Then
			RefreshAllStudios()
		ElseIf PlayListType.SelectedIndex = 3 Then
			For x = 0 To GenresList.Items.Count - 1
				Option2.Items.Add(GenresList.Items(x).SubItems(0).Text)
			Next
		ElseIf PlayListType.SelectedIndex = 4 Then
			RefreshAllGenres()
		ElseIf PlayListType.SelectedIndex = 5 Then
			RefreshAllGenres()
		ElseIf PlayListType.SelectedIndex = 6 Then
			For x = 0 To TVShowList.Items.Count - 1
				Option2.Items.Add(TVShowList.Items(x).SubItems(0).Text)
			Next
		End If

	End Sub

	Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click


		If PlayListType.Text = "Directory" Then
			FolderBrowserDialog1.ShowDialog()
			PlayListLocation.Text = FolderBrowserDialog1.SelectedPath
		ElseIf PlayListType.Text = "Playlist" Then
			OpenFileDialog1.ShowDialog()

			Dim Filename = OpenFileDialog1.FileName
			Dim FilenameSplit = Split(Filename, "\")

			PlayListLocation.Text = "special://profile/playlists/video/" & FilenameSplit(UBound(FilenameSplit))


		End If


	End Sub

	Private Sub RefreshButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		'Loop through config file.
		'Grab all comments MINUS the ones for selected #
		'Append this & our new content to the file.

		If TVGuideList.SelectedItems.Count > 0 Then

			Dim FILE_NAME As String = PseudoTvSettingsLocation
			Dim TextFile As String = ""

			Dim ChannelNum = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(0).Text


			'Loop through config file.
			'Grab all comments MINUS the ones for selected #
			If System.IO.File.Exists(FILE_NAME) = True Then

				Dim objReader As New System.IO.StreamReader(FILE_NAME)

				Do While objReader.Peek() <> -1

					Dim SingleLine = objReader.ReadLine()

					If InStr(SingleLine, "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_") Or InStr(SingleLine, "</settings", CompareMethod.Text) Then
					Else
						TextFile = TextFile & SingleLine & vbNewLine
					End If

				Loop

				objReader.Close()
			Else

				MsgBox("File Does Not Exist")

			End If

			Dim returnindex = TVGuideList.SelectedIndices(0)
			RefreshTVGuide()
			TVGuideList.Items(returnindex).Selected = True

		End If
	End Sub

	Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
		'Loop through config file.
		'Grab all comments MINUS the ones for selected #
		'Append this & our new content to the file.

		SaveExcludeBtn.Visible = False

		If TVGuideList.SelectedItems.Count > 0 Then

			Dim FILE_NAME As String = PseudoTvSettingsLocation
			Dim TextFile As String = ""

			Dim ChannelNum = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(0).Text


			'Loop through config file.
			'Grab all comments MINUS the ones for selected #
			If System.IO.File.Exists(FILE_NAME) = True Then

				Dim objReader As New System.IO.StreamReader(FILE_NAME)

				Do While objReader.Peek() <> -1

					Dim SingleLine = objReader.ReadLine()

					If InStr(SingleLine, "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_") Or InStr(SingleLine, "</settings", CompareMethod.Text) Then
					Else
						TextFile = TextFile & SingleLine & vbNewLine
					End If

				Loop

				objReader.Close()
			Else

				MsgBox("File Does Not Exist")

			End If

			'Now, append info for this channel we're editing.

			Dim AppendInfo As String = ""
			Dim rulecount = 0

			'Show the Logo is checked.
			'<setting id="Channel_1_rule_1_id" value="15" />
			If ChkLogo.Checked = True Then
				rulecount = rulecount + 1
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_id" & Chr(34) & " value=" & Chr(34) & "15" & Chr(34) & " />"
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_opt_1" & Chr(34) & " value=" & Chr(34) & "Yes" & Chr(34) & " />"
			End If

			'Don't show this channel is checked
			'<setting id="Channel_1_rule_1_id" value="5" />
			If chkDontPlayChannel.Checked = True Then
				rulecount = rulecount + 1
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_id" & Chr(34) & " value=" & Chr(34) & "5" & Chr(34) & " />"
			End If

			'Play Random Mode
			'<setting id="Channel_1_rule_1_id" value="10" />
			If ChkRandom.Checked = True Then
				rulecount = rulecount + 1
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_id" & Chr(34) & " value=" & Chr(34) & "10" & Chr(34) & " />"
			End If

			'Play Real-Time Mode
			'<setting id="Channel_1_rule_1_id" value="7" />
			If ChkRealTime.Checked = True Then
				rulecount = rulecount + 1
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_id" & Chr(34) & " value=" & Chr(34) & "7" & Chr(34) & " />"
			End If

			'Play Resume Mode
			'<setting id="Channel_1_rule_1_id" value="9" />
			If ChkResume.Checked = True Then
				rulecount = rulecount + 1
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_id" & Chr(34) & " value=" & Chr(34) & "9" & Chr(34) & " />"
			End If

			'Play Only Unwatched Films
			'<setting id="Channel_1_rule_1_id" value="11" />
			If ChkUnwatched.Checked = True Then
				rulecount = rulecount + 1
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_id" & Chr(34) & " value=" & Chr(34) & "11" & Chr(34) & " />"
			End If

			'Only play Watched
			'<setting id="Channel_1_rule_1_id" value="4" />
			If ChkWatched.Checked = True Then
				rulecount = rulecount + 1
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_id" & Chr(34) & " value=" & Chr(34) & "4" & Chr(34) & " />"
			End If

			'Exclude Strms?
			'<setting id="Channel_1_rule_1_id" value="14" />
			If ChkIceLibrary.Checked = True Then
				rulecount = rulecount + 1
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_id" & Chr(34) & " value=" & Chr(34) & "14" & Chr(34) & " />"
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_opt_1" & Chr(34) & " value=" & Chr(34) & "No" & Chr(34) & " />"
			End If

			'Exclude BCT?
			'<setting id="Channel_1_rule_1_id" value="17" />
			If ChkExcludeBCT.Checked = True Then
				rulecount = rulecount + 1
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_id" & Chr(34) & " value=" & Chr(34) & "17" & Chr(34) & " />"
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_opt_1" & Chr(34) & " value=" & Chr(34) & "No" & Chr(34) & " />"
			End If

			'Disable Popup?
			'<setting id="Channel_1_rule_1_id" value="18" />
			If ChkPopup.Checked = True Then
				rulecount = rulecount + 1
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_id" & Chr(34) & " value=" & Chr(34) & "18" & Chr(34) & " />"
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_opt_1" & Chr(34) & " value=" & Chr(34) & "No" & Chr(34) & " />"
			End If

			'Pause when not watching
			'<setting id="Channel_1_rule_1_id" value="8" />
			If ChkPause.Checked = True Then
				rulecount = rulecount + 1
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_id" & Chr(34) & " value=" & Chr(34) & "8" & Chr(34) & " />"
			End If

			'Play shows in order
			'<setting id="Channel_1_rule_1_id" value="12" />
			If ChkPlayInOrder.Checked = True Then
				rulecount = rulecount + 1
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_id" & Chr(34) & " value=" & Chr(34) & "12" & Chr(34) & " />"
			End If

			'Theres a # in the reset day amount
			'<setting id="Channel_1_rule_1_id" value="13" />
			'<setting id="Channel_1_rule_1_opt_1" value=ResetDays />

			If IsNumeric(ResetDays.Text) Then
				ResetHours = (Convert.ToInt32(ResetDays.Text) * 60)
				rulecount = rulecount + 1
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_id" & Chr(34) & " value=" & Chr(34) & "13" & Chr(34) & " />"
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_opt_1" & Chr(34) & " value=" & Chr(34) & ResetHours & Chr(34) & " />"
			End If

			'Theres a channel name
			'<setting id="Channel_1_rule_1_id" value="1" />
			'<setting id="Channel_1_rule_1_opt_1" value=ChannelName />
			If ChannelName.Text <> "" Then
				rulecount = rulecount + 1
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_id" & Chr(34) & " value=" & Chr(34) & "1" & Chr(34) & " />"
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_opt_1" & Chr(34) & " value=" & Chr(34) & ChannelName.Text & Chr(34) & " />"
			End If

			'Loop through shows not to play
			'<setting id="Channel_1_rule_1_id" value="2" />
			'<setting id="Channel_1_rule_1_opt_1" value=ShowName />
			If PlayListType.SelectedIndex = 15 Or PlayListType.SelectedIndex = 10 Then
			Else
				For x = 0 To NotShows.Items.Count - 1
					rulecount = rulecount + 1
					AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_id" & Chr(34) & " value=" & Chr(34) & "2" & Chr(34) & " />"
					AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_opt_1" & Chr(34) & " value=" & Chr(34) & NotShows.Items(x) & Chr(34) & " />"
				Next
			End If
			'Interleaved loop
			For x = 0 To InterleavedList.Items.Count - 1
				rulecount = rulecount + 1
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_id" & Chr(34) & " value=" & Chr(34) & "6" & Chr(34) & " />"
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_opt_1" & Chr(34) & " value=" & Chr(34) & InterleavedList.Items(x).SubItems(0).Text & Chr(34) & " />"
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_opt_2" & Chr(34) & " value=" & Chr(34) & InterleavedList.Items(x).SubItems(1).Text & Chr(34) & " />"
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_opt_3" & Chr(34) & " value=" & Chr(34) & InterleavedList.Items(x).SubItems(2).Text & Chr(34) & " />"
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_opt_4" & Chr(34) & " value=" & Chr(34) & InterleavedList.Items(x).SubItems(3).Text & Chr(34) & " />"
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_opt_5" & Chr(34) & " value=" & Chr(34) & InterleavedList.Items(x).SubItems(4).Text & Chr(34) & " />"
			Next

			For x = 0 To SchedulingList.Items.Count - 1
				rulecount = rulecount + 1
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_id" & Chr(34) & " value=" & Chr(34) & "3" & Chr(34) & " />"
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_opt_1" & Chr(34) & " value=" & Chr(34) & SchedulingList.Items(x).SubItems(0).Text & Chr(34) & " />"
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_opt_2" & Chr(34) & " value=" & Chr(34) & SchedulingList.Items(x).SubItems(1).Text & Chr(34) & " />"
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_opt_3" & Chr(34) & " value=" & Chr(34) & SchedulingList.Items(x).SubItems(2).Text & Chr(34) & " />"
				AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rule_" & rulecount & "_opt_4" & Chr(34) & " value=" & Chr(34) & SchedulingList.Items(x).SubItems(3).Text & Chr(34) & " />"
			Next

			'Update it has been changed to flag it?
			'<setting id="Channel_1_changed" value="True" />
			AppendInfo = AppendInfo & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_changed" & Chr(34) & " value=" & Chr(34) & "True" & Chr(34) & " />"

			'Add type of channel to the top.
			Dim TopAppend
			TopAppend = vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_type" & Chr(34) & " value=" & Chr(34) & PlayListType.SelectedIndex & Chr(34) & " />"

			If PlayListType.SelectedIndex = 0 Then
				TopAppend = TopAppend & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_1" & Chr(34) & " value=" & Chr(34) & PlayListLocation.Text & Chr(34) & " />"
			ElseIf PlayListType.SelectedIndex = 7 Then
				TopAppend = TopAppend & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_1" & Chr(34) & " value=" & Chr(34) & PlayListLocation.Text & Chr(34) & " />" & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_3" & Chr(34) & " value=" & Chr(34) & MediaLimitBox.Text & Chr(34) & " />" & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_4" & Chr(34) & " value=" & Chr(34) & SortTypeBox.SelectedIndex & Chr(34) & " />"
			ElseIf PlayListType.SelectedIndex = 8 Or PlayListType.SelectedIndex = 9 Then
				TopAppend = TopAppend & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_1" & Chr(34) & " value=" & Chr(34) & PlayListLocation.Text & Chr(34) & " />" & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_2" & Chr(34) & " value=" & Chr(34) & StrmUrlBox.Text & Chr(34) & " />" & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_3" & Chr(34) & " value=" & Chr(34) & ShowTitleBox.Text & Chr(34) & " />" & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_4" & Chr(34) & " value=" & Chr(34) & ShowDescBox.Text & Chr(34) & " />"
			ElseIf PlayListType.SelectedIndex = 10 Then
				If YouTubeType.SelectedIndex = 6 Or YouTubeType.SelectedIndex = 7 Then
					PlayListLocation.Text = YouTubeMulti
				Else
				End If
				TopAppend = TopAppend & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_1" & Chr(34) & " value=" & Chr(34) & PlayListLocation.Text & Chr(34) & " />" & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_2" & Chr(34) & " value=" & Chr(34) & YouTubeType.SelectedIndex + 1 & Chr(34) & " />" & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_3" & Chr(34) & " value=" & Chr(34) & MediaLimitBox.Text & Chr(34) & " />" & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_4" & Chr(34) & " value=" & Chr(34) & SortTypeBox.SelectedIndex & Chr(34) & " />"
			ElseIf PlayListType.SelectedIndex = 11 Then
				TopAppend = TopAppend & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_1" & Chr(34) & " value=" & Chr(34) & PlayListLocation.Text & Chr(34) & " />" & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_2" & Chr(34) & " value=" & Chr(34) & "1" & Chr(34) & " />" & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_3" & Chr(34) & " value=" & Chr(34) & MediaLimitBox.Text & Chr(34) & " />" & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_4" & Chr(34) & " value=" & Chr(34) & SortTypeBox.SelectedIndex & Chr(34) & " />"
			ElseIf PlayListType.SelectedIndex = 13 Then
				TopAppend = TopAppend & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_1" & Chr(34) & " value=" & Chr(34) & SubChannelType.SelectedIndex + 1 & Chr(34) & " />" & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_2" & Chr(34) & " value=" & Chr(34) & PlayListLocation.Text & Chr(34) & " />" & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_3" & Chr(34) & " value=" & Chr(34) & MediaLimitBox.Text & Chr(34) & " />" & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_4" & Chr(34) & " value=" & Chr(34) & SortTypeBox.SelectedIndex & Chr(34) & " />"
			ElseIf PlayListType.SelectedIndex = 15 Then
				TopAppend = TopAppend & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_1" & Chr(34) & " value=" & Chr(34) & PlayListLocation.Text & Chr(34) & " />" & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_2" & Chr(34) & " value=" & Chr(34) & PluginNotInclude & Chr(34) & " />" & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_3" & Chr(34) & " value=" & Chr(34) & MediaLimitBox.Text & Chr(34) & " />" & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_4" & Chr(34) & " value=" & Chr(34) & SortTypeBox.SelectedIndex & Chr(34) & " />"
			Else
				TopAppend = TopAppend & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_1" & Chr(34) & " value=" & Chr(34) & Option2.Text & Chr(34) & " />"
			End If

			'Also append the Rulecount to the top, just underneath the channel type & 2nd value
			TopAppend = TopAppend & vbCrLf & vbTab & "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_rulecount" & Chr(34) & " value=" & Chr(34) & rulecount & Chr(34) & " />"

			AppendInfo = TopAppend & AppendInfo

			'Combine the original text, plus the edited channel at the bottom, followed by ending the settings.
			TextFile = TextFile & AppendInfo & vbCrLf & "</settings>"

			SaveFile(PseudoTvSettingsLocation, TextFile)

			'RefreshALL()
			Dim returnindex = TVGuideList.SelectedIndices(0)
			RefreshTVGuide()
			TVGuideList.Items(returnindex).Selected = True
		End If
	End Sub

	Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
		If TVShowList.SelectedItems.Count > 0 Then
			'RefreshAllStudios()
			Form3.Visible = True
			Form3.Focus()
		End If
	End Sub

	Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
		If InterleavedList.SelectedItems.Count > 0 Then
			InterleavedList.Items(InterleavedList.SelectedIndices(0)).Remove()
		End If
	End Sub

	Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
		If SchedulingList.SelectedItems.Count > 0 Then
			SchedulingList.Items(SchedulingList.SelectedIndices(0)).Remove()
		End If
	End Sub

	Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
		Form4.Visible = True
	End Sub

	Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
		Dim Response = InputBox("Enter TV Show's Name", "TV Show Name")

		If Response <> "" Then
			NotShows.Items.Add(Response)
		End If

	End Sub

	Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
		If NotShows.SelectedItems.Count > 0 Then
			NotShows.Items.RemoveAt(NotShows.SelectedIndex)
		End If
	End Sub

	Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
		Dim NewItem(5) As String

		NewItem(0) = InputBox("Enter Channel Number", "Enter Channel Number")
		NewItem(1) = 1
		NewItem(2) = Nothing
		NewItem(3) = Nothing
		NewItem(4) = Nothing
		NewItem(5) = Nothing

		Dim itm As ListViewItem
		itm = New ListViewItem(NewItem)

		Dim InList As Boolean = False

		If IsNumeric(NewItem(0)) Then
			If NewItem(0) > 0 And NewItem(0) <= 999 Then
				For x = 0 To TVGuideList.Items.Count - 1
					If TVGuideList.Items(x).SubItems(0).Text = NewItem(0) Then
						MsgBox("You already have a channel " & NewItem(0))
						InList = True
						Exit For
					End If
				Next
			Else
				MsgBox("Sorry, the channel has to be 1 - 999)")
				InList = True
			End If
		End If

		If InList = False And IsNumeric(NewItem(0)) Then
			TVGuideList.Items.Add(itm)

			'Now make that the selected item.
			For x = 0 To TVGuideList.Items.Count - 1
				If TVGuideList.Items(x).SubItems(0).Text = NewItem(0) Then

					TVGuideList.Items(x).Selected = True
				ElseIf TVGuideList.Items(x).Selected = True Then
					TVGuideList.Items(x).Selected = False
				End If
			Next
		End If
	End Sub

	Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click

		If TVGuideList.Items.Count <> 1 Then

			'Loop through config file.
			'Grab all comments MINUS the ones for selected #
			'Append this & our new content to the file.

			Dim FILE_NAME As String = PseudoTvSettingsLocation
			Dim TextFile As String = ""

			Dim ChannelNum = TVGuideList.Items(TVGuideList.SelectedIndices(0)).SubItems(0).Text

			'Loop through config file.
			'Grab all comments MINUS the ones for selected #
			If System.IO.File.Exists(FILE_NAME) = True Then

				Dim objReader As New System.IO.StreamReader(FILE_NAME)

				Do While objReader.Peek() <> -1

					Dim SingleLine = objReader.ReadLine()

					If InStr(SingleLine, "<setting id=" & Chr(34) & "Channel_" & ChannelNum & "_") Or InStr(SingleLine, "</settings", CompareMethod.Text) Then
					Else
						TextFile = TextFile & SingleLine & vbNewLine
					End If

				Loop

				objReader.Close()
			Else

				MsgBox("File Does Not Exist")

			End If

			SaveFile(PseudoTvSettingsLocation, TextFile)

			RefreshTVGuide()

			TVGuideList.SelectedItems.Clear()

			'Clear everything on the form.

			'Reset the checked options.
			ChkLogo.Checked = False
			chkDontPlayChannel.Checked = False
			ChkRandom.Checked = False
			ChkRealTime.Checked = False
			ChkResume.Checked = False
			ChkIceLibrary.Checked = False
			ChkExcludeBCT.Checked = False
			ChkPopup.Checked = False
			ChkUnwatched.Checked = False
			ChkWatched.Checked = False
			ChkPause.Checked = False
			ChkPlayInOrder.Checked = False
			ResetDays.Clear()
			ChannelName.Clear()

			'Clear other form items.
			TVGuideSubMenu.Items.Clear()
			Option2.Items.Clear()
			InterleavedList.Items.Clear()
			SchedulingList.Items.Clear()
			NotShows.Items.Clear()

		Else
			MsgBox("You must have at least one channel")
		End If
	End Sub

	Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
		Form5.Visible = True
	End Sub

	Private Sub AaaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AaaToolStripMenuItem.Click
		Dim mySettings As New Form6
		mySettings.Version = Me.Version
		mySettings.Show()
	End Sub

	Private Sub DontShowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		If TVGuideSubMenu.SelectedItems.Count > 0 Then
			NotShows.Items.Add(TVGuideSubMenu.Items(TVGuideSubMenu.SelectedIndices(0)).SubItems(0).Text)
			TVGuideSubMenu.Items(TVGuideSubMenu.SelectedIndices(0)).BackColor = Color.Red
		End If
	End Sub

	Private Sub ListMoviePosters_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListMoviePosters.SelectedIndexChanged
		Dim x As Integer = ListMoviePosters.SelectedIndex

		MoviePicture.ImageLocation = ListMoviePosters.Items(x)
		MoviePicture.Refresh()

	End Sub

	Private Sub MoviePosterSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoviePosterSelect.Click
		Dim x As Integer = ListMoviePosters.SelectedIndex
		Dim Type As String = "poster"
		Dim MediaType As String = "movie"


		If MovieLocation.TextLength >= 6 Then
			If MovieLocation.Text.Substring(0, 6) = "smb://" Then
				MovieLocation.Text = MovieLocation.Text.Replace("/", "\")
				MovieLocation.Text = "\\" & MovieLocation.Text.Substring(6)
			End If
		End If

		Dim directoryName As String = ""
		directoryName = Path.GetDirectoryName(MovieLocation.Text)

		' Displays a SaveFileDialog so the user can save the Image
		' assigned to TVPosterSelect.
		Dim saveFileDialog1 As New SaveFileDialog()
		saveFileDialog1.InitialDirectory = directoryName
		saveFileDialog1.Filter = "JPeg Image|*.jpg"
		saveFileDialog1.Title = "Save an Image File"
		saveFileDialog1.FileName = "poster.jpg"
		saveFileDialog1.ShowDialog()

		Dim FileToSaveAs As String = System.IO.Path.Combine(saveFileDialog1.InitialDirectory, saveFileDialog1.FileName)
		MoviePicture.Image.Save(FileToSaveAs, System.Drawing.Imaging.ImageFormat.Jpeg)

		DbExecute("UPDATE art SET url = '" & ListMoviePosters.Items(x).ToString & "' WHERE media_id = '" & MovieIDLabel.Text & "' and media_type = '" & MediaType & "' and type = '" & Type & "'")
		Status.Text = "Updated " & MovieLabel.Text & " " & MovieIDLabel.Text & " Successfully with " & ListMoviePosters.Items(x).ToString & ""

	End Sub


	Private Sub MovieList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MovieList.SelectedIndexChanged
		If MovieList.SelectedItems.Count > 0 Then
			MoviePicture.Update()

			Dim ListItem As ListViewItem
			ListItem = MovieList.SelectedItems.Item(0)

			Dim MovieName
			Dim MovieID

			MovieID = ListItem.SubItems(1).Text
			MovieName = ListItem.SubItems(0).Text
			MovieIDLabel.Text = MovieID


			Dim SelectArray(3)
			SelectArray(0) = 16
			SelectArray(1) = 24
			SelectArray(2) = 20
			SelectArray(3) = 10


			'Shoot it over to the ReadRecord sub, 
			Dim ReturnArray() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM movie WHERE idMovie='" & MovieID & "'", SelectArray)

			Dim ReturnArraySplit() As String

			'We only have 1 response, since it searches by ID. So, just break it into parts. 
			ReturnArraySplit = Split(ReturnArray(0), "~")

			Dim MoviePoster As String = ReturnArraySplit(3)
			ListMoviePosters.Items.Clear()

			If InStr(MoviePoster, "<thumb aspect=""poster"" preview=""") > 0 Or InStr(MoviePoster, "<thumb>") > 0 Then
				Dim MoviePosterSplit() As String = Split(MoviePoster, "<thumb aspect=""poster"" preview=""")

				For X = 1 To UBound(MoviePosterSplit)
					Dim i As Integer = MoviePosterSplit(X).IndexOf("<thumb aspect=""poster"" preview=""")
					MoviePosterSplit(X) = MoviePosterSplit(X).Substring(i + 1, MoviePosterSplit(X).IndexOf(""">"))
					ListMoviePosters.Items.Add(MoviePosterSplit(X))
				Next

				Dim MoviePosterSplit2() As String = Split(MoviePoster, "<thumb>")

				For X = 1 To UBound(MoviePosterSplit2)
					Dim i As Integer = MoviePosterSplit2(X).IndexOf("<thumb>")
					MoviePosterSplit2(X) = MoviePosterSplit2(X).Substring(i + 1, MoviePosterSplit2(X).IndexOf("</thumb>"))
					ListMoviePosters.Items.Add(MoviePosterSplit2(X))
				Next
			Else
				ListMoviePosters.Items.Add("Nothing Found")
			End If

			Dim MovieGenres As String = ReturnArraySplit(0)

			MovieLabel.Text = MovieName
			MovieLocation.Text = ReturnArraySplit(1)

			If ReturnArraySplit(2) = "" Then
				txtMovieNetwork.SelectedIndex = -1
			Else
				txtMovieNetwork.Text = ReturnArraySplit(2)
			End If

			'Loop through each Movie Genre, if there more than one.
			MovieGenresList.Items.Clear()
			If InStr(MovieGenres, " / ") > 0 Then
				Dim MovieGenresSplit() As String = Split(MovieGenres, " / ")

				For x = 0 To UBound(MovieGenresSplit)
					MovieGenresList.Items.Add(MovieGenresSplit(x))
				Next
			ElseIf MovieGenres <> "" Then
				MovieGenresList.Items.Add(MovieGenres)
			End If


			If MovieLocation.TextLength >= 6 Then
				If MovieLocation.Text.Substring(0, 6) = "smb://" Then
					MovieLocation.Text = MovieLocation.Text.Replace("/", "\")
					MovieLocation.Text = "\\" & MovieLocation.Text.Substring(6)
				End If
			End If

			Dim directoryName As String = ""
			directoryName = Path.GetDirectoryName(MovieLocation.Text)

			If System.IO.File.Exists(directoryName & "\" & "poster.jpg") Then
				MoviePicture.ImageLocation = (directoryName & "\" & "poster.jpg")
			Else
				MoviePicture.ImageLocation = "https://github.com/Lunatixz/script.pseudotv.live/raw/development/resources/images/poster.png"
			End If

		End If
	End Sub



	Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
		Form7.Visible = True
		Form7.Focus()
	End Sub

	Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click
		If MovieGenresList.SelectedIndex >= 0 Then

			'Grab the 3rd column from the TVShowList, which is the TVShowID
			Dim GenreID = LookUpGenre(MovieGenresList.Items(MovieGenresList.SelectedIndex).ToString)

			'Now, remove the link in the database.
			'DbExecute("DELETE FROM genrelinktvshow WHERE idGenre = '" & GenreID & "' AND idShow ='" & TVShowList.Items(TVShowList.SelectedIndices(0)).SubItems(2).Text & "'")


			MovieGenresList.Items.RemoveAt(MovieGenresList.SelectedIndex)
			' SaveTVShow_Click(Nothing, Nothing)
			RefreshGenres()
		End If
	End Sub

	Public Sub RefreshNetworkListMovies()
		MovieNetworkList.Items.Clear()

		Dim SelectArray(1)
		SelectArray(0) = 2
		SelectArray(1) = 20

		Dim ReturnArray() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM movie ORDER BY c18 ASC", SelectArray)

		'Loop through each returned Movie
		For x = 0 To ReturnArray.Count - 1
			If (String.IsNullOrEmpty(ReturnArray(x))) Then
				Continue For
			End If

			Dim ReturnArraySplit() As String

			Dim ShowName As String
			Dim ShowNetwork As String

			ReturnArraySplit = Split(ReturnArray(x), "~")

			ShowName = ReturnArraySplit(0)
			'Updated ReturnArraySplit for ShowNetwork to reflect MyVideos78.db schema
			ShowNetwork = ReturnArraySplit(1)

			Dim NetworkListed As Boolean = False

			For y = 0 To MovieNetworkList.Items.Count - 1

				If MovieNetworkList.Items(y).SubItems(0).Text = ShowNetwork Then
					NetworkListed = True
					MovieNetworkList.Items(y).SubItems(1).Text = MovieNetworkList.Items(y).SubItems(1).Text + 1
				End If

			Next

			If NetworkListed = False Then
				Dim itm As ListViewItem
				Dim str(2) As String

				str(0) = ShowNetwork
				str(1) = 1
				'test
				itm = New ListViewItem(str)

				'Add the item to the TV show list.
				MovieNetworkList.Items.Add(itm)

			End If

		Next

	End Sub

    Public Sub RefreshNetworkList()
        NetworkList.Items.Clear()

        Dim SelectArray(1)
        SelectArray(0) = 1
        SelectArray(1) = 15

        Dim ReturnArray() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM tvshow ORDER BY c14 ASC", SelectArray)

        'Loop through each returned TV show.
        For x = 0 To ReturnArray.Count - 1


            Dim ReturnArraySplit() As String

            Dim ShowName As String
            Dim ShowNetwork As String

            ReturnArraySplit = Split(ReturnArray(x), "~")

            ShowName = ReturnArraySplit(0)
            ShowNetwork = ReturnArraySplit(1)

            Dim NetworkListed As Boolean = False

            For y = 0 To NetworkList.Items.Count - 1

                If NetworkList.Items(y).SubItems(0).Text = ShowNetwork Then
                    NetworkListed = True
                    NetworkList.Items(y).SubItems(1).Text = NetworkList.Items(y).SubItems(1).Text + 1
                End If

            Next

            If NetworkListed = False Then
                Dim itm As ListViewItem
                Dim str(2) As String

                str(0) = ShowNetwork
                str(1) = 1

                itm = New ListViewItem(str)

                'Add the item to the TV show list.
                NetworkList.Items.Add(itm)

            End If

        Next

    End Sub

    Private Sub Button17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button17.Click
        If MovieList.SelectedItems.Count > 0 Then

            ' Fix any issues with shows and 's.
            Dim MovieName As String = MovieLabel.Text
            'Convert show genres into the format ex:  genre1 / genre2 / etc.
            Dim MovieGenres = ConvertGenres(MovieGenresList)
            MovieName = Replace(MovieName, "'", "''")
            'Grab the Network ID based on the name
            Dim NetworkID = LookUpNetwork(txtMovieNetwork.Text)
			Dim MovieID As String = MovieList.SelectedItems(0).SubItems(1).Text


			If (Version >= KodiVersion.Isengard) Then
				DbExecute("DELETE FROM studio_link WHERE media_id = '" & MovieID & "'")
				DbExecute("INSERT INTO studio_link (studio_id, media_id, media_type) VALUES ('" & NetworkID & "', '" & MovieID & "', 'movie')")
			Else
				DbExecute("DELETE FROM studiolinkmovie WHERE idMovie = '" & MovieID & "'")
				DbExecute("INSERT INTO studiolinkmovie (idStudio, idMovie) VALUES ('" & NetworkID & "', '" & MovieID & "')")
			End If

            DbExecute("UPDATE movie SET c14 = '" & MovieGenres & "', c18 ='" & txtMovieNetwork.Text & "' WHERE idMovie = '" & MovieID & "'")
            Status.Text = "Updated " & MovieLabel.Text & " Successfully"

			If (Version >= KodiVersion.Isengard) Then
				'Remove all genres from tv show
				DbExecute("DELETE FROM genre_link  WHERE media_id = '" & MovieID & "'")

				'add each one.  one by one.
				For x = 0 To MovieGenresList.Items.Count - 1
					Dim GenreID = LookUpGenre(MovieGenresList.Items(x).ToString)
					DbExecute("INSERT INTO genre_link  (genre_id, media_id, media_type) VALUES ('" & GenreID & "', '" & MovieID & "', 'movie')")
				Next
			Else
				'Remove all genres from tv show
				DbExecute("DELETE FROM genrelinkmovie  WHERE idMovie = '" & MovieID & "'")

				'add each one.  one by one.
				For x = 0 To MovieGenresList.Items.Count - 1
					Dim GenreID = LookUpGenre(MovieGenresList.Items(x).ToString)
					DbExecute("INSERT INTO genrelinkmovie (idGenre, idMovie) VALUES ('" & GenreID & "', '" & MovieID & "')")
				Next
			End If
            

            'Save our spot on the list.
            Dim SavedName = txtMovieNetwork.Text

            'Refresh Things
            RefreshNetworkListMovies()
            RefreshGenres()

            Dim returnindex = MovieList.SelectedIndices(0)
            RefreshMovieList()
            MovieList.Items(returnindex).Selected = True

        End If
    End Sub

    Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button18.Click
        If MovieList.SelectedItems.Count > 0 Then
            RefreshAllStudios()
            Form8.Visible = True
            Form8.Focus()
        End If
    End Sub

    Private Sub MovieNetworkList_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles MovieNetworkList.ColumnClick
        ' Get the new sorting column. 
        Dim new_sorting_column As ColumnHeader = MovieNetworkList.Columns(e.Column)
        ' Figure out the new sorting order. 
        Dim sort_order As System.Windows.Forms.SortOrder
        If m_SortingColumn Is Nothing Then
            ' New column. Sort ascending. 
            sort_order = SortOrder.Ascending
        Else ' See if this is the same column. 
            If new_sorting_column.Equals(m_SortingColumn) Then
                ' Same column. Switch the sort order. 
                If m_SortingColumn.Text.StartsWith("> ") Then
                    sort_order = SortOrder.Descending
                Else
                    sort_order = SortOrder.Ascending
                End If
            Else
                ' New column. Sort ascending. 
                sort_order = SortOrder.Ascending
            End If
            ' Remove the old sort indicator. 
            m_SortingColumn.Text = m_SortingColumn.Text.Substring(2)
        End If
        ' Display the new sort order. 
        m_SortingColumn = new_sorting_column
        If sort_order = SortOrder.Ascending Then
            m_SortingColumn.Text = "> " & m_SortingColumn.Text
        Else
            m_SortingColumn.Text = "< " & m_SortingColumn.Text
        End If
        ' Create a comparer. 
        MovieNetworkList.ListViewItemSorter = New clsListviewSorter(e.Column, sort_order)
        ' Sort. 
        MovieNetworkList.Sort()
    End Sub

    Private Sub MovieNetworkList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MovieNetworkList.SelectedIndexChanged
        MovieNetworkListSubList.Items.Clear()

        If MovieNetworkList.SelectedIndices.Count > 0 Then

            Dim SelectArray(0)
            SelectArray(0) = 2

            Dim ReturnArray() As String = DbReadRecord(VideoDatabaseLocation, "SELECT * FROM movie WHERE c18='" & MovieNetworkList.Items(MovieNetworkList.SelectedIndices(0)).SubItems(0).Text & "'", SelectArray)

            For x = 0 To ReturnArray.Count - 1
                MovieNetworkListSubList.Items.Add(ReturnArray(x))
            Next

        End If
    End Sub

    Private Sub Button19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button19.Click
        LookUpGenre("aaccc")
    End Sub

    Private Sub YouTubeType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles YouTubeType.SelectedIndexChanged
        If YouTubeType.SelectedIndex = 0 Then
            Label6.Text = "Channel/User:"
            With PlayListLocation
                .Location = New System.Drawing.Point(270, 120)
            End With
            PlayListLocation.Visible = True
            NotShows.Visible = False
            AddExcludeBtn.Visible = False
            DelExcludeBtn.Visible = False
            Label12.Visible = False
        ElseIf YouTubeType.SelectedIndex = 1 Then
            Label6.Text = "Playlist:"
            With PlayListLocation
                .Location = New System.Drawing.Point(220, 120)
            End With
            PlayListLocation.Visible = True
            NotShows.Visible = False
            AddExcludeBtn.Visible = False
            DelExcludeBtn.Visible = False
            Label12.Visible = False
        ElseIf YouTubeType.SelectedIndex = 2 Or YouTubeType.SelectedIndex = 3 Then
            Label6.Text = "Username:"
            With PlayListLocation
                .Location = New System.Drawing.Point(245, 120)
            End With
            PlayListLocation.Visible = True
            NotShows.Visible = False
            AddExcludeBtn.Visible = False
            DelExcludeBtn.Visible = False
            Label12.Visible = False
        ElseIf YouTubeType.SelectedIndex = 4 Then
            Label6.Text = "Search String:"
            With PlayListLocation
                .Location = New System.Drawing.Point(270, 120)
            End With
            PlayListLocation.Visible = True
            NotShows.Visible = False
            AddExcludeBtn.Visible = False
            DelExcludeBtn.Visible = False
            Label12.Visible = False
        ElseIf YouTubeType.SelectedIndex = 6 Then
            Label6.Text = ""
            With PlayListLocation
                .Location = New System.Drawing.Point(295, 120)
            End With
            PlayListLocation.Visible = False
            NotShows.Visible = True
            AddExcludeBtn.Visible = True
            DelExcludeBtn.Visible = True
            Label12.Text = "Add/Remove Playlists"
            Label12.Visible = True
        ElseIf YouTubeType.SelectedIndex = 7 Then
            Label6.Text = ""
            With PlayListLocation
                .Location = New System.Drawing.Point(295, 120)
            End With
            PlayListLocation.Visible = False
            NotShows.Visible = True
            AddExcludeBtn.Visible = True
            DelExcludeBtn.Visible = True
            Label12.Text = "Add/Remove Channels"
            Label12.Visible = True
        ElseIf YouTubeType.SelectedIndex = 8 Then
            Label6.Text = "GData Url:"
            With PlayListLocation
                .Location = New System.Drawing.Point(245, 120)
            End With
            PlayListLocation.Visible = True
            NotShows.Visible = False
            AddExcludeBtn.Visible = False
            DelExcludeBtn.Visible = False
            Label12.Visible = False
            GDataDemoLink.Links.Remove(GDataDemoLink.Links(0))
            GDataDemoLink.Links.Add(0, GDataDemoLink.Text = "GDataDemo", "http://gdata.youtube.com/demo/index.html")
            GDataDemoLink.Visible = True
        Else
            Label6.Text = "Nothing Here"
            PlayListLocation.Visible = False
            NotShows.Visible = False
            AddExcludeBtn.Visible = False
            DelExcludeBtn.Visible = False
            Label12.Visible = False
        End If

    End Sub

    Private Sub SubChannelType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles SubChannelType.SelectedIndexChanged
        If PlayListType.SelectedIndex = 13 And SubChannelType.SelectedIndex = 0 Then
            Label6.Text = "LastFM User:"
            With PlayListLocation
                .Location = New System.Drawing.Point(270, 120)
            End With
        ElseIf PlayListType.SelectedIndex = 13 And SubChannelType.SelectedIndex = 1 Then
            Label6.Text = "Channel_##:"
            With PlayListLocation
                .Location = New System.Drawing.Point(295, 120)
            End With
        End If
    End Sub

    Private Sub AddBanner_Click(sender As Object, e As EventArgs) Handles AddBanner.Click
        Form9.Visible = True
        Form9.Focus()
        Form9.AddBannerPictureBox.ImageLocation = "http://github.com/Lunatixz/script.pseudotv.live/raw/development/resources/images/banner.png"
    End Sub

    Private Sub AddPoster_Click(sender As Object, e As EventArgs) Handles AddPoster.Click
        Form10.Visible = True
        Form10.Focus()
        Form10.AddPosterPictureBox.ImageLocation = "http://github.com/Lunatixz/script.pseudotv.live/raw/development/resources/images/poster.png"
    End Sub


    Private Sub AddMoviePosterButton_Click(sender As Object, e As EventArgs) Handles AddMoviePosterButton.Click
        Form11.Visible = True
        Form11.Focus()
        Form11.AddMoviePosterPictureBox.ImageLocation = "http://github.com/Lunatixz/script.pseudotv.live/raw/development/resources/images/poster.png"
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Close()
    End Sub

    Private Sub GDataDemoLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles GDataDemoLink.LinkClicked
        Dim sInfo As New ProcessStartInfo(e.Link.LinkData.ToString())
        Process.Start(sInfo)
    End Sub

    Private Sub HelpList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles HelpList.SelectedIndexChanged

        If HelpList.SelectedIndex = 0 Then
            HelpLink.Links.Remove(HelpLink.Links(0))
            HelpLink.Links.Add(0, HelpLink.Text = "Go!", "http://www.youtube.com/embed/k7htE58DrRA?autoplay=1&fs=0&rel=0&showinfo=0")

        ElseIf HelpList.SelectedIndex = 1 Then
            HelpLink.Links.Remove(HelpLink.Links(0))
            HelpLink.Links.Add(0, HelpLink.Text = "Go!", "https://www.youtube.com/embed/qfAgznkiwWM?autoplay=1&fs=0&rel=0&showinfo=0")

        ElseIf HelpList.SelectedIndex = 2 Then
            HelpLink.Links.Remove(HelpLink.Links(0))
            HelpLink.Links.Add(0, HelpLink.Text = "Go!", "https://www.youtube.com/embed/1ISxddyAGFQ?autoplay=1&fs=0&rel=0&showinfo=0")

        ElseIf HelpList.SelectedIndex = 3 Then
            HelpLink.Links.Remove(HelpLink.Links(0))
            HelpLink.Links.Add(0, HelpLink.Text = "Go!", "https://www.youtube.com/embed/DabCIm-tSVQ?autoplay=1&fs=0&rel=0&showinfo=0")
        End If

    End Sub

    Private Sub HelpLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles HelpLink.LinkClicked
        Dim sInfo As New ProcessStartInfo(e.Link.LinkData.ToString())
        Process.Start(sInfo)
    End Sub

    Private Sub PeppyDonatePic_Click(sender As Object, e As EventArgs) Handles PeppyDonatePic.Click
        Dim sInfo As New ProcessStartInfo("https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=peppy6582%40gmail%2ecom&lc=US&no_note=0¤cy_code=USD&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHostedGuest")
        Process.Start(sInfo)
    End Sub

    Private Sub LunaDonatePic_Click(sender As Object, e As EventArgs) Handles LunaDonatePic.Click
        Dim sInfo As New ProcessStartInfo("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=4BP96JF9YK2MQ")
        Process.Start(sInfo)
    End Sub

    Private Sub AddExcludeBtn_Click(sender As Object, e As EventArgs) Handles AddExcludeBtn.Click

        If PlayListType.SelectedIndex = 10 Then

            Dim Response = InputBox("Enter Playlist or User String")

            If Response <> "" Then
                NotShows.Items.Add(Response)
            End If

            If YouTubeMulti = "" Then
                YouTubeMulti = Response
            Else
                YouTubeMulti = YouTubeMulti & "|" & Response
            End If
        Else
            Dim Response = InputBox("Enter Exclude String")

            If Response <> "" Then
                NotShows.Items.Add(Response)
            End If

            If PluginNotInclude = "" Then
                PluginNotInclude = Response
            Else
                PluginNotInclude = PluginNotInclude & "," & Response
            End If
        End If

    End Sub

    Private Sub DelExclutn_Click(sender As Object, e As EventArgs) Handles DelExcludeBtn.Click
        If PlayListType.SelectedIndex = 10 Then
            If NotShows.SelectedItems.Count > 0 Then
                NotShows.Items.RemoveAt(NotShows.SelectedIndex)
            End If

            Dim items As String() = NotShows.Items.OfType(Of Object)().[Select](Function(item) item.ToString()).ToArray()
            Dim result As String = String.Join("|", items)

            YouTubeMulti = result
        Else
            If NotShows.SelectedItems.Count > 0 Then
                NotShows.Items.RemoveAt(NotShows.SelectedIndex)
            End If

            Dim items As String() = NotShows.Items.OfType(Of Object)().[Select](Function(item) item.ToString()).ToArray()
            Dim result As String = String.Join(",", items)

            PluginNotInclude = result
        End If
    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click

    End Sub

End Class
