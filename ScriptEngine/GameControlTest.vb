Imports System
Imports System.Xml
Imports System.Linq
Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Input
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
        Dim controllerIndex = Settings.GetSetting(Of String)("ControllerInput")
        Dim gamepadstate = GamePad.GetState([Enum].Parse(GetType(PlayerIndex), controllerIndex))
        Dim t = gameObject.GetComponent(Of Transform)
        Dim Right = gamepadstate.ThumbSticks.Left.X
        Dim Forward = gamepadstate.ThumbSticks.Left.Y
        Dim Up = gamepadstate.Triggers.Right - gamepadstate.Triggers.Left
        Dim turnX = -gamepadstate.ThumbSticks.Right.X * RotationSpeed
        Dim turnY = gamepadstate.ThumbSticks.Right.Y * RotationSpeed
        Dim reset = gamepadstate.Buttons.Start = ButtonState.Pressed
        Dim speed = If(gamepadstate.Buttons.A = ButtonState.Pressed, 5.0F, 1.0)

        If (reset) Then
            t.localPosition = startPosition
            t.localRotation = startRotation
        End If

        t.position += (t.worldPose.Right * Right + t.worldPose.Forward * Forward + Vector3.Up * Up) * gametime.ElapsedGameTime.TotalSeconds * MoveSpeed * speed

        angle.Y += turnX * gametime.ElapsedGameTime.TotalSeconds * RotationSpeed
        angle.X += turnY * gametime.ElapsedGameTime.TotalSeconds * RotationSpeed

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
