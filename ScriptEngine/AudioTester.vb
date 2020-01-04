Imports FPX
Imports Microsoft.Xna.Framework

Public Class AudioTester
    Inherits Component

    Private keyDown As Boolean

    Public Sub Update(gameTime As GameTime)
        If (Input.Keyboard.GetState().IsKeyDown(Input.Keys.A) And Not keyDown) Then
            GetComponent(Of AudioSource).Play()
        End If

        keyDown = Input.Keyboard.GetState().IsKeyDown(Input.Keys.A)
    End Sub

End Class
