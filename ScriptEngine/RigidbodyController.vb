Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports Microsoft.Xna.Framework.Input
Imports FPX

Public Class RigidbodyController
    Inherits Component

    Private startPosition = Vector3.Zero
    Private startRotation = Quaternion.Identity

    Public Sub Start()
        startPosition = transform.position
        startRotation = transform.rotation
    End Sub

    Public Sub Update(gameTime As GameTime)
        Dim gamepadstate = GamePad.GetState(PlayerIndex.One)
        Dim leftThumbstick = gamepadstate.ThumbSticks.Left
        Dim rightThumbstick = gamepadstate.ThumbSticks.Right
        Dim vertical = gamepadstate.Triggers.Left - gamepadstate.Triggers.Right

        If gamepadstate.IsButtonDown(Buttons.A) Then
            transform.position = startPosition
            transform.rotation = startRotation
            GetComponent(Of Rigidbody).SendMessage("Reset")
        End If

        GetComponent(Of Rigidbody).acceleration = Vector3.Right * leftThumbstick.X + Vector3.Up * vertical + Vector3.Forward * leftThumbstick.Y
        GetComponent(Of Rigidbody).torque = Vector3.Right * rightThumbstick.Y + Vector3.Up * rightThumbstick.X
    End Sub

    Public Sub DrawUI(spriteBatch As SpriteBatch)
        spriteBatch.DrawString(GameCore.fonts("SegoeUI"), GetComponent(Of Rigidbody).velocity.ToString(), Vector2.Zero, Color.Black)
    End Sub
End Class
