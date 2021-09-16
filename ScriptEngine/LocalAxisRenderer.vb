Imports FPX
Imports Microsoft.Xna.Framework

Public Class LocalAxisRenderer
    Inherits Component

    Private upLine As LineRenderer
    Private rightLine As LineRenderer
    Private frontLine As LineRenderer

    Public Sub Start()
        If Not Graphics.Mode = "Default" Then
            Debug.LogError("Local Axis Renderer is not designed to work outside of Default rendering mode")
        End If

        upLine = gameObject.AddComponent(Of LineRenderer)
        rightLine = gameObject.AddComponent(Of LineRenderer)
        frontLine = gameObject.AddComponent(Of LineRenderer)

        frontLine.positions.Add(Vector3.Zero)
        frontLine.positions.Add(Vector3.Forward)

        rightLine.positions.Add(Vector3.Zero)
        rightLine.positions.Add(Vector3.Right)

        upLine.positions.Add(Vector3.Zero)
        upLine.positions.Add(Vector3.Up)
    End Sub

    Public Sub Update(gameTime As GameTime)
        If upLine.material Is Nothing Then
            Return
        End If
        upLine.material.DiffuseColor = Color.Lime
        rightLine.material.DiffuseColor = Color.Red
        frontLine.material.DiffuseColor = Color.Blue
    End Sub
End Class