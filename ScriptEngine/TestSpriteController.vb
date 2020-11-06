Imports System.Xml
Imports Microsoft.Xna.Framework
Imports FPX

Public Class TestSpriteController
    Inherits Component

    Public MoveSpeed As Single

    Public Sub Update(gameTime As GameTime)
        Dim vertical = Input.GetAxis("Vertical")
        Dim horizontal = Input.GetAxis("Horizontal")


        transform.localPosition += New Vector3(horizontal, vertical, 0.0F) * MoveSpeed * gameTime.TotalGameTime.Seconds
    End Sub

    Public Overrides Sub LoadXml(node As XmlElement)
        Dim speedNode = node.SelectSingleNode("MoveSpeed")
        If Not speedNode Is Nothing Then
            MoveSpeed = Single.Parse(speedNode.InnerText)
        End If
    End Sub

End Class
