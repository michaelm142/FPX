Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports FPX

Public Class TitleScreen
    Inherits Component

    Private t As Single

    Public Sub Start()
        For Each obj In Scene.Active.Objects
            If obj.Name = "Death Screen" Or obj.Name = Name Then
                Continue For
            End If
            obj.Enabled = False
            obj.Visible = False
        Next
    End Sub

    Public Sub Update(gametime As GameTime)
        t += Time.deltaTime

        If Input.GetAxis("Submit") > 0.0F Then
            For Each obj In Scene.Active.Objects
                If obj.Name = "Death Screen" Then
                    Continue For
                End If
                obj.Enabled = True
                obj.Visible = True
            Next
            gameObject.Enabled = False
            gameObject.Visible = False
            Camera.Active = GameObject.FindByTag("Camera").GetComponent(Of Camera)
        End If
    End Sub

    Public Sub DrawUI(spriteBatch As SpriteBatch)
        spriteBatch.DrawString(GameCore.fonts("Impact"), "FPX v0.1", position.ToVector2(), Color.Blue)
        spriteBatch.DrawString(GameCore.fonts("ImpactSmall"), "Press 'A' to begin", position.ToVector2() + New Vector2(300, 500), Color.Blue * Math.Abs(Math.Cos(t)))
    End Sub

End Class
