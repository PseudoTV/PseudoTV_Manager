Imports System
Imports System.IO
Imports System.Data
Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions
Imports NLog

Public Enum KodiVersion
	Gotham = 13
	Helix = 14
	Isengard = 15
	Jarvis = 16
End Enum

Module Module1

	Dim _logger = LogManager.GetCurrentClassLogger()

	Public Function GetKodiVersion(ByVal VideoDBFileName As String)
		Dim regex As Regex = New Regex("(MyVideos)(\d+).db")
		Dim match As Match = regex.Match(VideoDBFileName)
		If match.Success Then
			Dim DbNumber As Integer = Convert.ToInt32(match.Groups(2).Value)
			If (DbNumber < 90) Then
				Return KodiVersion.Gotham
			ElseIf (DbNumber < 91) Then
				Return KodiVersion.Helix
			ElseIf (DbNumber < 94) Then
				Return KodiVersion.Isengard
			ElseIf (DbNumber > 93) Then
				Return KodiVersion.Jarvis
			End If
		End If
		Return KodiVersion.Gotham
	End Function

	Public Function ReadFile(ByVal FilePath As String)
		'Reads the file and returns it back as a string variable.
		Dim fileText As String = File.ReadAllText(FilePath)
		Return fileText
	End Function

	Public Sub SaveFile(ByVal FilePath As String, ByVal Data As String)

		If System.IO.File.Exists(FilePath) = True Then
			Dim objWriter2 As New System.IO.StreamWriter(FilePath)
			objWriter2.Write(Data)
			objWriter2.Dispose()
			objWriter2.Close()
		End If

	End Sub

	Public Function ReadPluginRecord(ByVal DBLocation, ByVal SQLStatement, ByVal ColumnArray())
		'Connect to the data-base

		Dim PluginDatabaseData As String = "Data Source=" & Form1.AddonDatabaseLocation

		Dim ArrayResponse() As String = {""}
		'This is a standard, SQLite database.
		Dim SQLconnect As New SQLite.SQLiteConnection()
		Dim SQLcommand As SQLite.SQLiteCommand = Nothing
		SQLconnect.ConnectionString = "Data Source=" & Form1.AddonDatabaseLocation

		Try
			SQLconnect.Open()
			SQLcommand = SQLconnect.CreateCommand
			SQLcommand.CommandText = SQLStatement
			Dim SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()

			Dim x As Integer = 0

			While SQLreader.Read()
				ReDim Preserve ArrayResponse(x)
				Dim StringResponse As String = ""


				For y = 0 To (UBound(ColumnArray))
					If y > 0 Then
						StringResponse = StringResponse & "~" & SQLreader(ColumnArray(y))
					Else
						StringResponse = SQLreader(ColumnArray(y))
					End If
				Next

				ArrayResponse(x) = StringResponse

				x = x + 1

			End While

		Catch myerror As SQLite.SQLiteException
			_logger.Error(myerror.Message)
			MessageBox.Show("Error Connecting to Database: " & myerror.Message)

		Finally
			SQLcommand.Dispose()
			SQLconnect.Close()

		End Try

		Return ArrayResponse
	End Function

	Public Sub PluginExecute(ByVal SQLQuery As String)

		Dim PluginDatabaseData As String = "Data Source=" & Form1.AddonDatabaseLocation

		'These are standard SQLite databases.
		'Open the connection.
		Dim SQLconnect As New SQLite.SQLiteConnection()
		Dim SQLcommand As SQLite.SQLiteCommand = Nothing

		SQLconnect.ConnectionString = "Data Source=" & Form1.AddonDatabaseLocation


		Try
			SQLconnect.Open()

			'Set the command.
			SQLcommand = SQLconnect.CreateCommand

			'Execute the command.
			SQLcommand.CommandText = SQLQuery
			SQLcommand.ExecuteNonQuery()

		Catch myerror As SQLite.SQLiteException
			_logger.Error(myerror.Message)
			MessageBox.Show("Error Connecting to Database: " & myerror.Message)
		Finally
			'Dispose of and close the connection.
			SQLcommand.Dispose()
			SQLconnect.Close()
		End Try

	End Sub

	Public Function DbReadRecord(ByVal DBLocation, ByVal SQLStatement, ByVal ColumnArray())
		'Connect to the data-base

		'Dim VideoDatabaseData As String = "Data Source=" & Form1.VideoDatabaseLocation
		'Dim PluginDatabaseData As String = "Data Source=" & Form1.AddonDatabaseLocation

		Dim ArrayResponse() As String = {""}


		If Form1.DatabaseType = 0 Then
			'This is a standard, SQLite database.
			Dim SQLconnect As New SQLite.SQLiteConnection()
			Dim SQLcommand As SQLite.SQLiteCommand = Nothing
			SQLconnect.ConnectionString = "Data Source=" & Form1.VideoDatabaseLocation

			Try
				SQLconnect.Open()
				SQLcommand = SQLconnect.CreateCommand
				SQLcommand.CommandText = SQLStatement
				Dim SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()

				Dim x As Integer = 0

				While SQLreader.Read()
					ReDim Preserve ArrayResponse(x)
					Dim StringResponse As String = ""


					For y = 0 To (UBound(ColumnArray))
						If y > 0 Then
							StringResponse = StringResponse & "~" & SQLreader(ColumnArray(y))
						Else
							StringResponse = SQLreader(ColumnArray(y))
						End If
					Next

					ArrayResponse(x) = StringResponse

					x = x + 1

				End While

			Catch myerror As MySqlException
				_logger.Error(myerror.Message)
				MessageBox.Show("Error Connecting to Database: " & myerror.Message)

			Finally
				SQLcommand.Dispose()
				SQLconnect.Close()

			End Try

		Else
			'This is for MySQL Databases, just slight syntax differences.
			Dim SQLconnect As New MySqlConnection
			Dim SQLcommand As MySqlCommand = Nothing
			SQLconnect.ConnectionString = Form1.MySQLConnectionString
			Try
				SQLconnect.Open()
				SQLcommand = SQLconnect.CreateCommand
				SQLcommand.CommandText = SQLStatement
				Dim SQLreader As MySqlDataReader = SQLcommand.ExecuteReader()

				Dim x As Integer = 0

				While SQLreader.Read()
					ReDim Preserve ArrayResponse(x)
					Dim StringResponse As String = ""


					For y = 0 To (UBound(ColumnArray))
						If y > 0 Then
							StringResponse = StringResponse & "~" & SQLreader(ColumnArray(y))
						Else
							StringResponse = SQLreader(ColumnArray(y))
						End If
					Next

					ArrayResponse(x) = StringResponse

					x = x + 1

				End While

			Catch myerror As MySqlException
				_logger.Error(myerror.Message)
				MessageBox.Show("Error Connecting to Database: " & myerror.Message)
			Finally
				SQLcommand.Dispose()
				SQLconnect.Close()

			End Try

		End If


		Return ArrayResponse

	End Function

	Public Sub DbExecute(ByVal SQLQuery As String)

		If Form1.DatabaseType = 0 Then
			'These are standard SQLite databases.
			'Open the connection.
			Dim SQLconnect As New SQLite.SQLiteConnection()
			Dim SQLcommand As SQLite.SQLiteCommand = Nothing
			SQLconnect.ConnectionString = "Data Source=" & Form1.VideoDatabaseLocation

			Try
				SQLconnect.Open()

				'Set the command.
				SQLcommand = SQLconnect.CreateCommand

				'Execute the command.
				SQLcommand.CommandText = SQLQuery
				SQLcommand.ExecuteNonQuery()

			Catch myerror As MySqlException
				_logger.Error(myerror.Message)
				MessageBox.Show("Error Connecting to Database: " & myerror.Message)
			Finally
				'Dispose of and close the connection.
				SQLcommand.Dispose()
				SQLconnect.Close()
			End Try

		Else
			'These are for MYSQL connections
			'Just anything that says SQLite is changed to MySQL .. no biggy.

			'Open the connection.
			Dim SQLconnect As New MySqlConnection
			Dim SQLcommand As MySqlCommand = Nothing
			SQLconnect.ConnectionString = Form1.MySQLConnectionString
			Try
				SQLconnect.Open()

				'Set the command.
				SQLcommand = SQLconnect.CreateCommand

				'Execute the command.
				SQLcommand.CommandText = SQLQuery
				SQLcommand.ExecuteNonQuery()
			Catch myerror As MySqlException
				_logger.Error(myerror.Message)
				MessageBox.Show("Error Connecting to Database: " & myerror.Message)
				'Finally
				'Dispose of and close the connection.
			Finally
				SQLcommand.Dispose()
				SQLconnect.Close()

			End Try

		End If
	End Sub

	Public Function TestMYSQL(ByVal connectionstring As String)

		Dim ConnectSuccessful As Boolean = False

		Dim conn = New MySqlConnection()
		'Dim SQLCommand As MySqlCommand
		conn.ConnectionString = connectionstring
		Try
			conn.Open()
			ConnectSuccessful = True
			MsgBox("Successfully connected to database.")
			conn.Close()
		Catch myerror As MySqlException
			_logger.Error(myerror.Message)
			MessageBox.Show("Error Connecting to Database: " & myerror.Message)
		Finally
			conn.Dispose()
		End Try

		Return ConnectSuccessful
	End Function

	Public Function TestMYSQLite(ByVal connectionstring As String)

		Dim ConnectSuccessful As Boolean = False

		Dim conn As New SQLite.SQLiteConnection()
		'Dim SQLCommand As MySqlCommand
		conn.ConnectionString = "Data Source=" & connectionstring
		Try
			conn.Open()
			ConnectSuccessful = True
			MsgBox("Successfully connected to database.")
			conn.Close()
		Catch myerror As MySqlException
			_logger.Error(myerror.Message)
			MessageBox.Show("Error Connecting to Database: " & myerror.Message)
		Finally
			conn.Dispose()
		End Try

		Return ConnectSuccessful
	End Function

	Public Function testMysql2(ByVal DBLocation, ByVal SQLStatement, ByRef ColumnArray())
		'Connect to the data-base
		Dim SQLconnect As New MySqlConnection()
		Dim SQLcommand As MySqlCommand
		SQLconnect.ConnectionString = "server=" & "127.0.0.1" & ";" _
		  & "user id=" & "xbmc" & ";" _
		  & "password=" & "xbmc" & ";" _
		  & "database=xbmcvideo60"
		SQLconnect.Open()
		SQLcommand = SQLconnect.CreateCommand
		SQLcommand.CommandText = SQLStatement
		Dim SQLreader As MySqlDataReader = SQLcommand.ExecuteReader()

		Dim x As Integer = 0

		Dim ArrayResponse() As String = Nothing
		While SQLreader.Read()
			ReDim Preserve ArrayResponse(x)
			Dim StringResponse As String = ""


			For y = 0 To (UBound(ColumnArray))
				If y > 0 Then
					StringResponse = StringResponse & "~" & SQLreader(ColumnArray(y))
				Else
					StringResponse = SQLreader(ColumnArray(y))
				End If
			Next

			ArrayResponse(x) = StringResponse

			x = x + 1

		End While
		SQLcommand.Dispose()
		SQLconnect.Close()


		Return ArrayResponse
	End Function
End Module


