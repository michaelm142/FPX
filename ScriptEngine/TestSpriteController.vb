Imports System.Xml
Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Input
Imports Microsoft.Xna.Framework.Graphics
Imports ComponentModel

Public Class TestSpriteController
    Inherits Component

    Public MoveSpeed As Single

    Public Sub Update(gameTime As GameTime)
        Dim keyboard = Input.Keyboard.GetState()
        Dim vertical = 0.0F
        Dim horizontal = 0.0F
        If (keyboard.IsKeyDown(Keys.Up)) Then
            vertical += 1.0F
        ElseIf keyboard.IsKeyDown(Keys.Down) Then
            vertical -= 1.0F
        End If

        If keyboard.IsKeyDown(Keys.Left) Then
            horizontal -= 1.0F
        ElseIf keyboard.IsKeyDown(Keys.Right) Then
            horizontal += 1.0F
        End If

        transform.localPosition += New Vector3(horizontal, vertical, 0.0F) * MoveSpeed * gameTime.TotalGameTime.Seconds
    End Sub

    Public Sub LoadXml(node As XmlElement)
        Dim speedNode = node.SelectSingleNode("MoveSpeed")
        If Not speedNode Is Nothing Then
            MoveSpeed = Single.Parse(speedNode.InnerText)
        End If
    End Sub

End Class
