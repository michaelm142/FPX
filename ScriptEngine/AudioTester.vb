Imports FPX
Imports Microsoft.Xna.Framework

Public Class AudioTester
    Inherits Component

    Private keyDown As Boolean

    Public Sub Update(gameTime As GameTime)
        If (Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) And Not keyDown) Then
            GetComponent(Of AudioSource).Play()
        End If

        keyDown = Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A)
    End Sub

End Class
