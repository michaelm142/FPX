Imports System.Xml
Imports Microsoft.Xna.Framework
Imports FPX

Public Class BoxTester
    Inherits Component

    Dim line As LineRenderer

    Private width As Single
    Private height As Single

    Dim region As Rect

    Public Sub Start()
        line = gameObject.AddComponent(Of LineRenderer)()
        line.positions.Add(New Vector3(region.X, region.Y, 0.0F))
        line.positions.Add(New Vector3(region.X + region.Width, region.Y, 0.0F))
        line.positions.Add(New Vector3(region.X + region.Width, region.Y + region.Height, 0.0F))
        line.positions.Add(New Vector3(region.X, region.Y + region.Height, 0.0F))
        line.positions.Add(New Vector3(region.X, region.Y, 0.0F))
        line.material.DiffuseColor = Color.Green
    End Sub

    Public Sub Update(gameTime As GameTime)
        region = New Rect(position.X, position.Y, width, height)
        If region.Contains(Input.mousePosition) Then
            line.Visible = True
        Else
            line.Visible = False
        End If
    End Sub

    Public Overrides Sub LoadXml(element As XmlElement)
        MyBase.LoadXml(element)

        Dim rectNode = element.SelectSingleNode("Rect")
        If Not rectNode Is Nothing Then
            Dim rect = Utill.RectFromXml(rectNode)
            width = rect.Width
            height = rect.Height
            region = rect
        End If
    End Sub
End Class