Imports System.Xml
Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Input
Imports Microsoft.Xna.Framework.Graphics
Imports FPX.ComponentModel

Public Class CameraController
    Inherits Component

    Private moveSpeed As Single
    Private rotationSpeed As Single

    Public Sub Update(ByVal gametime As GameTime)
        Dim forward As Single
        Dim right As Single
        Dim up As Single
        Dim turnH As Single
        Dim turnV As Single

        Dim keyboardState As KeyboardState = Keyboard.GetState()

        If keyboardState.IsKeyDown(Keys.W) Then
            forward += 1.0F
        ElseIf keyboardState.IsKeyDown(Keys.S) Then
            forward -= 1.0F
        End If

        If keyboardState.IsKeyDown(Keys.D) Then
            right += 1.0F
        ElseIf keyboardState.IsKeyDown(Keys.A) Then
            right -= 1.0F
        End If

        If keyboardState.IsKeyDown(Keys.Q) Then
            up += 1.0F
        ElseIf keyboardState.IsKeyDown(Keys.E) Then
            up -= 1.0F
        End If

        If keyboardState.IsKeyDown(Keys.Right) Then
            turnH -= 1.0F
        ElseIf keyboardState.IsKeyDown(Keys.Left) Then
            turnH += 1.0F
        End If

        If keyboardState.IsKeyDown(Keys.Up) Then
            turnV -= 1.0F
        ElseIf keyboardState.IsKeyDown(Keys.Down) Then
            turnV += 1.0F
        End If

        transform.position += (transform.worldPose.Up * up + transform.worldPose.Right * right + transform.worldPose.Forward * forward) * moveSpeed * gametime.ElapsedGameTime.TotalSeconds
        transform.rotation *= Quaternion.CreateFromAxisAngle(Vector3.Up, turnH * gametime.ElapsedGameTime.TotalSeconds) * Quaternion.CreateFromAxisAngle(Vector3.Right, turnV * gametime.ElapsedGameTime.TotalSeconds)
    End Sub

    Public Sub DrawUI(spriteBatch As SpriteBatch)
        spriteBatch.DrawString(GameCore.fonts("SegoeUI"), String.Format("Current Camera: {0}", Camera.Active.gameObject.Name), Vector2.Zero, Color.Black)
        spriteBatch.DrawString(GameCore.fonts("SegoeUI"), String.Format("Camera Position:{0}", transform.position), Vector2.UnitY * 10.0F, Color.Black)
        spriteBatch.DrawString(GameCore.fonts("SegoeUI"), String.Format("Camera Rotation:{0}", transform.rotation.GetEulerAngles()), Vector2.UnitY * 20.0F, Color.Black)
    End Sub

    Public Sub LoadXml(node As XmlElement)
        Dim moveSpeedNode = node.SelectSingleNode("MoveSpeed")
        Dim rotationSpeedNode = node.SelectSingleNode("RotationSpeed")

        If Not moveSpeedNode Is Nothing Then
            moveSpeed = Single.Parse(moveSpeedNode.InnerText)
        End If

        If Not rotationSpeedNode Is Nothing Then
            rotationSpeed = Single.Parse(rotationSpeedNode.InnerText)
        End If
    End Sub

End Class
