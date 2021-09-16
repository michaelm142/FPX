Imports Microsoft.Xna.Framework
Imports System.Xml
Imports FPX

Public Class Spin
    Inherits Component

    Public axis As Vector3

    Public speed As Single

    Public Sub Update(gameTime As GameTime)
        transform.rotation *= Quaternion.CreateFromAxisAngle(axis, speed * Time.deltaTime)
    End Sub

    Public Overrides Sub LoadXml(node As XmlElement)
        Dim speedNode = node.SelectSingleNode("Speed")
        Dim axisNode = node.SelectSingleNode("Axis")
        If Not axisNode Is Nothing Then
            axis = LinearAlgebraUtil.Vector3FromXml(axisNode)
        End If
        If Not speedNode Is Nothing Then
            speed = Single.Parse(speedNode.InnerText)
        End If
    End Sub
End Class