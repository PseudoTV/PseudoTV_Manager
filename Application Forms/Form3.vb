Public Class Form3
	Public Version As KodiVersion = KodiVersion.Helix
    Private Sub Form3_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        'Add the item name, then make it the selected option.
        'MsgBox("leaving")
		'Form1.RefreshAllStudios()
    End Sub


    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ListBox1.Items.Add("aaa")
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim NetworkName = InputBox("Please Enter the Network Name", "Network Name")

        If NetworkName <> "" Then
            Dim AlreadyUsed As Boolean = False

            For x = 0 To ListBox1.Items.Count - 1

                If StrComp(ListBox1.Items(x).ToString, NetworkName, CompareMethod.Text) = 0 Then
                    AlreadyUsed = True
                End If
            Next
			If AlreadyUsed = False Then
				Dim studioPar As String = "name"
				If (Version < KodiVersion.Isengard) Then
					studioPar = "strStudio"
				End If

				'DbExecute("INSERT INTO studio (" + studioPar + ") VALUES ('" & NetworkName & "')")
				Form1.RefreshAllStudios()
			Else
				MsgBox("You already have a network labeled : " & NetworkName)
			End If
        End If

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click


    End Sub
End Class