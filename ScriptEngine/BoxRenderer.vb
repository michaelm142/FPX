Imports System.Xml
Imports Microsoft.Xna.Framework
Imports FPX

Public Class BoxRenderer
    Inherits Component

    Dim top As LineRenderer
    Dim bottom As LineRenderer

    Dim legFrontLeft As LineRenderer
    Dim legFrontRight As LineRenderer
    Dim legBackLeft As LineRenderer
    Dim legBackRight As LineRenderer

    Public Color As Color

    Public Sub Start()
        top = gameObject.AddComponent(Of LineRenderer)
        top.positions.Add(Vector3.Forward + Vector3.Left + Vector3.Up)
        top.positions.Add(Vector3.Forward + Vector3.Right + Vector3.Up)
        top.positions.Add(Vector3.Backward + Vector3.Right + Vector3.Up)
        top.positions.Add(Vector3.Backward + Vector3.Left + Vector3.Up)
        top.positions.Add(Vector3.Forward + Vector3.Left + Vector3.Up)

        bottom = gameObject.AddComponent(Of LineRenderer)
        bottom.positions.Add(Vector3.Forward + Vector3.Left + Vector3.Down)
        bottom.positions.Add(Vector3.Forward + Vector3.Right + Vector3.Down)
        bottom.positions.Add(Vector3.Backward + Vector3.Right + Vector3.Down)
        bottom.positions.Add(Vector3.Backward + Vector3.Left + Vector3.Down)
        bottom.positions.Add(Vector3.Forward + Vector3.Left + Vector3.Down)

        legFrontLeft = gameObject.AddComponent(Of LineRenderer)
        legFrontLeft.positions.Add(Vector3.Forward + Vector3.Left + Vector3.Up)
        legFrontLeft.positions.Add(Vector3.Forward + Vector3.Left + Vector3.Down)

        legFrontRight = gameObject.AddComponent(Of LineRenderer)
        legFrontRight.positions.Add(Vector3.Forward + Vector3.Right + Vector3.Up)
        legFrontRight.positions.Add(Vector3.Forward + Vector3.Right + Vector3.Down)

        legBackLeft = gameObject.AddComponent(Of LineRenderer)
        legBackLeft.positions.Add(Vector3.Backward + Vector3.Left + Vector3.Up)
        legBackLeft.positions.Add(Vector3.Backward + Vector3.Left + Vector3.Down)

        legBackRight = gameObject.AddComponent(Of LineRenderer)
        legBackRight.positions.Add(Vector3.Backward + Vector3.Right + Vector3.Up)
        legBackRight.positions.Add(Vector3.Backward + Vector3.Right + Vector3.Down)
    End Sub

    Public Sub Update(gameTime As GameTime)
        If top.material Is Nothing Then
            Return
        End If
        top.material.DiffuseColor = Color
        bottom.material.DiffuseColor = Color
        legBackLeft.material.DiffuseColor = Color
        legBackRight.material.DiffuseColor = Color
        legFrontLeft.material.DiffuseColor = Color
        legFrontRight.material.DiffuseColor = Color
    End Sub

    Public Overrides Sub LoadXml(node As XmlElement)
        Dim colorNode = node.SelectSingleNode("Color")
        If Not colorNode Is Nothing Then
            Color = LinearAlgebraUtil.ColorFromXml(colorNode)
        End If
    End Sub
End Class