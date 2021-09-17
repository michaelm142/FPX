Imports System.Xml
Imports Microsoft.Xna.Framework
Imports FPX

Public Class SpaceshipControls
    Inherits Component

    Public moveSpeed As Single
    Public turnSpeed As Single

    Public Sub Update(gameTime As GameTime)
        Dim horizontal = Input.GetAxis("Horizontal")
        Dim vertical = Input.GetAxis("Vertical")
        Dim pitch = Input.GetAxis("Pitch")
        Dim yaw = Input.GetAxis("Yaw")

        Dim force = transform.right * horizontal * moveSpeed + transform.forward * vertical * moveSpeed
        Dim torque = Vector3.Up * yaw * turnSpeed + Vector3.Forward * pitch * turnSpeed

        Dim rigidbody = GetComponent(Of Rigidbody)()
        rigidbody.angularVelocity = torque
        rigidbody.AddForce(force)
    End Sub

    Public Overrides Sub LoadXml(element As XmlElement)
        MyBase.LoadXml(element)

        Dim moveSpeedNode = element.SelectSingleNode("MoveSpeed")
        Dim turnSpeedNode = element.SelectSingleNode("TurnSpeed")

        If Not moveSpeedNode Is Nothing Then
            moveSpeed = Single.Parse(moveSpeedNode.InnerText)
        End If

        If Not turnSpeedNode Is Nothing Then
            turnSpeed = Single.Parse(turnSpeedNode.InnerText)
        End If

    End Sub

End Class