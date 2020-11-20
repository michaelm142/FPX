Imports Microsoft.Xna.Framework
Imports FPX
Imports System.Xml

Public Class MoveAxisSin
    Inherits Component

    Public Axis As Vector3
    Private startPos As Vector3

    Public speed As Single
    Public lenght As Single

    Public Sub Start()
        startPos = transform.position
    End Sub

    Public Sub Update(gameTime As GameTime)
        transform.position = startPos + Axis * Math.Sin((gameTime.TotalGameTime.TotalMilliseconds / 1000.0F) * speed) * lenght
    End Sub

    Public Overrides Sub LoadXml(element As XmlElement)
        MyBase.LoadXml(element)

        Dim speedNode = element.SelectSingleNode("Speed")
        Dim lengthNode = element.SelectSingleNode("Length")
        Dim axisNode = element.SelectSingleNode("Axis")
        If Not axisNode Is Nothing Then
            Axis = LinearAlgebraUtil.Vector3FromXml(axisNode)
        Else
            Axis = Vector3.Up
        End If

        If Not speedNode Is Nothing Then
            speed = Single.Parse(speedNode.InnerText)
        End If
        If Not lengthNode Is Nothing Then
            lenght = Single.Parse(speedNode.InnerText)
        End If
    End Sub

End Class
