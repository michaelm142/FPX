Imports System
Imports System.Xml
Imports System.Linq
Imports Microsoft.Xna.Framework
Imports FPX

Public Class GameControlTest
    Inherits Component

    Public MoveSpeed As Single = 1.0F
    Public RotationSpeed As Single = 1.0F

    Private angle As Vector2
    Private startPosition As Vector3
    Private startRotation As Quaternion

    Public Sub Start()
        startPosition = transform.localPosition
        startRotation = transform.localRotation

        Dim euler = rotation.GetEulerAngles()
        angle.X = MathHelper.ToRadians(euler.Y)
        angle.Y = MathHelper.ToRadians(euler.X)
    End Sub

    Public Sub Update(ByVal gametime As GameTime)
        Dim t = gameObject.GetComponent(Of Transform)
        Dim Right = Input.GetAxis("Horizontal")
        Dim Forward = Input.GetAxis("Vertical")
        'Dim Up = gamepadstate.Triggers.Right - gamepadstate.Triggers.Left
        Dim turnX = Input.GetAxis("Pitch")
        Dim turnY = Input.GetAxis("Yaw")
        Dim vertical = Input.GetAxis("Up") + Input.GetAxis("Down")
        'Dim reset = gamepadstate.Buttons.Start = ButtonState.Pressed
        'Dim speed = If(gamepadstate.Buttons.A = ButtonState.Pressed, 5.0F, 1.0)

        'If (reset) Then
        '    t.localPosition = startPosition
        '    t.localRotation = startRotation
        'End If

        position += (t.worldPose.Right * Right + t.worldPose.Forward * Forward + t.transform.up * vertical) * gametime.ElapsedGameTime.TotalSeconds * MoveSpeed ' * speed

        angle.X += turnX * gametime.ElapsedGameTime.TotalSeconds * RotationSpeed
        angle.Y += turnY * gametime.ElapsedGameTime.TotalSeconds * RotationSpeed

        localRotation = Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(angle.Y), MathHelper.ToRadians(angle.X), 0.0F)

    End Sub

    Public Overrides Sub LoadXml(ByVal node As XmlElement)
        Dim moveSpeedNode = node.SelectSingleNode("MoveSpeed")
        Dim rotationSpeedNode = node.SelectSingleNode("RotationSpeed")

        If Not moveSpeedNode Is Nothing Then
            MoveSpeed = Single.Parse(moveSpeedNode.InnerText)
        End If

        If Not rotationSpeedNode Is Nothing Then
            RotationSpeed = Single.Parse(rotationSpeedNode.InnerText)
        End If
    End Sub
End Class
