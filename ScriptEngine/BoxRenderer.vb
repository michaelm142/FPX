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

    Public Property Color As Color
        Get
            Return top.material.DiffuseColor
        End Get
        Set(value As Color)
            top.material.DiffuseColor = value
            bottom.material.DiffuseColor = value
            legBackLeft.material.DiffuseColor = value
            legBackRight.material.DiffuseColor = value
            legFrontLeft.material.DiffuseColor = value
            legFrontRight.material.DiffuseColor = value
        End Set
    End Property

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

        legBackLeft = gameObject.AddComponent(Of LineRenderer)
        legBackLeft.positions.Add(Vector3.Backward + Vector3.Right + Vector3.Up)
        legBackLeft.positions.Add(Vector3.Backward + Vector3.Right + Vector3.Down)
    End Sub
End Class