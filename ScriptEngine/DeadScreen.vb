Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports FPX

Public Class DeadScreen
    Inherits Component

    Private target As GameObject

    Private t As Single

    Public Sub Start()
        target = GameObject.FindByTag("Player")
    End Sub


    Public Sub Update(gameTime As GameTime)
        If target.Destroyed Then
            gameObject.Visible = True

            If Input.GetAxis("Submit") > 0.0F Then
                Scene.Active.Reset()
            End If
        End If

        t += Time.deltaTime
    End Sub

    Public Sub DrawUI(spriteBatch As SpriteBatch)
        spriteBatch.DrawString(GameCore.fonts("Impact"), "You Died", position.ToVector2(), Color.Red)
        spriteBatch.DrawString(GameCore.fonts("ImpactSmall"), "Press 'A' to exit", position.ToVector2() + New Vector2(450, 500), Color.Red * Math.Abs(Math.Sin(t)))
    End Sub
End Class
